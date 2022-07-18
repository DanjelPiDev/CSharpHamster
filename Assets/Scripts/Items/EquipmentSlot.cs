/************************************************
 * Created by:  Danjel Galic
 * 
 * Modified by: -
 ************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Item")]
    public Item item;
    public Item.EquipmentType equipType;

    [Header("UI")]
    public Image itemImage;

    private Item selectedItem = null;
    private Color empty = new Color(0.57f, 0.49f, 0.39f, 0.74f);

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameObject go = eventData.pointerCurrentRaycast.gameObject;
            selectedItem = go.GetComponent<EquipmentSlot>().item;
            UnequipItem(selectedItem);
        }
    }

    private void UnequipItem(Item item)
    {
        if (item == null) return;

        item.OnUnequip();
        this.GetComponent<Image>().color = empty;
        this.item = null;
        this.itemImage.sprite = null;

        HamsterGameManager hamsterGameManager = hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();

        for (int i = 0; i < hamsterGameManager.itemContent.childCount; i++)
        {
            if (hamsterGameManager.itemContent.GetChild(i).GetComponent<ItemHolder>().item.IsEquipped && hamsterGameManager.itemContent.GetChild(i).GetComponent<ItemHolder>().item == selectedItem)
            {
                switch (hamsterGameManager.itemContent.GetChild(i).GetComponent<ItemHolder>().item.ItemRarity)
                {
                    case Item.Rarity.Normal: 
                        hamsterGameManager.itemContent.GetChild(i).gameObject.GetComponent<Image>().color = hamsterGameManager.itemContent.GetChild(i).GetComponent<ItemHolder>().item.Normal; break;
                    case Item.Rarity.Rare: 
                        hamsterGameManager.itemContent.GetChild(i).gameObject.GetComponent<Image>().color = hamsterGameManager.itemContent.GetChild(i).GetComponent<ItemHolder>().item.Rare; break;
                    case Item.Rarity.Epic: 
                        hamsterGameManager.itemContent.GetChild(i).gameObject.GetComponent<Image>().color = hamsterGameManager.itemContent.GetChild(i).GetComponent<ItemHolder>().item.Epic; break;
                    case Item.Rarity.Legendary: 
                        hamsterGameManager.itemContent.GetChild(i).gameObject.GetComponent<Image>().color = hamsterGameManager.itemContent.GetChild(i).GetComponent<ItemHolder>().item.Legendary; break;
                    case Item.Rarity.Unique: 
                        hamsterGameManager.itemContent.GetChild(i).gameObject.GetComponent<Image>().color = hamsterGameManager.itemContent.GetChild(i).GetComponent<ItemHolder>().item.Unique; break;
                    default: break;
                }
                hamsterGameManager.itemContent.GetChild(i).GetComponent<ItemHolder>().item.IsEquipped = false;
            }
        }
    }

    private void Update()
    {
        if (item == null)
        {
            itemImage.color = new Color(0, 0, 0, 0);
            return;
        }

        if (itemImage != null)
        {
            itemImage.color = new Color(1, 1, 1, 1);
            itemImage.sprite = item.ItemImage;
            switch (item.ItemRarity)
            {
                case Item.Rarity.Normal:
                    this.GetComponent<Image>().color = item.Normal; break;
                case Item.Rarity.Rare:
                    this.GetComponent<Image>().color = item.Rare; break;
                case Item.Rarity.Epic:
                    this.GetComponent<Image>().color = item.Epic; break;
                case Item.Rarity.Legendary:
                    this.GetComponent<Image>().color = item.Legendary; break;
                case Item.Rarity.Unique:
                    this.GetComponent<Image>().color = item.Unique; break;
            }
        }
    }
}
