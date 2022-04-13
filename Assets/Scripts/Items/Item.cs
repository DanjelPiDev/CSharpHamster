using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "HamsterGame/Items/Item", fileName = "new Item")]
public class Item : ScriptableObject, IComparable<Item>
{
    [Header("General Item Info")]
#if UNITY_EDITOR
    [Help("Jede Id muss einzigartig sein.\nItem anschließend im GameObject 'Manager' in der Komponente ItemHolder einfügen.\nListe wird automatisch zu spielbegin sortiert.", UnityEditor.MessageType.Info)]
#endif
    [SerializeField] private int id;
#if UNITY_EDITOR
    [Help("Dieser Name muss für die Tiles verwendet werden!", UnityEditor.MessageType.Warning)]
#endif
    [SerializeField] private new string name;
    [SerializeField] private Sprite itemImage;
    [SerializeField, TextArea(5, 5)] private string description;
    [SerializeField] private ItemType type;
    [ConditionalHide("type", true)] public bool canChangeHamsterValues = false;
    [SerializeField, Tooltip("Ist nur relevant falls Type == Consumable"), ConditionalHide("canChangeHamsterValues", true)] private int healValue;
    [SerializeField, Tooltip("Ist nur relevant falls Type == Consumable"), ConditionalHide("canChangeHamsterValues", true)] private int damageValue;
    [SerializeField, Tooltip("Ist nur relevant falls Type == Consumable"), ConditionalHide("canChangeHamsterValues", true)] private int enduranceHealValue;
    [SerializeField, Tooltip("Ist nur relevant falls Type == Consumable"), ConditionalHide("canChangeHamsterValues", true)] private int enduranceDamageValue;
    public bool isEquipment = false;
    [SerializeField, ConditionalHide("isEquipment", true)] private EquipmentType equipType;
    [SerializeField] private int buyPrice;
    [SerializeField] private int sellPrice;
    [SerializeField, Tooltip("Wieviele Items können auf einem Stack gestapelt werden.")] private int stackAmount;
    private bool isEquipped = false;
    [SerializeField] private int slotId;
    public bool hasSpecialEffects = false;
    [Header("Special Effects")]
    [SerializeField, ConditionalHide("hasSpecialEffects", true)] private int moveSpeed = 1;
    [Header("Item Events")]
    public UnityEvent onEquip;
    public UnityEvent onUnequip;
    public UnityEvent onUse;

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
    }

    public int SlotId
    {
        get { return slotId; }
        set { slotId = value; }
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
}