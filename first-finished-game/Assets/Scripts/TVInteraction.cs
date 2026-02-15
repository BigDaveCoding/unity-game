using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVInteraction : MonoBehaviour
{

    private bool playerInArea;
    PlayerController playerCont;
    public GameObject tvAnimation;
    private bool tvOn;
    // Start is called before the first frame update
    void Start()
    {
        playerCont = FindObjectOfType<PlayerController>();
        tvOn = false;
        tvAnimation.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInArea && Input.GetKeyDown(KeyCode.Space))
        {
            if (!tvOn)
            {
                tvAnimation.SetActive(true);
                tvOn = true;
            }
            else
            {
                tvAnimation.SetActive(false);
                tvOn = false;
            }
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
