using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public void AddItem(Item item)
    {
        items.Add(item);

    }
    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }

    public bool ContainsItem(string itemName)
    {
        foreach (Item item in items)
        {
            if(item.itemName == itemName)
            {
                return true; //Found Item in inventory
            }
        }
        return false;
    }

    public string GetItemName(Item item)
    {
        foreach (Item items in items)
        {
            return items.itemName;
        }
        return null;
    }
}
