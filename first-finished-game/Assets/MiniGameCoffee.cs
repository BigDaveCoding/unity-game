using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
using TMPro;
using UnityEngine.UI;

public class MiniGameCoffee : MonoBehaviour
{
    
    public bool playerEnteredCoffeeGame;
    public Camera mainCamera;
    public GameObject canvas;
    public TextMeshProUGUI minigameText;
    public TextMeshProUGUI[] currentTempText;
    public Image minigameImage;
    public InteractionText gameText;

    public GameObject portafilterTargetsquare;
    public GameObject machineGroupheadTargetSquare;
    public BoxCollider2D machineGroupheadBounds;
    public GameObject portafilter;
    public Vector3 portafilterFinalPos;

    public GameObject sproAnimation;
    public GameObject sproMachineButtonGO;
    public GameObject sproMachineButtonUI;

    public GameObject milkPitcherTargetSquare;
    public BoxCollider2D milkPitcherBounds;
    public Vector3 milkPitcherFinalPos;
    public GameObject milkPitcher;
    private BoxCollider2D milkPitcherCollider;

    public GameObject steamSlider;
    public BoxCollider2D steamSliderBounds;
    public GameObject steamTargetArea;
    public GameObject playerControlSteamSlider;
    private Vector2 randomPos;
    private bool hasRandomPos;
    public float targetAreaMoveSpeed = 1f;
    [SerializeField] private float playerBarMoveSpeed = 1f;

    public GameObject steamPlayerControlBar;
    public BoxCollider2D targetSteamBounds;
    private float initialYPos;
    public float steamTemperature = 0f;
    public float tempAdd = 10f;
    public float targetTemp = 65f;
    
    

    public GameObject[] changeColorTargets;

    public Vector3 finishedCameraPos;
    public Vector3 finishedPlayerPos;

    public DoorInteraction transition;
    public GameObject minigameScene;

    private PlayerController playerCont;


    [SerializeField] private bool portafilterInPlace;
    [SerializeField] private bool moveMilkPitcher;
    [SerializeField] private bool steamingMilk;
    [SerializeField] private bool playerSteaming;
    [SerializeField] private bool finishedMiniGame;
    //private bool drawLatteArt;

    [SerializeField] private Inventory inv;
    public Item removeBeans;
    public Item addCoffeeInv;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
        //playerEnteredCoffeeGame = true;
        GetSpriteRenderers();
        portafilterInPlace = false;
        sproAnimation.SetActive(false);
        sproMachineButtonUI.SetActive(false);
        milkPitcherCollider = milkPitcher.GetComponent<BoxCollider2D>();
        milkPitcherCollider.enabled = false;
        hasRandomPos = false;
        initialYPos = steamPlayerControlBar.transform.position.y;
        steamSlider.SetActive(false);
        minigameImage.preserveAspect = true;
        minigameText.text = gameText.textToDisplay[0];
        minigameImage.sprite = gameText.spritesToDisplay[0];
        canvas.SetActive(false);
        foreach (TextMeshProUGUI child in currentTempText)
        {
            child.text = "";
        }
        playerCont = FindObjectOfType<PlayerController>();

