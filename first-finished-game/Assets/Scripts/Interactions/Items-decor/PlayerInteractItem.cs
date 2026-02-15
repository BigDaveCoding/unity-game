using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractItem : MonoBehaviour
{

    public Item item; //using scriptable object to set variables so dont have to repeat every time for new items.
    public InteractionText interactionText; //scriptable object text to display.
    public GameObject uiCanvas;
    public TextMeshProUGUI canvasText;
    public float textSpeed = 0.05f;
    public GameObject exitButton;
    public AudioSource audioSource;
    public AudioClip typingAudio;

    private Inventory inventory;
    private PlayerController playerController;
    private bool playerInArea;


    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        playerController = FindObjectOfType<PlayerController>();
        uiCanvas.SetActive(false);

    }

    private void Update()
    {
        if (playerInArea && playerController.isInteracting && !uiCanvas.activeInHierarchy)
        {
            StartCoroutine(PickUpItem());
        }
        else
        {
            return;
        }
    }

    private IEnumerator PickUpItem()
    {
        exitButton.SetActive(false);
        playerController.playerCanMove = false;
        playerController.playerInUI = true;
        playerController.playerCanInteract = false;
        inventory.AddItem(item);
        uiCanvas.SetActive(true);
        yield return null;
        //StartCoroutine(DestroyGameObjectAfterText());
        audioSource.clip = typingAudio;
        audioSource.Play();

        yield return StartCoroutine(TypeText(interactionText.textToDisplay[0]));
        audioSource.Stop();
        gameObject.SetActive(false);
        exitButton.SetActive(true);
        
    }

    
    private IEnumerator TypeText(string text)
    {
        canvasText.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            canvasText.text += text[i];
            yield return new WaitForSeconds(textSpeed);
        }

    }

    //private IEnumerator DestroyGameObjectAfterText()
    //{
    //    yield return StartCoroutine(TypeText(interactionText.textToDisplay[0]));
    //    gameObject.SetActive(false);
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerInArea = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInArea = false;
    }
    public void ExitButton()
    {
        uiCanvas.SetActive(false);
        playerController.playerCanMove = true;
        playerController.playerInUI = false;
        playerController.playerCanInteract = true;
    }
}
