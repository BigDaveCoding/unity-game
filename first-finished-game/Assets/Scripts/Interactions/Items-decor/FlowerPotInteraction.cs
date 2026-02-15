using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FlowerPotInteraction : MonoBehaviour
{
    private Inventory inventory;
    private PlayerController playerController;
    private bool playerInArea;
    private bool flowerNotWatered = true;

    public GameObject canvasFlowerPot;
    public TextMeshProUGUI canvasText;
    public InteractionText interactionText;
    public float textSpeed = 0.03f;
    public GameObject cowJuiceButton;
    public GameObject exitButton;
    public Animator flowerAnimator;
    public Item item;
    public AudioSource typingAudio;


    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        playerController = FindObjectOfType<PlayerController>();
        cowJuiceButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInArea && playerController.isInteracting && !canvasFlowerPot.activeInHierarchy)
        {
            if(!inventory.ContainsItem("Flower Petals"))
            {
                StartCoroutine(StartInteraction());
            }
            else
            {
                return;
            }
        }
    }

    private IEnumerator StartInteraction()
    {
        exitButton.SetActive(false);
        canvasFlowerPot.SetActive(true);
        playerController.playerInUI = true;
        playerController.playerCanMove = false;
        yield return null;

        if (inventory.ContainsItem("Cow Juice") && flowerNotWatered)
        {
            //Debug.Log("Player has Item in inventory to commence interaction");
            //StartCoroutine(TypeTextAndActivateButton(interactionText.textToDisplay[1]));
            yield return StartCoroutine(TypeText(interactionText.textToDisplay[1]));
            cowJuiceButton.SetActive(true);
            exitButton.SetActive(true);

        }
        else
        {
            Debug.Log("Player does not have the neccesary item in inventory so will display generic text");
            yield return StartCoroutine(TypeText(interactionText.textToDisplay[0]));
            exitButton.SetActive(true);
        }
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
        canvasFlowerPot.SetActive(false);
        playerController.playerInUI = false;
        playerController.playerCanMove = true;
        cowJuiceButton.SetActive(false);
    }
    private IEnumerator TypeText(string text)
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
    //private IEnumerator TypeTextAndActivateButton(string text)
    //{
    //    yield return StartCoroutine(TypeText(text)); // Wait for the text typing coroutine to finish
    //    cowJuiceButton.SetActive(true); // Activate the button after the text typing coroutine finishes
    //    exitButton.SetActive(true);
    //}

    public void UseCowJuiceButton()
    {
        StartCoroutine(FlowerGrowth());
        
    }

    private IEnumerator FlowerGrowth()
    {
        flowerNotWatered = false;
        flowerAnimator.SetBool("Growing", true);
        canvasFlowerPot.SetActive(false);
        cowJuiceButton.SetActive(false);
        yield return null;

        AnimatorStateInfo stateInfo = flowerAnimator.GetCurrentAnimatorStateInfo(0);

        while(stateInfo.normalizedTime < 1f)
        {
            stateInfo = flowerAnimator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
        
        Debug.Log("Flower Growth Animation is complete");
        canvasFlowerPot.SetActive(true);
        exitButton.SetActive(false);
        
        yield return StartCoroutine(TypeText(interactionText.textToDisplay[2]));
        inventory.AddItem(item);
        exitButton.SetActive(true);

    }
}
