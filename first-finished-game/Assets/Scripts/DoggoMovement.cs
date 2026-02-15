using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoggoMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;

    public BoxCollider2D movementBoundary;
    private Vector2 posDestination;
    [SerializeField] private float posThreshold = 0.05f;

    [SerializeField] private float idleTime = 5f;
    public bool npcIdle;
    public bool npcMoving;

    public Animator animator;
    public bool npcCanMove;
    public bool playerInteracting;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GetRandomMovementPosition();
        npcIdle = true;
        npcCanMove = true;
        playerInteracting = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!playerInteracting)
        {
            if (npcIdle)
            {
                IdleTimer();
            }
            else if (!npcIdle && npcCanMove)
            {
                MoveNPC();
            }
        }
        else
        {
            PlayerInteracting();
        }
        
    }

    private void GetRandomMovementPosition()
    {
        //get random position within movementBoundary

        float randomX = Random.Range(movementBoundary.bounds.min.x, movementBoundary.bounds.max.x);
        float randomY = Random.Range(movementBoundary.bounds.min.y, movementBoundary.bounds.max.y);
        posDestination.x = randomX;
        posDestination.y = randomY;
    }

    private void MoveNPC()
    {
        
        npcMoving = true;
        animator.SetBool("isMoving", npcMoving);

        Vector2 moveDirection = posDestination - (Vector2)gameObject.transform.position;
        moveDirection.Normalize();

        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, posDestination, moveSpeed * Time.fixedDeltaTime);
        animator.SetFloat("x", moveDirection.x);
        //animator.SetFloat("y", moveDirection.y);
        if(Vector2.Distance(gameObject.transform.position, posDestination) < posThreshold)
        {
            gameObject.transform.position = posDestination;
            GetRandomMovementPosition();
            npcIdle = true;
            npcMoving = false;
            animator.SetBool("isMoving", false);
        }
    }

    private void IdleTimer()
    {
        if (npcIdle)
        {
            idleTime -= Time.fixedDeltaTime;
            if(idleTime <= 0f)
            {
                npcIdle = false;
                idleTime = Random.Range(3f, 8f);
            }
        }
    }

    private void PlayerInteracting()
    {
        animator.SetBool("isMoving", false);
        npcCanMove = false;
    }
}
