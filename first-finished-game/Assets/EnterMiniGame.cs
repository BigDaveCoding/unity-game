using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterMiniGame : MonoBehaviour
{

    private bool playerInArea;
    [SerializeField] private MiniGameCoffee miniGame;
    [SerializeField] private PlayerController playerCont;
    public Vector3 cameraMoveToPos;
    [SerializeField] private Transform CameraPos;
    private Inventory inv;

    [SerializeField] private bool hasPlayedMiniGame;

    private void Start()
    {
        miniGame = FindObjectOfType<MiniGameCoffee>();
        playerCont = FindObjectOfType<PlayerController>();
        CameraPos = Camera.main.transform;
        inv = FindObjectOfType<Inventory>();
        hasPlayedMiniGame = false;
    }

    private void Update()
    {
        if (!hasPlayedMiniGame)
        {
            if (playerInArea && playerCont.isInteracting && inv.ContainsItem("Coffee Beans"))
            {
                playerCont.SetInteractionBools();
                miniGame.playerEnteredCoffeeGame = true;
                miniGame.canvas.SetActive(true);
                Camera.main.orthographicSize = 2;
                Camera.main.transform.position = cameraMoveToPos;
                hasPlayedMiniGame = true;
            }
        }
        else if (playerInArea && hasPlayedMiniGame)
        {
            Debug.Log("Player has already played minigame");
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

}
