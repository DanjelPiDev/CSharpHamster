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
        items.Sort();
    }

    public Item GetItem(int id)
    {
        if (id < 0 || id > items.Count) return null;
        return items[id];
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
