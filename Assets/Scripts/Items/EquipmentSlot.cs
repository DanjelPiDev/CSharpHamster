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

        this.item = null;
        this.itemImage.sprite = null;

        HamsterGameManager hamsterGameManager = hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();

        for (int i = 0; i < hamsterGameManager.itemContent.childCount; i++)
        {
            if (hamsterGameManager.itemContent.GetChild(i).GetComponent<ItemHolder>().item.IsEquipped && hamsterGameManager.itemContent.GetChild(i).GetComponent<ItemHolder>().item == selectedItem)
            {
                hamsterGameManager.itemContent.GetChild(i).gameObject.GetComponent<Image>().color = hamsterGameManager.itemContent.GetChild(i).GetComponent<ItemHolder>().UnequipColor;
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
        }
    }
}