        //Debug.Log(initialYPos);
        inv = FindObjectOfType<Inventory>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerEnteredCoffeeGame)
        {
            canvas.SetActive(true);
            if(mainCamera.orthographicSize != 2)
            {
                mainCamera.orthographicSize = 2;
                Debug.Log("Camera Size set to 2");
            }
            minigameText.text = gameText.textToDisplay[0];
            minigameImage.sprite = gameText.spritesToDisplay[0];

            PlayerMovingPortafilter();
            Debug.Log("In First section of minigame");
        }
        
        if (portafilterInPlace)
        {
            portafilter.transform.position = portafilterFinalPos; //make sure portafilter is in place
            Debug.Log("portafilter is in place, onto step 2");
        }

        if (moveMilkPitcher)
        {
            minigameText.text = gameText.textToDisplay[1];
            minigameImage.sprite = gameText.spritesToDisplay[1];
            MoveMilkJug();
            Debug.Log("move milk pitcher");
        }

        if (steamingMilk)
        {
            steamSlider.SetActive(true);
            minigameText.text = gameText.textToDisplay[2];
            currentTempText[0].text = "Target Temp: " + (int)targetTemp;
            currentTempText[1].text = "Current Temp: " + (int)steamTemperature;


            milkPitcher.transform.position = milkPitcherFinalPos; //make sure milk jug is in place
            Debug.Log("steaming milk");
            Animator milkPAnim = milkPitcher.GetComponentInChildren<Animator>(); 
            //Debug.Log(milkPAnim);
            milkPAnim.SetTrigger("steam");//start milk steaming animation

            PlayerControlSteamBar();

            if (!hasRandomPos)
            {
                GetRandomPos();
                Debug.Log("reset random pos");
            }
            else
            {
                MoveTargetArea();
                Debug.Log("moving towards random pos");
            }

            if (IsWithinBounds(steamPlayerControlBar.transform.position, targetSteamBounds.bounds))
            {
                steamTemperature += tempAdd * Time.deltaTime;
                Debug.Log($" steam temp is {(int)steamTemperature}");
            }

            if(steamTemperature >= targetTemp)
            {
                minigameText.text = gameText.textToDisplay[3];
                minigameImage.sprite = gameText.spritesToDisplay[2];
                currentTempText[1].text = "Current Temp: " + (int)steamTemperature;
                steamTemperature = targetTemp;
                steamingMilk = false;
                milkPAnim.SetBool("stopSteam", true);
                steamSlider.SetActive(false);
                finishedMiniGame = true; 


            }

           

            if (finishedMiniGame)
            {
                StartCoroutine(FinishedMiniGame());
                playerCont.resetInteractionBools();
                
            }
        }
    }

    private void PlayerMovingPortafilter()
    {
        Vector2 portafilterTargetPos = portafilterTargetsquare.transform.position;
        //Debug.Log(portafilterTargetPos);
        Bounds groupheadColliderBounds = machineGroupheadBounds.bounds;
        //Debug.Log(groupheadColliderBounds);

        Debug.Log("Player moving portafilter");

        if(IsWithinBounds(portafilterTargetPos, groupheadColliderBounds))
        {
            portafilter.transform.position = portafilterFinalPos;
            DeactivateCollider(portafilter.GetComponent<Collider2D>());
            portafilterInPlace = true;
            sproMachineButtonUI.SetActive(true);
            Debug.Log($"UI button active is {sproMachineButtonUI.activeInHierarchy}");
            playerEnteredCoffeeGame = false;
        }
    }

    public void SproMachineButton()
    {
        sproAnimation.SetActive(true);
        SpriteRenderer buttonSpriteRen = sproMachineButtonGO.GetComponent<SpriteRenderer>();
        Color newColor = new Color(0f, 0.84f, 0.05f, 1f);
        buttonSpriteRen.color = newColor;
        sproMachineButtonUI.SetActive(false);
        Debug.Log("Playr pressed button, onto steaming milk");
        portafilterInPlace = false;
        moveMilkPitcher = true;
        
    }

    private void MoveMilkJug()
    {
        Vector2 mpTargetPos = milkPitcherTargetSquare.transform.position;
        Bounds bounds = milkPitcherBounds.bounds;
        milkPitcherCollider.enabled = true;

        if (IsWithinBounds(mpTargetPos, bounds))
        {
            milkPitcher.transform.position = milkPitcherFinalPos;
            DeactivateCollider(milkPitcherCollider);
            Debug.Log("Milk pitcher is in place");
            steamingMilk = true;
            moveMilkPitcher = false;

        }
    }

    private void GetRandomPos()
    {
        float minY = steamSliderBounds.bounds.min.y;
        float maxY = steamSliderBounds.bounds.max.y;

        //Debug.Log(minY);
        //Debug.Log(maxY);
        randomPos = new Vector2(steamTargetArea.transform.position.x, Random.Range(minY, maxY));
        Debug.Log(randomPos);
        hasRandomPos = true;
        targetAreaMoveSpeed = Random.Range(0.5f, 1f);
    }

    private void MoveTargetArea()
    {

        if((Vector2)steamTargetArea.transform.position != randomPos)
        {
            Vector2 newPos = Vector2.MoveTowards((Vector2)steamTargetArea.transform.position, randomPos, targetAreaMoveSpeed * Time.deltaTime);
            steamTargetArea.transform.position = newPos;

            if(Vector2.Distance((Vector2)steamTargetArea.transform.position, randomPos) < 0.00005f)
            {
                //Debug.Log($"target are transform pos is {steamTargetArea.transform.position}");
                steamTargetArea.transform.position = randomPos;
                hasRandomPos = false;
            }
        }
    }

    private void PlayerControlSteamBar()
    {
        
        Vector2 targetPos = steamPlayerControlBar.transform.position;
        
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerSteaming = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            playerSteaming = false;
        }

        if (playerSteaming)
        {
            targetPos.y += playerBarMoveSpeed * Time.deltaTime;
            steamPlayerControlBar.transform.position = targetPos;
            Debug.Log("Player steaming and bar moving up");
            //Debug.Log(targetPos);
        }
        else
        {
            if(targetPos.y > initialYPos)
            {
                targetPos.y -= playerBarMoveSpeed * Time.deltaTime;
                steamPlayerControlBar.transform.position = targetPos;
                Debug.Log("Player not steaming and bar moving down");
            }
            else
            {
                Debug.Log("player not steaming but bar cannot pass initail y pos");
                return;
            }   
        }
    }

    private IEnumerator FinishedMiniGame()
    {
        
        yield return new WaitForSeconds(5f);
        //minigameScene.SetActive(false);
        
        Debug.Log("Starting finished mini game co routine");
        transition.cameraMoveToPos = finishedCameraPos;
        transition.playerMoveToPos = finishedPlayerPos;
        StartCoroutine(transition.EnteredTransition());
        yield return new WaitForSeconds(1f);
        canvas.SetActive(false);
        Camera.main.orthographicSize = 5;

        if (inv.ContainsItem(removeBeans.itemName))
        {
            inv.RemoveItem(removeBeans);
        }
        // if inventory contains item then remove item from inventory

        inv.AddItem(addCoffeeInv);
        //add coffee to inventory
        


    }


    bool IsWithinBounds(Vector2 point, Bounds bounds)
    {
        bool result = bounds.Contains(point);
        Debug.Log($"Point {point} is within bounds: {result}");
        return result;
    }


    //bool IsWithinBounds(Vector2 point, Bounds bounds)
    //{

    //    return bounds.Contains(point);
    //}

    //bool IsWithinBounds(Vector2 point, Bounds bounds)
    //{
    //    Collider2D collider = Physics2D.OverlapPoint(point);
    //    return collider != null && collider.bounds == bounds;
    //}


    private void DeactivateCollider(Collider2D collider)
    {
        collider.enabled = false;
    }

    private void GetSpriteRenderers()
    {
        foreach (GameObject child in changeColorTargets)
        {
            SpriteRenderer spriteRen = child.GetComponent<SpriteRenderer>();
            Color spriteRenColor = spriteRen.color;
            spriteRenColor.a = 0;
            spriteRen.color = spriteRenColor;
        }
    }
}
