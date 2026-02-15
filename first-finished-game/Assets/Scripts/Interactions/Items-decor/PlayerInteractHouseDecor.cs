using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInteractHouseDecor : MonoBehaviour
{
    public Item item; //using scriptable object to set variables so dont have to repeat every time for new items.
    public InteractionText interactionText; //scriptable object text to display.
    public GameObject uiCanvas;
    public TextMeshProUGUI canvasText;
    public float textSpeed = 0.05f;
    public bool decorHasItem;
    public Button exitButton;
    public AudioSource typingAudio;
    

    private Inventory inventory;
    private PlayerController playerController;
    private bool playerInArea;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        playerController = FindObjectOfType<PlayerController>();
        uiCanvas.SetActive(false);
        decorHasItem = true;
    }

    private void Update()
    {
        if (playerInArea && playerController.isInteracting && !uiCanvas.activeInHierarchy)
        {
            StartCoroutine(OpenCanvas());
        }
        else
        {
            return;
        }
    }

    private IEnumerator OpenCanvas()
    {
        exitButton.gameObject.SetActive(false);
        playerController.playerInUI = true;
        playerController.playerCanMove = false;
        playerController.playerCanInteract = false;
        uiCanvas.SetActive(true);

        if (decorHasItem)
        {
            Debug.Log("decor has item so will display interaction text [0]");
            Debug.Log("opening canvas, setting playerinUI to true and canMove to false");
            
            yield return StartCoroutine(TypeText(interactionText.textToDisplay[0]));
            decorHasItem = false;
            inventory.AddItem(item);
            exitButton.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("decor does not have an item. displaying interaction text [1]");
            yield return StartCoroutine(TypeText(interactionText.textToDisplay[1]));
            exitButton.gameObject.SetActive(true);
            
        }
        
    }

    public IEnumerator TypeText(string text)
    {
        canvasText.text = "";
        typingAudio.Play();
        for (int i = 0; i < text.Length; i++)
        {
            canvasText.text += text[i];
            yield return new WaitForSeconds(textSpeed);
        }
        typingAudio.Stop();
        yield return null;
        
    }

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
