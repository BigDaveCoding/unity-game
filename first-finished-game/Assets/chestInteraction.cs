using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class chestInteraction : MonoBehaviour
{
    public PlayerController playerController;
    public Inventory inv;
    public Item itemToAdd;
    private bool playerInArea;
    private bool chestOpened;

    public GameObject canvasChest;
    public GameObject exitButton;
    public TextMeshProUGUI canvasText;
    public InteractionText interactionText;
    public float textSpeed = 0.03f;
    public AudioSource typingAudio;

    // Start is called before the first frame update
    void Start()
    {
        chestOpened = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerInArea && playerController.isInteracting && !chestOpened)
        {
            canvasChest.SetActive(true);
            
            playerController.playerInUI = true;
            playerController.playerCanMove = false;

            inv.AddItem(itemToAdd);
            Debug.Log("coffee beans added to inventory");
            chestOpened = true;
            StartCoroutine(TypeText(interactionText.textToDisplay[0]));

            exitButton.SetActive(true);


        }
        
    }

    public void ExitButton()
    {
        canvasChest.SetActive(false);
        playerController.playerInUI = false;
        playerController.playerCanMove = true;
    }

    private IEnumerator TypeText(string text)
    {
        exitButton.SetActive(false);
        canvasText.text = ""; //reset textbox to ""
        typingAudio.Play(); // start playing typing audio
        for (int i = 0; i < text.Length; i++) //if i is less that length of text, add 1
        {
            canvasText.text += text[i]; // adding text[index] to textbox ""
            yield return new WaitForSeconds(textSpeed); //adds a letter every second set by textspeed variable
        }
        //loop will run until text is typed
        typingAudio.Stop(); // stop typing audio
        yield return null;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerInArea = true;
        Debug.Log("player is in area");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInArea = false;
    }
}
