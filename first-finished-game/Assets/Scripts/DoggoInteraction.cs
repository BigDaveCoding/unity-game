using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Xml;

public class DoggoInteraction : MonoBehaviour
{

    private DoggoMovement doggoMovement;
    private bool playerInArea;
    private PlayerController playerCont;
    private Inventory inventory;

    public GameObject canvas;
    public TextMeshProUGUI dialogue;
    public Image npcImage;
    public TextMeshProUGUI npcName;
    public NPCInfo npcInfo;
    public InteractionText interactionText;
    public GameObject exitButton;
    public float textSpeed = 0.03f;
    public AudioSource typingAudio;

    //need scriptable object for npcInfo including name, icon and description
    //need to set up interactionText with doggo text
    //need to set up canvas to load which will contain UI for interaction.

    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        playerCont = FindObjectOfType<PlayerController>();
        doggoMovement = FindObjectOfType<DoggoMovement>();
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInArea && playerCont.isInteracting && !canvas.activeInHierarchy)
        {
            playerCont.isInteracting = false; // This seems to have fixed the problem of the update function executing twice.
            //StartCoroutine(NoItemStartInteraction());
            if (inventory.ContainsItem("Tea Bag"))
            {
                Debug.Log("Player has teabag in inventory");
                return;

            }
            else
            {
                Debug.Log("Player does not have tea bag in inventory");
                StartCoroutine(NoItemStartInteraction());
            }
        }
    }

    private IEnumerator NoItemStartInteraction()
    {
        //doggoMovement.npcCanMove = false;
        //doggoMovement.animator.SetBool("isMoving", false);
        //playerCont.SetInteractionBools();
        doggoMovement.playerInteracting = true;
        exitButton.SetActive(false);

        playerCont.playerCanInteract = false;
        playerCont.playerCanMove = false;
        playerCont.playerInUI = true;

        yield return null;
        //Debug.Log($"Doggo interaction started.\n playerCanInteract is {playerCont.playerCanInteract}" +
        //    $" \n playerCanMove is {playerCont.playerCanMove} \n playerInUI is {playerCont.playerInUI}");

        canvas.SetActive(true);
        npcImage.sprite = npcInfo.npcImage;
        npcName.text = npcInfo.npcName;
        yield return StartCoroutine(TypeText(interactionText.textToDisplay[0]));
        exitButton.SetActive(true);

    }

    

    private IEnumerator TypeText(string text)
    {
        dialogue.text = ""; //reset text to ""
        typingAudio.Play();

        for (int i = 0; i < text.Length; i++)
        {
            dialogue.text += text[i];
            yield return new WaitForSeconds(textSpeed);
        }
        typingAudio.Stop();
        yield return null;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInArea = true;
            //Debug.Log("Player is in doggo area to interact");
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInArea = false;
            //Debug.Log("Player has left area of interaction with doggo");
        }
    }

    public void ExitButton()
    {
        canvas.SetActive(false);
        doggoMovement.playerInteracting = false;
        doggoMovement.npcCanMove = true;
        //playerCont.playerCanInteract = true;
        //playerCont.playerCanMove = true;
        //playerCont.playerInUI = false;
        playerCont.resetInteractionBools();
    }
}
