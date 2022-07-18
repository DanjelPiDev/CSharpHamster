/************************************************
 * Created by:  Danjel Galic
 * 
 * Modified by: -
 ************************************************/
using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "HamsterGame/Items/Item", fileName = "new Item")]
public class Item : ScriptableObject, IComparable<Item>
{
    [Header("General Item Info")]
#if UNITY_EDITOR
    [Help("Every id needs to be unique for each item.\nAdd item to the GameObject 'Manager' into the 'Item Collection' component.\nList sorts automatically.", UnityEditor.MessageType.Info)]
#endif
    [SerializeField] private int id;
#if UNITY_EDITOR
    [Help("Use this name creating a tile for this item.", UnityEditor.MessageType.Warning)]
#endif
    [SerializeField] private new string name;
    [SerializeField] private Sprite itemImage;
    [SerializeField, TextArea(5, 5)] private string description;
    [SerializeField] private ItemType type;
    [SerializeField] private Rarity rarity;
    [ConditionalHide("type", true)] public bool canChangeHamsterValues = false;
    [SerializeField, Tooltip("Relevant if Type == Consumable"), ConditionalHide("canChangeHamsterValues", true)] private int healValue;
    [SerializeField, Tooltip("Relevant if Type == Consumable"), ConditionalHide("canChangeHamsterValues", true)] private int damageValue;
    [SerializeField, Tooltip("Relevant if Type == Consumable"), ConditionalHide("canChangeHamsterValues", true)] private int enduranceHealValue;
    [SerializeField, Tooltip("Relevant if Type == Consumable"), ConditionalHide("canChangeHamsterValues", true)] private int enduranceDamageValue;
    public bool isEquipment = false;
    [SerializeField, ConditionalHide("isEquipment", true)] private EquipmentType equipType;
    [SerializeField] private int buyPrice;
    [SerializeField] private int sellPrice;
    [SerializeField, Tooltip("How many items can you stack in one slot?")] private int stackAmount;
    private bool isEquipped = false;
    [SerializeField] private int slotId;
    public bool hasSpecialEffects = false;
    [Header("Special Effects")]
    [SerializeField, ConditionalHide("hasSpecialEffects", true)] private int moveSpeed = 0;
    [SerializeField, ConditionalHide("hasSpecialEffects", true)] private int attackPower = 0;
    [Header("Item Events")]
    public UnityEvent onEquip;
    public UnityEvent onUnequip;
    public UnityEvent onUse;

    private Color normal = new Color(0.69f, 0.85f, 0.60f);
    private Color rare = new Color(0.19f, 0.29f, 0.92f);
    private Color epic = new Color(0.47f, 0.19f, 0.92f);
    private Color legendary = new Color(0.92f, 0.55f, 0.19f);
    private Color unique = new Color(0.92f, 0.88f, 0.19f);



    public void OnEquip()
    {
        onEquip?.Invoke();
    }

    public void OnUnequip()
    {
        onUnequip?.Invoke();
    }

    public void OnUse()
    {
        onUse?.Invoke();
    }

    public int CompareTo(Item other)
    {
        if (other.Id < this.id)
            return 1;
        else
            return -1;
    }


    #region GETTER_SETTER

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public Rarity ItemRarity
    {
        get { return rarity; }
    }

    public int SlotId
    {
        get { return slotId; }
        set { slotId = value; }
    }

    public Color Normal
    {
        get { return normal; }
    }

    public Color Rare
    {
        get { return rare; }
    }

    public Color Epic
    {
        get { return epic; }
    }

    public Color Legendary
    {
        get { return legendary; }
    }

    public Color Unique
    {
        get { return unique; }
    }

    public string Name
    {
        get { return name; }
    }

    public Sprite ItemImage
    {
        get { return itemImage; }
    }

    public string Description
    {
        get { return description; }
    }

    public ItemType Type
    {
        get { return type; }
    }

    public int HealValue
    {
        get { return healValue; }
        set { healValue = value; }
    }

    public int DamageValue
    {
        get { return damageValue; }
        set { damageValue = value; }
    }

    public int EnduranceHealValue
    {
        get { return enduranceHealValue; }
        set { enduranceHealValue = value; }
    }

    public int EnduranceDamageValue
    {
        get { return enduranceDamageValue; }
        set { enduranceDamageValue = value; }
    }

    public EquipmentType EquipType
    {
        get { return equipType; }
    }

    public bool IsEquipped
    {
        get { return isEquipped; }
        set { isEquipped = value; }
    }

    public int BuyPrice
    {
        get { return buyPrice; }
    }

    public int SellPrice
    {
        get { return sellPrice; }
    }

    public int StackAmount
    {
        get { return stackAmount; }
    }

    public int MoveSpeed
    {
        get { return moveSpeed; }
    }

    public int AttackPower
    {
        get { return attackPower; }
    }

    #endregion

    public enum ItemType
    {
        Equippable,
        Consumable
    };

    public enum EquipmentType
    {
        None,
        Head,
        Hands,
        Foot,
        Extra1,
        Extra2
    };

    public enum Rarity
    {
        Normal,
        Rare,
        Epic,
        Legendary,
        Unique
    };
}