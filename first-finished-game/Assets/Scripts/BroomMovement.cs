using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomMovement : MonoBehaviour
{
    ColourRoomCutscene colourRoomCutscene;
    private Vector3 broomMoveLocation;
    public float moveSpeed;
    private Animator broomAnimator;

    // Start is called before the first frame update
    void Start()
    {
        broomAnimator = gameObject.GetComponent<Animator>();
        colourRoomCutscene = FindObjectOfType<ColourRoomCutscene>();
        broomMoveLocation = gameObject.transform.position + new Vector3(5f, 0f, 0f);
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (colourRoomCutscene.broomMove)
        {
            MoveBroom();
        }
        else
        {
            //Debug.Log($"broomMovement bool is: {colourRoomCutscene.broomMove} ");
            return;
        }
        
    }

    private void MoveBroom()
    {
        broomAnimator.SetBool("broomActive", true);
        //int randomNum = Random.Range(0, 2);
        Vector3 currentPos = gameObject.transform.position;
        Vector3 targetPos = broomMoveLocation;

        gameObject.transform.position = Vector3.MoveTowards(currentPos, targetPos, moveSpeed * Time.fixedDeltaTime);

        if (Vector3.Distance(currentPos, targetPos) < 0.1f)
        {
            gameObject.transform.position = targetPos;
            broomAnimator.SetBool("broomActive", false);
            colourRoomCutscene.broomMove = false;
        }

    }
}



//when broomMove bool is active the broom will move

//move from current pos to a target pos
//when the gameObject reaches target pos it moves back to original position
//when it is back at the original position is stops moving. Animation stops and resets broomMove bool in colour room script
