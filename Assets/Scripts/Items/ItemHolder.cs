using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour, IPointerClickHandler
{
    [Header("Item")]
    public Item item;

    [Header("Components")]
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemQuantity;
    public TextMeshProUGUI itemBuyPrice;
    public TextMeshProUGUI itemSellPrice;
    public Image itemImage;

    public int quantity = 1;

    public GameObject itemInfoPrefab;

    private Color unequipColor = new Color(0.2618814f, 0.5188679f, 0.388993f);
    private Color equipColor = new Color(0.5176471f, 0.2934902f, 0.2627451f);
    private GameObject itemInfoGo;

    #region GETTER
    public Color UnequipColor
    {
        get { return unequipColor; }
    }

    public Color EquipColor
    {
        get { return equipColor; }
    }
    #endregion

    private void Awake()
    {
        if (item == null) return;

        this.itemName.SetText(item.Name);
        this.itemQuantity.SetText(this.quantity + "/" + item.StackAmount);
        this.itemBuyPrice.SetText(item.BuyPrice.ToString());
        this.itemSellPrice.SetText(item.SellPrice.ToString());
        this.itemImage.sprite = this.item.ItemImage;
    }

    private void UseItem()
    {
        HamsterGameManager hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();
        this.item.OnUse();
        
        /* Finde den Hamster der im inventar ist oder das Item verwendet */
        Hamster hamster = null;
        foreach (Hamster ham in Territory.activHamsters)
        {
            if (ham.IsUsingItem)
            {
                hamster = ham;
                break;
            }
        }

        hamster.RemoveItem(this.item);
        hamsterGameManager.RefreshInventoryWindow();
    }

    private void EquipItem(Item item)
    {
        Transform equipment = this.transform.parent.parent.parent.parent.GetChild(5);

        for (int i = 1; i < equipment.childCount; i++)
        {
            // First check if the component exists.
            if (equipment.GetChild(i).GetChild(1).GetComponent<EquipmentSlot>())
            {
                // If the component does exists, check what type it is.
                if (equipment.GetChild(i).GetChild(1).GetComponent<EquipmentSlot>().equipType == item.EquipType)
                {
                    if (equipment.GetChild(i).GetChild(1).GetComponent<EquipmentSlot>().item != null)
                    {
                        UnequipItem();
                    }

                    equipment.GetChild(i).GetChild(1).GetComponent<EquipmentSlot>().item = item;
                    item.OnEquip();
                    return;
                }
            }
        }
    }

    private void UnequipItem()
    {
        HamsterGameManager hamsterGameManager = hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();
        Transform equipment = this.transform.parent.parent.parent.parent.GetChild(5);

        for (int i = 1; i < equipment.childCount; i++)
        {
            // Erst überprüfen ob die Komponente existiert.
            if (equipment.GetChild(i).GetChild(1).GetComponent<EquipmentSlot>())
            {
                // Falls die Komponente überprüft, prüfe ob der EquipmentType übereinstimmt.
                if (equipment.GetChild(i).GetChild(1).GetComponent<EquipmentSlot>().equipType == item.EquipType &&
                    equipment.GetChild(i).GetChild(1).GetComponent<EquipmentSlot>().item == this.item)
                {
                    equipment.GetChild(i).GetChild(1).GetComponent<EquipmentSlot>().item.OnUnequip();
                    equipment.GetChild(i).GetChild(1).GetComponent<EquipmentSlot>().item = null;

                    return;
                }
            }
        }

        for (int j = 0; j < hamsterGameManager.itemContent.childCount; j++)
        {
            if (hamsterGameManager.itemContent.GetChild(j).GetComponent<ItemHolder>().item.IsEquipped)
            {
                hamsterGameManager.itemContent.GetChild(j).gameObject.GetComponent<Image>().color = unequipColor;
                hamsterGameManager.itemContent.GetChild(j).GetComponent<ItemHolder>().item.OnUnequip();
                hamsterGameManager.itemContent.GetChild(j).GetComponent<ItemHolder>().item.IsEquipped = false;
            }
        }

        
    }

    public void SellItem()
    {
        HamsterGameManager hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();

        /* Check if the other hamster got enough grains */
        if (HamsterGameManager.hamster2.GrainCount >= this.item.SellPrice)
        {
            //this.quantity -= 1;
            HamsterGameManager.hamster2.GrainCount -= this.item.SellPrice;
            HamsterGameManager.hamster1.GrainCount += this.item.SellPrice;

            // Füge dem anderen Hamster das item ins Inventar ein
            HamsterGameManager.hamster2.AddItem(this.item);
            HamsterGameManager.hamster1.RemoveItem(this.item);

            //eventData.pointerEnter.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(this.quantity.ToString() + "/" + this.item.StackAmount.ToString());
            hamsterGameManager.tradeHamster1Grains.SetText(HamsterGameManager.hamster1.GrainCount.ToString());
            hamsterGameManager.tradeHamster2Grains.SetText(HamsterGameManager.hamster2.GrainCount.ToString());

            hamsterGameManager.RefreshTradeWindow();

            if (this.quantity == 0)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            Debug.Log(HamsterGameManager.hamster2.Name + ": Ich besitze nicht genug Körner.");
        }
    }

    public void BuyItem()
    {
        HamsterGameManager hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();

        /* Check if the other hamster got enough grains */
        if (HamsterGameManager.hamster1.GrainCount >= this.item.BuyPrice)
        {
            //this.quantity -= 1;
            HamsterGameManager.hamster2.GrainCount += this.item.BuyPrice;
            HamsterGameManager.hamster1.GrainCount -= this.item.BuyPrice;

            HamsterGameManager.hamster1.AddItem(this.item);
            HamsterGameManager.hamster2.RemoveItem(this.item);

            //eventData.pointerEnter.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(this.quantity.ToString() + "/" + this.item.StackAmount.ToString());
            hamsterGameManager.tradeHamster1Grains.SetText(HamsterGameManager.hamster1.GrainCount.ToString());
            hamsterGameManager.tradeHamster2Grains.SetText(HamsterGameManager.hamster2.GrainCount.ToString());

            hamsterGameManager.RefreshTradeWindow();

            if (this.quantity == 0)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            Debug.Log(HamsterGameManager.hamster1.Name + ": Ich besitze nicht genug Körner.");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && 
            !HamsterGameManager.isTrading)
        {
            if (!this.item.IsEquipped && this.item.Type == Item.ItemType.Equippable)
            {
                this.gameObject.GetComponent<Image>().color = equipColor;

                EquipItem(this.item);
                this.item.IsEquipped = true;
            }
            else if (this.item.Type == Item.ItemType.Consumable && (!HamsterGameManager.isTrading || !HamsterGameManager.isTalking))
            {
                foreach(Hamster hamster in Territory.activHamsters)
                {
                    if (hamster.IsInInventory)
                    {
                        hamster.IsUsingItem = true;
                        Territory.GetInstance().UpdateHamsterProperties(hamster);
                        break;
                    }
                }
                UseItem();
            }
            else if (this.item.IsEquipped)
            {
                this.gameObject.GetComponent<Image>().color = unequipColor;

                UnequipItem();
                this.item.IsEquipped = false;
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right && 
            HamsterGameManager.isTrading)
        {
            HamsterGameManager hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();

            
            if (eventData.pointerEnter.transform.parent.parent.parent.parent.name.Contains("1"))
            {
                /* Check if the other hamster got enough grains */
                if (HamsterGameManager.hamster2.GrainCount >= this.item.SellPrice)
                {
                    //this.quantity -= 1;
                    HamsterGameManager.hamster2.GrainCount -= this.item.SellPrice;
                    HamsterGameManager.hamster1.GrainCount += this.item.SellPrice;
                    
                    // Füge dem anderen Hamster das item ins Inventar ein
                    HamsterGameManager.hamster2.AddItem(this.item);
                    HamsterGameManager.hamster1.RemoveItem(this.item);

                    //eventData.pointerEnter.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(this.quantity.ToString() + "/" + this.item.StackAmount.ToString());
                    hamsterGameManager.tradeHamster1Grains.SetText(HamsterGameManager.hamster1.GrainCount.ToString());
                    hamsterGameManager.tradeHamster2Grains.SetText(HamsterGameManager.hamster2.GrainCount.ToString());

                    hamsterGameManager.RefreshTradeWindow();

                    if (this.quantity == 0)
                    {
                        Destroy(this.gameObject);
                    }
                }
                else
                {
                    Debug.Log(HamsterGameManager.hamster2.Name + ": Ich besitze nicht genug Körner.");
                }
            }
            else
            {
                /* Check if the other hamster got enough grains */
                if (HamsterGameManager.hamster1.GrainCount >= this.item.BuyPrice)
                {
                    //this.quantity -= 1;
                    HamsterGameManager.hamster2.GrainCount += this.item.BuyPrice;
                    HamsterGameManager.hamster1.GrainCount -= this.item.BuyPrice;     

                    HamsterGameManager.hamster1.AddItem(this.item);
                    HamsterGameManager.hamster2.RemoveItem(this.item);

                    //eventData.pointerEnter.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(this.quantity.ToString() + "/" + this.item.StackAmount.ToString());
                    hamsterGameManager.tradeHamster1Grains.SetText(HamsterGameManager.hamster1.GrainCount.ToString());
                    hamsterGameManager.tradeHamster2Grains.SetText(HamsterGameManager.hamster2.GrainCount.ToString());

                    hamsterGameManager.RefreshTradeWindow();

                    if (this.quantity == 0)
                    {
                        Destroy(this.gameObject);
                    }
                }
                else
                {
                    Debug.Log(HamsterGameManager.hamster1.Name + ": Ich besitze nicht genug Körner.");
                }
            }

            

        }
    }

    public void DisplayItemInfo(bool show)
    {
        base.StartCoroutine(WaitForDisplay(1.5f, show));
    }

    private IEnumerator WaitForDisplay(float time, bool b)
    {
        if (b && this.itemInfoGo == null)
        {
            yield return new WaitForSeconds(time);
            itemInfoGo = Instantiate(itemInfoPrefab, this.transform);
            itemInfoGo.SetActive(true);
            itemInfoGo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(this.item.Description);
            itemInfoGo.transform.position = Input.mousePosition;
        }
        else if (!b)
        {
            Destroy(itemInfoGo);
        }
    }
}