using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayer : MonoBehaviour
{
    private GameObject player;
    private Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        AdjustSortingLayer();
    }

    void AdjustSortingLayer()
    {
        foreach (Transform child in gameObject.transform)
        {
            Transform childPos = child.GetComponent<Transform>();
            //Debug.Log(childPos);
            SpriteRenderer childSR = child.GetComponent<SpriteRenderer>();
            //Debug.Log(childSR);

            if(childPos.position.y > playerPos.position.y)
            {
                childSR.sortingOrder = -1;
                //Debug.Log(childSR.sortingOrder);
            }
            else
            {
                childSR.sortingOrder = 1;
                //Debug.Log(childSR.sortingOrder);
            }

        }
    }

    
}
