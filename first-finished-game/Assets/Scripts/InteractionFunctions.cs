using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractionFunctions : MonoBehaviour
{
    public GameObject canvas;
    public TextMeshProUGUI textBox;
    public float textSpeed = 0.05f;

    public BoxCollider2D[] boxColliders;

    private PlayerController playerCont;

    private void Start()
    {
        playerCont = FindObjectOfType<PlayerController>();
    }

    public IEnumerator TypeText(string text)
    {
        
        for (int i = 0; i < text.Length; i++)
        {
            
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void ExitButton()
    {
        playerCont.playerCanMove = true;
        playerCont.playerInUI = false;
        playerCont.playerCanInteract = true;
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
