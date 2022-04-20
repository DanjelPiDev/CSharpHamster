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
        AssignIDs();
        items.Sort();
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
        foreach (Item item in items)
        {
            if (item.Id == id)
            {
                return item;
            }
        }
        return null;
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

    private void Update()
    {
    }
}
