using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //using input system to control player movement

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 3f; //public variable for movespeed that can be changed in the inspector.
    private Rigidbody2D rb;
    private Vector2 moveDirection; //creating private vector2 to assign a value which will be obtained through the OnMove() function
    public bool playerCanMove; //bool to set if the player can control the movement of the character. public so it can be used across other scripts
    public bool playerMoving; //bool to set whether the player is idle. public so it can be used across other scripts
    public bool isInteracting;
    public bool playerCanInteract;
    public bool playerInUI; //need bool for when player has a ui open to set whether the player can move or not.
    [SerializeField] private GameObject inventoryCanvas;
    public bool playerInTransition;

    public Animator animator;
    private InventoryUIManager inventoryUIManager;
    private bool playerInInventory;

    private AudioSource audioSource;
    public AudioClip[] audioClips;


    
    void Start()
    {

        
        inventoryCanvas.SetActive(false);

        rb = GetComponent<Rigidbody2D>(); //assigning the private rigidbody2d to the rigidbody2d component this script is attached too
        playerCanMove = true; //assigning the value of true to the boolean so the player can move upon starting game.
        playerMoving = false; //assigning value of true to bool as when starting the game the boolean is set to false which would indicate movement. I want the player to start in a idle position.
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        playerCanInteract = true;

        inventoryUIManager = FindObjectOfType<InventoryUIManager>();
        //inventoryCanvas.SetActive(false);
        playerInInventory = false;

    }

    private void FixedUpdate()
    {
        if(playerCanMove)
        {
            Vector2 movement = moveDirection * moveSpeed * Time.fixedDeltaTime;
            rb.position += movement; //using the rb position of x and y and adding the vector2 movement x and y in order to make the player move on the screen
            
        }
        else
        {
            //Debug.Log("player cannot control movement");
            return;

        }

        //Debug.Log("player moving is " + playerMoving);
        //Debug.Log("PlayerCanInteract is " + playerCanInteract);

    }

    private void PlayFootstepSound()
    {
        audioSource.clip = audioClips[0];
        audioSource.pitch = 0.7f;
        audioSource.volume = 0.2f;

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            audioSource.loop = true;
        }
        
        

    }

    private void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>().normalized;

        if (playerCanMove)
        {
            if(moveDirection != Vector2.zero)
            {
                
                playerMoving = true; //player is moving so boolean is true
                animator.SetFloat("x", moveDirection.x);
                animator.SetFloat("y", moveDirection.y); //setting the x and y floats in the animator equal to the move direction x and y values
                animator.SetBool("isMoving", playerMoving); //setting the bool of animator to control transitions between idle and moving animation.
                PlayFootstepSound();
            }
            else
            {
                playerMoving = false;
                animator.SetBool("isMoving", playerMoving);
                audioSource.Stop();
            }
        }
        
    }

    private void OnInteract(InputValue value)
    {
        if (playerCanInteract)
        {
            //isInteracting = true;
            //Debug.Log("player is interacting");
            //playerCanMove = false;
            //Debug.Log("Player cannot move whilst interacting and bool is " + playerCanMove);
            //animator.SetBool("isMoving", false);

            //Invoke("EndInteraction", 0.3f);

            StartCoroutine(Interact());
        }
        
        
    }

    private IEnumerator Interact()
    {
        audioSource.clip = null;
        isInteracting = true;
        Debug.Log("Player is interacting");
        playerCanMove = false;
        animator.SetBool("isMoving", false);
        yield return null;

        isInteracting = false;
        Debug.Log("Player is not interacting");
        if (playerInUI)
        {
            playerCanMove = false;
            Debug.Log("Player in UI so playerCanMove Bool is " + playerCanMove);
        }
        else
        {
            playerCanMove = true;
            Debug.Log("Player can now move and bool is " + playerCanMove);
            animator.SetBool("isMoving", playerMoving);
        }
        yield return null;
    }

    //private void EndInteraction()
    //{
        
    //    isInteracting = false;
    //    Debug.Log("player no longer interacting");
    //    if(playerInUI)
    //    {
    //        playerCanMove = false;
    //        Debug.Log("Player in UI so playerCanMove Bool is " + playerCanMove);
    //    }
    //    else
    //    {
    //        playerCanMove = true;
    //        Debug.Log("Player can now move and bool is " + playerCanMove);
    //        animator.SetBool("isMoving", playerMoving);
    //    }
    //}

    private void OnOpenInventory()
    {
        if (!playerInInventory)
        {
            inventoryUIManager.UpdateInventory();
            inventoryCanvas.SetActive(true);
            playerInUI = true;
            playerCanMove = false;
            playerInInventory = true;
        }
        else if (playerInInventory)
        {
            inventoryCanvas.SetActive(false);
            playerInUI = false;
            playerCanMove = true;
            playerInInventory = false;
            inventoryUIManager.extraInfoCanvas.SetActive(false);
        }
    }

    public void SetInteractionBools()
    {
        playerCanInteract = false;
        playerCanMove = false;
        playerInUI = true;
    }
    public void resetInteractionBools()
    {
        playerCanInteract = true;
        playerCanMove = true;
        playerInUI = false;
    }
}
