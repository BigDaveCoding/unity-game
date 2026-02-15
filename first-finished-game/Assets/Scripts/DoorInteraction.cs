using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{

    [SerializeField] private Transform mainCameraPos;
    [SerializeField] private Transform playerPos;
    public Vector3 playerMoveToPos;
    public Vector3 cameraMoveToPos;
    [SerializeField] private PlayerController playerCont;
    public GameObject transitionCanvas;
    public Animator transitionAnimator;

    private bool playerInArea;

    // Start is called before the first frame update
    void Start()
    {
        playerInArea = false;
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        mainCameraPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        playerCont = FindObjectOfType<PlayerController>();
        transitionCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInArea && playerCont.isInteracting && !playerCont.playerInTransition)
        {
            StartCoroutine(EnteredTransition());
        }
    }

    public IEnumerator EnteredTransition()
    {
        playerCont.playerCanMove = false;
        playerCont.playerInTransition = true;
        transitionCanvas.SetActive(true);
        transitionAnimator.SetBool("FadeIn", true);
        yield return new WaitForSeconds(transitionAnimator.GetCurrentAnimatorStateInfo(0).length);

        mainCameraPos.position = cameraMoveToPos;
        playerPos.position = playerMoveToPos;

        yield return new WaitForSeconds(0.5f);

        transitionAnimator.SetBool("FadeIn", false);
        transitionAnimator.SetBool("FadeOut", true);
        yield return new WaitForSeconds(transitionAnimator.GetCurrentAnimatorStateInfo(0).length);

        transitionAnimator.SetBool("FadeOut", false);
        playerCont.playerInTransition = false;
        playerCont.playerCanMove = true;
        transitionCanvas.SetActive(false);
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
