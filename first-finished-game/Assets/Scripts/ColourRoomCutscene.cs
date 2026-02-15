using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ColourRoomCutscene : MonoBehaviour
{
    DoorInteraction doorInteraction;
    public GameObject assignTransitionCanvas;
    public Animator assignTransitionAnimator;
    public Vector3 moveCameraPos;
    public Vector3 movePlayerPos;

    public GameObject dialogueCanvas;
    public InteractionText interactionText;
    public TextMeshProUGUI dialogueText;
    public NPCInfo npcInfo;
    public Image npcImage;
    public TextMeshProUGUI npcName;
    public Animator cauldronAnimator;
    public GameObject exitButton;
    public GameObject continueText;
    public AudioSource typingAudio;

    public Vector3[] playerMovementPos;

    public bool chestUnlocked = false;

    public GameObject wizardNPC;
    public Animator wizardAnimator;
    public NPCInfo wizardNPCInfo;
    public InteractionText wizardDialogue;
    public Vector3[] teleportLocations;
    //private SpriteRenderer wizardSpriteRen;

    public bool broomMove = false;
    public GameObject spellOne;

    public Vector3 endTransitionCameraPos;
    public Vector3 endTransitionPlayerPos;

    public GameObject[] portalsToDisable;
    public GameObject activateChest;

    public GameObject portalSetInactive;
    public GameObject chestSetActive;
    

    //public bool animateBroom;
    //public Animator broomAnimator;

    private GameObject player;
    private PlayerController playerCont;
    private bool playerInArea;
    private float textSpeed = 0.05f;

    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        doorInteraction = FindObjectOfType<DoorInteraction>();
        playerCont = FindObjectOfType<PlayerController>();
        exitButton.SetActive(false);
        dialogueCanvas.SetActive(false);
        npcName.text = "?????";
        //wizardSpriteRen = wizardNPC.GetComponent<SpriteRenderer>();
        wizardNPC.SetActive(false);
        spellOne.SetActive(false);
        activateChest.SetActive(false);
        portalSetInactive.SetActive(true);
        
        //animateBroom = false;
        //npcImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInArea && playerCont.isInteracting)
        {
            playerCont.isInteracting = false;
            StartCoroutine(Cutscene());
        }
    }

    private IEnumerator Cutscene()
    {
        doorInteraction.transitionCanvas = assignTransitionCanvas;
        doorInteraction.transitionAnimator = assignTransitionAnimator;
        doorInteraction.cameraMoveToPos = moveCameraPos;
        doorInteraction.playerMoveToPos = movePlayerPos;
        yield return null;
        yield return StartCoroutine(doorInteraction.EnteredTransition());
        playerCont.playerCanMove = false;
        Debug.Log("transition finished");
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FirstScene());
        yield return StartCoroutine(wizardEnter());
        yield return StartCoroutine(WizardText());
        yield return StartCoroutine(WizardTeleport(teleportLocations[0]));
        yield return StartCoroutine(TypeText(wizardDialogue.textToDisplay[1]));
        yield return StartCoroutine(WizardTeleport(teleportLocations[1]));
        yield return StartCoroutine(TypeText(wizardDialogue.textToDisplay[2]));
        yield return StartCoroutine(WizardTeleport(teleportLocations[2]));
        yield return StartCoroutine(TypeText(wizardDialogue.textToDisplay[3]));
        yield return StartCoroutine(TypeText(wizardDialogue.textToDisplay[4]));
        yield return StartCoroutine(TypeText(wizardDialogue.textToDisplay[5]));
        dialogueCanvas.SetActive(false);
        broomMove = true;
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(TypeText(wizardDialogue.textToDisplay[6]));
        yield return StartCoroutine(TypeText(wizardDialogue.textToDisplay[7]));
        yield return StartCoroutine(TypeText(wizardDialogue.textToDisplay[8]));
        yield return StartCoroutine(TypeText(wizardDialogue.textToDisplay[9]));
        yield return StartCoroutine(TypeText(wizardDialogue.textToDisplay[10]));
        yield return StartCoroutine(CastSpellOne());
        yield return StartCoroutine(EndTrnasition());



    }

    private IEnumerator FirstScene()
    {
        Debug.Log("FirstScene Coroutine Started");
        dialogueCanvas.SetActive(true);
        yield return StartCoroutine(TypeText(interactionText.textToDisplay[0]));
        yield return StartCoroutine(PlayerLookAround());
        yield return StartCoroutine(TypeText(interactionText.textToDisplay[1]));
        yield return StartCoroutine(MovePlayer(0));
        yield return StartCoroutine(PlayerLookAround());
        yield return StartCoroutine(TypeText(interactionText.textToDisplay[2]));
        yield return StartCoroutine(MovePlayer(1));
        yield return StartCoroutine(CauldronWakeUp());
        yield return StartCoroutine(TypeText(interactionText.textToDisplay[3]));
        yield return StartCoroutine(TypeText(interactionText.textToDisplay[4]));
        //animateBroom = true;
        yield return StartCoroutine(TypeText(interactionText.textToDisplay[5]));
        yield return StartCoroutine(TypeText(interactionText.textToDisplay[6]));
        yield return StartCoroutine(TypeText(interactionText.textToDisplay[7]));
        chestUnlocked = true;
        yield return StartCoroutine(TypeText(interactionText.textToDisplay[8]));
        dialogueCanvas.SetActive(false);
        yield return StartCoroutine(CauldronFallAsleep());
        playerCont.animator.SetFloat("y", -1);
        playerCont.animator.SetFloat("x", 0); // make the player look down towards screen
        //first scene finished
        portalSetInactive.SetActive(false); //setting portal in secret b&W room to false active so player can no longer use portal
        chestSetActive.SetActive(true); //setting chest true active so player can collect the coffee beans



    }

    private IEnumerator PlayerLookAround()
    {
        dialogueCanvas.SetActive(false);
        playerCont.animator.SetBool("isMoving", false);
        playerCont.animator.SetFloat("y", 0);
        playerCont.animator.SetFloat("x", 1);
        yield return new WaitForSeconds(1f);
        playerCont.animator.SetFloat("x", -1);
        yield return new WaitForSeconds(1f);
        playerCont.animator.SetFloat("x", 0);
        playerCont.animator.SetFloat("y", -1);
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator MovePlayer(int movePos)
    {
        dialogueCanvas.SetActive(false);
        playerCont.animator.SetBool("isMoving", true);
        Vector3 targetPos = playerMovementPos[movePos];
        Vector3 direction = (targetPos - player.transform.position).normalized;
        playerCont.animator.SetFloat("x", direction.x);
        playerCont.animator.SetFloat("y", direction.y);

        while(player.transform.position != targetPos)
        {
            Vector3 newPos = player.transform.position + direction * (playerCont.moveSpeed / 10) * Time.fixedDeltaTime;
            // Make sure the player doesn't overshoot the target position
            if (Vector3.Distance(newPos, targetPos) < Vector3.Distance(player.transform.position, targetPos))
            {
                player.transform.position = newPos;
            }
            else
            {
                player.transform.position = targetPos;
                playerCont.animator.SetBool("isMoving", false);
            }

            // Wait for the next frame
            yield return null;
        }
    }

    private IEnumerator TypeText(string text)
    {
        if (!dialogueCanvas.activeInHierarchy)
        {
            dialogueCanvas.SetActive(true);
        }

        continueText.SetActive(false);
        dialogueText.text = "";
        typingAudio.Play();
        for (int i = 0; i < text.Length; i++)
        {
            dialogueText.text += text[i];
            yield return new WaitForSeconds(textSpeed);
        }
        continueText.SetActive(true);
        typingAudio.Stop();
        yield return StartCoroutine(WaitForPlayerInput());
    }

    private IEnumerator WaitForPlayerInput()
    {
        Debug.Log("Waiting for player to press enter");
        while(!Input.GetKeyDown(KeyCode.KeypadEnter) && !Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        Debug.Log("Player pressed enter");
    }

    private IEnumerator CauldronWakeUp()
    {
        cauldronAnimator.SetBool("wakeUp", true);
        yield return new WaitForSeconds(2f);
        Debug.Log("Cauldron Woke Up");
        npcName.text = npcInfo.npcName;
        npcImage.sprite = npcInfo.npcImage;
        npcImage.color = Color.white;
        yield return null;
    }
    private IEnumerator CauldronFallAsleep()
    {
        cauldronAnimator.SetBool("wakeUp", false);
        cauldronAnimator.SetTrigger("fallAsleep");
        yield return new WaitForSeconds(2f);
        cauldronAnimator.SetTrigger("asleep");

    }

    private IEnumerator wizardEnter()
    {
        wizardNPC.SetActive(true);
        wizardAnimator.SetFloat("x", -1);
        wizardAnimator.SetBool("enter", true);
        Debug.Log(wizardAnimator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(wizardAnimator.GetCurrentAnimatorStateInfo(0).length);
        wizardAnimator.SetBool("enter", false);
        yield return new WaitForSeconds(1f); //wait for a second to allow animation frames.
        
    }

    private IEnumerator WizardTeleport(Vector3 teleportLocation)
    {
        if (dialogueCanvas.activeInHierarchy)
        {
            dialogueCanvas.SetActive(false);
        }

        Vector3 faceDirectionVector3 = (player.transform.position - teleportLocation).normalized;
        float faceDirection = faceDirectionVector3.x;
        Debug.Log($"face direction x is : {faceDirection}");
        Debug.Log("Wizard teleporting");
        
        wizardAnimator.SetBool("exit", true);
        yield return null;
        Debug.Log($"length of exit animation is: {wizardAnimator.GetCurrentAnimatorStateInfo(0).length}");
        yield return new WaitForSeconds(wizardAnimator.GetCurrentAnimatorStateInfo(0).length);
        wizardNPC.SetActive(false);
        wizardNPC.transform.position = teleportLocation;
        yield return null;
        wizardNPC.SetActive(true);
        wizardAnimator.SetFloat("x", faceDirection);
        wizardAnimator.SetBool("enter", true);
        yield return new WaitForSeconds(wizardAnimator.GetCurrentAnimatorStateInfo(0).length);
        //wizardAnimator.SetFloat("x", faceDirection);
        wizardAnimator.SetBool("enter", false);

    }

    private IEnumerator WizardText()
    {
        npcName.text = wizardNPCInfo.npcName;
        npcImage.sprite = wizardNPCInfo.npcImage;
        yield return null;
        yield return StartCoroutine(TypeText(wizardDialogue.textToDisplay[0]));
        Debug.Log("First wizard text complete");
    }

    private IEnumerator CastSpellOne()
    {
        dialogueCanvas.SetActive(false);
        spellOne.transform.position = player.transform.position + new Vector3(0f, 0.5f, 0f);
        spellOne.SetActive(true);
        AnimatorStateInfo stateInfo = spellOne.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        yield return null;
        yield return new WaitForSeconds(stateInfo.length);
        player.SetActive(false);
        spellOne.SetActive(false); 
    }

    private IEnumerator EndTrnasition()
    {
        doorInteraction.cameraMoveToPos = endTransitionCameraPos;
        doorInteraction.playerMoveToPos = endTransitionPlayerPos;
        foreach (GameObject portals in portalsToDisable)
        {
            portals.SetActive(false);
        }
        activateChest.SetActive(true);
        yield return StartCoroutine(doorInteraction.EnteredTransition());
        yield return StartCoroutine(CastSpellOne());
        player.SetActive(true);
        playerCont.resetInteractionBools();
        

        //reseting the vector 3 values on the doorinteraction script as it changes throughout the cutscene.
        doorInteraction.cameraMoveToPos = new Vector3(0f, 0f, -10f);
        doorInteraction.playerMoveToPos = new Vector3(1f, -3f, -1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerInArea = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInArea = false;
    }
}
