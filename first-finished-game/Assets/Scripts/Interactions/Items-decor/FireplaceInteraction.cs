using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FireplaceInteraction : MonoBehaviour
{
    private Inventory inventory;
    private bool playerInArea;
    private PlayerController playerCont;

    public GameObject canvas;
    public TextMeshProUGUI textBox;
    public GameObject exitButton;
    public GameObject useMatchButton;
    public InteractionText interactionText;
    public float textSpeed = 0.03f;
    public AudioSource typingAudio;
    public GameObject fireAnimation;
    public bool fireOn;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        playerCont = FindObjectOfType<PlayerController>();
        canvas.SetActive(false);
        useMatchButton.SetActive(false);
        fireOn = false;
        fireAnimation.SetActive(false);
    }

    private void Update()
    {
        if (!fireOn)
        {
            if (playerInArea && playerCont.isInteracting && !canvas.activeInHierarchy)
            {
                canvas.SetActive(true);
                SetPlayerBools();

                if (inventory.ContainsItem("Matches"))
                {
                    Debug.Log("matches in inv");
                    StartCoroutine(CanStartFire());
                }
                else
                {
                    StartCoroutine(CannotStartFire());
                    return;
                }

            }

        }
        else
        {
            //Debug.Log("Fire is on");
            return;
        }

        
        
    }

    private IEnumerator CannotStartFire()
    {
        Debug.Log("No matches in inv");
        //exitButton.SetActive(false);
        useMatchButton.SetActive(false);
        yield return null;

        yield return StartCoroutine(TypeText(interactionText.textToDisplay[0]));
        //exitButton.SetActive(true);
    }

    private IEnumerator CanStartFire()
    {
        //exitButton.SetActive(false);
        yield return StartCoroutine(TypeText(interactionText.textToDisplay[1]));
        useMatchButton.SetActive(true);
        //exitButton.SetActive(true);
    }

    public void MatchButton()
    {
        StartCoroutine(OnClickMatchButton());
    }

    private IEnumerator OnClickMatchButton()
    {
        canvas.SetActive(false);
        fireAnimation.SetActive(true);
        fireOn = true;
        useMatchButton.SetActive(false);
        yield return new WaitForSeconds(2f);
        canvas.SetActive(true);
        yield return StartCoroutine(TypeText(interactionText.textToDisplay[2]));
        
    }

    private IEnumerator TypeText(string text)
    {
        textBox.text = "";
        typingAudio.Play();
        exitButton.SetActive(false);
        yield return null;

        for (int i = 0; i < text.Length; i++)
        {
            textBox.text += text[i];
            yield return new WaitForSeconds(textSpeed);
        }
        exitButton.SetActive(true);
        typingAudio.Stop();
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerInArea = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInArea = false;
    }

    public void ExitButton()
    {
        ResetPlayerBools();
        canvas.SetActive(false);
    }
    private void SetPlayerBools()
    {
        playerCont.playerCanMove = false;
        playerCont.playerCanInteract = false;
        playerCont.playerInUI = true;
    }
    private void ResetPlayerBools()
    {
        playerCont.playerCanMove = true;
        playerCont.playerCanInteract = true;
        playerCont.playerInUI = false;
    }
}
