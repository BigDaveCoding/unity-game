using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BookcaseInteractionHouse : MonoBehaviour
{

    private PlayerController playerController; //Need isInteracting bool from player controller

    public GameObject CanvasToDisplay; //Canvas which contains UI.

    public bool playerInArea; //bool controlled by boxcollider2d trigger to say whether the player is within the bounds to interact with gameobject
    public string[] textToDisplay;
    public TextMeshProUGUI tmpText;
    public float textSpeed;

    public Button[] buttonsToEnable;
    public Button exitButton;

    private bool finishedText;
    private bool openingSecretDoor;
    private bool secretDoorOpened;

    public GameObject secretDoor;

    public GameObject storyCanvas;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI storyText;
    public TextMeshProUGUI authorText;

    public AudioSource typingAudio;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>(); //assinging playercontroller.
        //Debug.Log(playerController);
        CanvasToDisplay.SetActive(false); //Making sure the canvas is set to false when the game starts.
        secretDoor.SetActive(false);
        openingSecretDoor = false;
        finishedText = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerInArea && playerController.isInteracting && !CanvasToDisplay.activeInHierarchy)
        {
            StartCoroutine(OpenCanvas());
        }
        else
        {
            return;
        }
        
        //if(finishedText && !openingSecretDoor)
        //{
        //    EnableButtons();
        //}
        //else
        //{
        //    DisableButtons();
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerInArea = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInArea = false;
    }

    private IEnumerator OpenCanvas()
    {
        CanvasToDisplay.SetActive(true);
        playerController.playerCanMove = false;
        playerController.playerInUI = true;
        playerController.playerCanInteract = false;
        finishedText = false;
        DisableButtons();
        exitButton.gameObject.SetActive(false);
        yield return null;

        yield return StartCoroutine(TypeText(textToDisplay[0]));
        exitButton.gameObject.SetActive(true);
        EnableButtons();
        
    }

    private void EnableButtons()
    {
        foreach (var button in buttonsToEnable)
        {
            button.gameObject.SetActive(true);
        }
    }
    private void DisableButtons()
    {
        foreach (var button in buttonsToEnable)
        {
            button.gameObject.SetActive(false);
        }
        
    }


    public void ExitButton()
    {
        CanvasToDisplay.SetActive(false);
        playerController.playerCanMove = true;
        playerController.playerInUI = false;
        playerController.playerCanInteract = true;
    }

    private IEnumerator TypeText(string text)
    {
        tmpText.text = ""; //reset text to ""
        typingAudio.Play();

        for (int i = 0; i < text.Length; i++)
        {
            tmpText.text += text[i].ToString();
            yield return new WaitForSeconds(textSpeed);
        }
        typingAudio.Stop();
        finishedText = true;
        yield return null;
    }

    public void SecretDoorButton()
    {
        if(!secretDoorOpened)
        {
            finishedText = false;
            DisableButtons();
            StartCoroutine(SecretDoorTwo());
        }
        else
        {
            Debug.Log("secret door already opened");
            return;
        }
    }

    private IEnumerator SecretDoorTwo()
    {
        string[] secretDoorText = { "*Click*", "A secret door!" };
        tmpText.text = "";

        exitButton.gameObject.SetActive(false);
        openingSecretDoor = true;
        yield return StartCoroutine(TypeText(secretDoorText[0]));

        secretDoor.SetActive(true);
        AnimatorStateInfo stateInfo = secretDoor.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        while(stateInfo.normalizedTime < 1f)
        {
            stateInfo = secretDoor.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
        Debug.Log("Secret door animation complete");

        yield return StartCoroutine(TypeText(secretDoorText[1]));

        openingSecretDoor = false;
        secretDoorOpened = true;
        exitButton.gameObject.SetActive(true);

    }

    
    //private IEnumerator SecretDoor()
    //{
    //    string[] secretDoorText = {"*Click*", "A secret door!"};
    //    tmpText.text = "";

    //    openingSecretDoor = true;
    //    StartCoroutine(TypeText(secretDoorText[0]));
    //    yield return new WaitForSeconds(1f);

    //    secretDoor.SetActive(true);
    //    yield return new WaitForSeconds(2f);

    //    StartCoroutine(TypeText(secretDoorText[1]));
    //    yield return new WaitForSeconds(10f);

    //    openingSecretDoor = false;
    //    secretDoorOpened = true;
    //    CanvasToDisplay.SetActive(false);
    //    playerController.playerCanMove = true;
    //    playerController.playerInUI = false;
    //}

    public void BookOneButton()
    {
        string title = "The Three Doggos";
        string story = "Three dogs, Max, Bella, and Rocky, wrestled for a tattered tennis ball in the park. Their playful tugs turned into a tussle," +
            " until suddenly, Max's paws left the ground. Astonished, his friends watched as Max sprouted magnificent wings. With a joyful yip, he soared into the sky," +
            " leaving Bella and Rocky barking in amazement.\n\nMax's wings carried him high above the park, where he soared among the clouds," +
            " a joyful grin on his furry face. As he disappeared into the horizon, Bella and Rocky watched, awestruck. From that day on, they shared the tattered tennis ball," +
            " knowing that sometimes, dreams could lift you higher than you ever imagined.";
        string author = "Written by: Bubba";

        CanvasToDisplay.SetActive(false);
        storyCanvas.SetActive(true);
        titleText.text = title;
        storyText.text = story;
        authorText.text = author;
        //StartCoroutine(StoryTextType(story));
    }

    public void BookTwoButton()
    {
        string title = "The Cold Candles";
        string story = "In a cozy candle shop nestled on a quiet street, a peculiar problem arose." +
            " The candles had caught a chilly draft, and their flickering flames shivered." +
            " The shopkeeper, Mrs. Thompson, knew she had to help." +
            "\n\nSo, she picked up her knitting needles and began crafting tiny jumpers for each candle." +
            " With great care, she stitched woolen garments, adorning them with patterns and colors." +
            " As she dressed each candle, their flames danced with delight, and the shop sparkled with warmth." +
            "\n\nThe candles no longer caught cold, and customers marveled at their snug attire." +
            " Mrs. Thompson's little knitted jumpers turned her shop into a haven of both light and coziness, where even candles could stay warm on a chilly night.";
        string author = "Written by: Mac";

        CanvasToDisplay.SetActive(false);
        storyCanvas.SetActive(true);
        titleText.text = title;
        storyText.text = story;
        authorText.text = author;

    }

    public void BookThreeButton()
    {
        string title = "Raven";
        string story = "In the depths of a dense forest, there lived a raven named Rufus." +
            " He had feathers as dark as the midnight sky and a spirit as fierce as a lion's." +
            " Rufus was convinced he was a lion, despite the bewildered chirps of his feathered friends." +
            "\n\nPerched on the highest branches, Rufus would let out a mighty \"roar,\" a caw that echoed through the woods." +
            " He patrolled his territory, protecting it from imaginary threats." +
            "\n\nOne day, as Rufus spread his ebony wings and let out another \"roar,\" a real lion strolled by." +
            " Rufus froze, and for the first time, he felt small." +
            " Yet, the lion merely yawned, acknowledging Rufus's wild spirit with a knowing nod before continuing on its way." +
            "\n\nFrom that day forward, Rufus, the lion-hearted raven, learned that bravery came in all shapes and sizes," +
            " and sometimes the mightiest roars came from the unlikeliest of souls.";
        string author = "Written by: Cara & Joy";

        CanvasToDisplay.SetActive(false);
        storyCanvas.SetActive(true);
        titleText.text = title;
        storyText.text = story;
        authorText.text = author;

    }

    public void StoryExitButton()
    {
        storyCanvas.SetActive(false);
        playerController.playerCanMove = true;
        playerController.playerInUI = false;
        playerController.playerCanInteract = true;
    }

    //private IEnumerator StoryTextType(string text)
    //{
    //    storyText.text = "";
    //    for (int i = 0; i < text.Length; i++)
    //    {
    //        storyText.text += text[i];
    //        yield return new WaitForSeconds(textSpeed);
    //    }
        
    //}
}
