using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds all available items available in the game
/// </summary>
public class ItemCollection : MonoBehaviour
{
    public List<Item> items = new List<Item>();


    private void Awake()
    {
        /*
         * Sort the list, afterwards set the id for each item again
         */
        items.Sort();
        AssignIDs();
    }

    private void AssignIDs()
    {
        int id = 0;

        foreach (Item item in items)
        {
            item.Id = id;
            id += 1;
        }
    }

    public Item GetItem(int id)
    {
        Item item = items[id];
        if (item != null) return item;
        else return null;
    }

    public Item GetItem(string name)
    {
        foreach (Item item in items)
        {
            if (item.Name == name)
            {
                return item;
            }
        }
        return null;
    }
}
