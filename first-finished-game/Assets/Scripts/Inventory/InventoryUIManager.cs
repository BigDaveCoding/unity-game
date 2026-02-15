using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventorySlotPrefab;
    public Transform inventoryPanel;

    public GameObject extraInfoCanvas;
    public Image extraInfoSprite;
    public TextMeshProUGUI extraInfoItemName;
    public TextMeshProUGUI extraInfoItemDesc;

    private Inventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        extraInfoCanvas.SetActive(false);
        
    }

    public void UpdateInventory()
    {
        //clear slots in inventory
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in inventory.items)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);
            TextMeshProUGUI slotText = slot.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>();

            slotText.text = item.name;

            Button slotButton = slot.GetComponent<Button>();
            slotButton.onClick.AddListener(() => ShowExtraInformation(item));
        }
        
    }

    public void ShowExtraInformation(Item item)
    {
        extraInfoSprite.sprite = item.itemIcon;
        extraInfoItemName.text = item.itemName;
        extraInfoItemDesc.text = item.itemDescription;

        extraInfoCanvas.SetActive(true);
        
        
    }

}
