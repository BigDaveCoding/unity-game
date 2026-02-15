using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoffeeMachine : MonoBehaviour
{

    public Slider steamSlider;
    [SerializeField] private bool isSteaming;
    [SerializeField] private float gravityEffect = 1f;
    [SerializeField] private float steamValue = 1f;
    [SerializeField] private float steamTimeInTarget = 5f;
    [SerializeField] private float minSteamTargetValue = 0.55f;
    [SerializeField] private float maxSteamTargetValue = 0.85f;

    private Inventory inv;
    //public BoxCollider2D inAreaBounds;

    public GameObject SliderCanvas;
    public GameObject TextCanvas;
    public TextMeshProUGUI textDialogue;
    

    [SerializeField] private bool playerInArea;
    [SerializeField] private bool playerInteractingMachine;

    private PlayerController playerCont;

    public Slider targetSlider;

    // Start is called before the first frame update
    void Start()
    {
        inv = FindObjectOfType<Inventory>();
        playerCont = FindObjectOfType<PlayerController>();
        SliderCanvas.SetActive(false);
        TextCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        MoveTargetArea();
        StartCoffeeMachine();

        if (playerInteractingMachine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isSteaming = true;
                Debug.Log("Player is pressing l and steaming");
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isSteaming = false;
                Debug.Log("Player let go of l and is not steaming");
            }
        }

        if (isSteaming)
        {
            PlayerSteamingMilk();
        }
        if (!isSteaming)
        {
            steamSlider.value -= gravityEffect * Time.smoothDeltaTime;
        }

        if(steamSlider.value > minSteamTargetValue && steamSlider.value < maxSteamTargetValue)
        {
            Debug.Log("slider in target area");
            steamTimeInTarget -= Time.deltaTime;
            Debug.Log($"Time in target zone left is: {steamTimeInTarget}");
            int steamTimeInt = (int)steamTimeInTarget;
            textDialogue.text = "Press 'Spacebar' to steam." +
                "\nKeep inside the target area for 5 seconds!" +
                "\n\n Steam Timer: " + steamTimeInt;
        }
        else
        {
            steamTimeInTarget = 5f;

        }

        if(steamTimeInTarget <= 0f)
        {
            FinishSteaming();
        }
    }

    private void StartCoffeeMachine()
    {
        if (inv.ContainsItem("Coffee Beans") && playerCont.isInteracting && playerInArea && !SliderCanvas.activeInHierarchy)
        {
            playerCont.SetInteractionBools();
            SliderCanvas.SetActive(true);
            TextCanvas.SetActive(true);
            textDialogue.text = "Press 'Spacebar' to steam the cow juice" +
                "\nKeep inside the target are for 5 seconds!" +
                "\n\n Steam Timer: " + steamTimeInTarget.ToString();
            playerInteractingMachine = true;
        }
    }

    private void PlayerSteamingMilk()
    {
        steamSlider.value += steamValue * Time.smoothDeltaTime;
        Debug.Log("Slider value should be changing");
    }

    private void FinishSteaming()
    {
        Debug.Log("Player has finished steaming");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerInArea = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInArea = false;
    }

    

    private void MoveTargetArea()
    {
        float idleTimer = 5;
        float randomFloat = 0f;
        float moveSpeed = 1f;

        if(idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;
            Debug.Log(idleTimer);
        }
        else
        {
            randomFloat = Random.Range(0.3f, 0.7f);
            Debug.Log(randomFloat);
            targetSlider.value = randomFloat;

        }

        

    }
}
