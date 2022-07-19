/************************************************
 * Created by:  Danjel Galic
 * 
 * Modified by: -
 ************************************************/
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;


#if UNITY_EDITOR
using UnityEditor;
#endif

/************************************************
 * -- ItemSlot --
 * 
 * 
 ************************************************/
[System.Serializable]
public class ItemSlot : IComparable<ItemSlot>
{
    public int slotId = int.MinValue + 1;
    public Item item;
    public int quantity;

    public int CompareTo(ItemSlot other)
    {
        if (other.item.Name[0] < this.item.Name[0])
            return 1;
        else
            return -1;
    }

}

/************************************************
 * -- Hamster --
 * 
 * This class controls all the properties and
 * actions for the Hamster.
 * 
 * Hint: This is a ScriptableObject
 ************************************************/
[CreateAssetMenu(menuName = "HamsterGame/Hamster", fileName = "new Hamster")]
public class Hamster : ScriptableObject
{
    [Header("General Hamster Info")]
#if UNITY_EDITOR
    [Help("Enable only for testing!", UnityEditor.MessageType.Info)]
#endif
    [SerializeField] private bool debug = false;
    [SerializeField, ConditionalHide("debug")] private int id;
    [SerializeField] private string hamsterName;
    [SerializeField, ConditionalHide("debug")] private int row;
    [SerializeField, ConditionalHide("debug")] private int column;
    [SerializeField] private LookingDirection direction;
    [SerializeField] private int grainCount;
    [SerializeField, ConditionalHide("debug")] private bool playerControl;
    [SerializeField] private bool respawn = true;
    [SerializeField] private bool isNPC;
    [SerializeField] private bool moveRandom;
    [SerializeField] private MovementPattern movementPattern;
    [SerializeField, ConditionalHide("debug")] private SpriteRenderer spriteRenderer;
    [SerializeField, ConditionalHide("debug")] private GameObject hamsterObject;
    [SerializeField] private HamsterColor hamsterColor;
    [SerializeField] private bool canMove = false;
    [SerializeField] private bool canTrade = true;
    [SerializeField] private bool canTalk = true;
    [SerializeField] private List<ItemSlot> inventory = new List<ItemSlot>();
    [SerializeField, ConditionalHide("debug")] private bool godMode = true; // Off, because of reasons
    [SerializeField] private int healthPoints;
    [SerializeField] private int endurancePoints;
    [SerializeField, Min(0)] private int healthPointsFull;
    [SerializeField, Min(0)] private int endurancePointsFull;
    private Vector2 startPoint;
    [Header("Location and Movement Info")]
    [SerializeField, Range(1,5)] private int moveSpeed = 1;
    [SerializeField] private Transform movePoint;
    [SerializeField] private float currentMovementSpeed = 1f;
    [SerializeField] private float attackSpeedDelay = 2f;
    [SerializeField] private int attackPower = 0;

    [Header("NPC Options")]
    [SerializeField] private List<Dialogue> dialogues = new List<Dialogue>();
    [SerializeField] private bool turnToSpeaker = true;
    [SerializeField] private bool isEvil = false;
    [SerializeField] private int aggroRadius = 1;

    [Header("Hamster UI")]
    [SerializeField] private bool isDisplayingName = false;
    [SerializeField] private bool isDisplayingHealth = false;
    [SerializeField] private bool isDisplayingEndurance = false;

    [Header("Current States (Change only for testing)")]
    [SerializeField, ConditionalHide("debug")] private bool isTrading = false;
    [SerializeField, ConditionalHide("debug")] private bool isTalking = false;
    [SerializeField, ConditionalHide("debug")] private bool isInInventory = false;
    [SerializeField, ConditionalHide("debug")] private bool isUsingItem = false;

    [SerializeField, ConditionalHide("debug")] private bool effectsActiv = true;
    [SerializeField, ConditionalHide("debug")] private bool isUsingEndurance = false;
    [SerializeField, ConditionalHide("debug")] private bool tookDamage = false;
    [SerializeField, ConditionalHide("debug")] private bool snapCamera = false;

    private HamsterGameManager hamsterGameManager;
    private Dialogue dialogue = null;
    private readonly string path = "Assets/Objects/Hamster/Player/hamster_";
    private int currentLevel = 0; // See Tile class (var.: level)

    #region Getter and Setter
    public string Name
    {
        get { return hamsterName; }
        set { hamsterName = value; }
    }

    public int CurrentLevel
    {
        get { return currentLevel; }
        set { currentLevel = value; }
    }

    public List<ItemSlot> Inventory
    {
        get { return inventory; }
        set { inventory = value; }
    }

    public bool TookDamage
    {
        get { return tookDamage; }
        set { tookDamage = value; }
    }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public List<Dialogue> NPCDialogues
    {
        get { return dialogues; }
    }

    public bool SnapCamera
    {
        get { return snapCamera; }
        set { snapCamera = value; }
    }

    public bool Respawn
    {
        get { return respawn; }
        set { respawn = value; }
    }

    public int HealthPoints
    {
        get { return healthPoints; }
        set { healthPoints = value; }
    }

    public bool GodMode
    {
        get { return godMode; }
        set { godMode = value; }
    }

    public Transform MovePoint
    {
        get { return movePoint; }
        set { movePoint = value; }
    }

    public bool IsEvil
    {
        get { return isEvil; }
        set { isEvil = value; }
    }

    public int AggroRadius
    {
        get { return aggroRadius; }
        set { aggroRadius = value; }
    }

    public float AttackSpeedDelay
    {
        get { return attackSpeedDelay; }
        set { attackSpeedDelay = value; }
    }

    public int AttackPower
    {
        get { return attackPower; }
        set { attackPower = value; }
    }

    public int EndurancePoints
    {
        get { return endurancePoints; }
        set { endurancePoints = value; }
    }

    public int HealthPointsFull
    {
        get { return healthPointsFull; }
        set { healthPointsFull = value; }
    }

    public MovementPattern Pattern
    {
        get { return movementPattern; }
        set { movementPattern = value; }
    }

    public int EndurancePointsFull
    {
        get { return endurancePointsFull; }
        set { endurancePointsFull = value; }
    }

    public Vector2 StartPoint
    {
        get { return startPoint; }
        set { startPoint = value; }
    }

    public bool IsInInventory
    {
        get { return isInInventory; }
        set { isInInventory = value; }
    }

    public bool IsNPC
    {
        get { return isNPC; }
        set { isNPC = value; }
    }

    public bool MoveRandom
    {
        get { return moveRandom; }
        set { moveRandom = value; }
    }

    public bool IsUsingEndurance
    {
        get { return isUsingEndurance; }
        set { isUsingEndurance = value; }
    }

    public bool IsTrading
    {
        get { return isTrading; }
        set { isTrading = value; }
    }

    public bool IsUsingItem
    {
        get { return isUsingItem; }
        set { isUsingItem = value; }
    }

    public bool IsTalking
    {
        get { return isTalking; }
        set { isTalking = value; }
    }

    public bool IsDisplayingName
    {
        get { return isDisplayingName; }
        set { isDisplayingName = value; }
    }

    public bool IsDisplayingHealth
    {
        get { return isDisplayingHealth; }
        set { isDisplayingHealth = value; }
    }

    public bool IsDisplayingEndurance
    {
        get { return isDisplayingEndurance; }
        set { isDisplayingEndurance = value; }
    }

    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    public int GrainCount
    {
        get { return grainCount; }
        set { grainCount = value; }
    }

    public bool CanTalk
    {
        get { return canTalk; }
        set { canTalk = value; }
    }

    public bool CanTrade
    {
        get { return canTrade; }
        set { canTrade = value; }
    }

    public bool EffectsActiv
    {
        get { return effectsActiv; }
        set { effectsActiv = value; }
    }

    public bool PlayerControl
    {
        get { return playerControl; }
        set { playerControl = value; }
    }

    public GameObject HamsterObject
    {
        get { return hamsterObject; }
        set { hamsterObject = value; }
    }

    public SpriteRenderer HamsterSpriteRenderer
    {
        get { return spriteRenderer; }
        set { spriteRenderer = value; }
    }

    public int Row
    {
        get { return this.row; }
        set { this.row = value; }
    }

    public int Column
    {
        get { return this.column; }
        set { this.column = value; }
    }

    public LookingDirection Direction
    {
        get { return this.direction; }
        set { this.direction = value; }
    }

    public HamsterColor Color
    {
        get { return hamsterColor; }
        set { hamsterColor = value; }
    }

    public int MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    #endregion

    public Hamster(int column = 0, int row = 0, LookingDirection direction = LookingDirection.East, int grainCount = 0, string name = "Ham", int healthPoints = 0, int endurancePoints = 0, HamsterColor color = HamsterColor.Orange, bool playerControl = false)
    {
        this.row = row;
        this.column = column;
        this.direction = direction;
        this.currentLevel = 0;

        if (string.Compare(name, string.Empty) == 0)
            this.hamsterName = "Ham";
        else
            this.hamsterName = name;

        this.grainCount = Mathf.Abs(grainCount);
        this.healthPoints = Mathf.Abs(healthPoints);
        this.endurancePoints = Mathf.Abs(endurancePoints);
        this.healthPointsFull = Mathf.Abs(healthPoints);
        this.hamsterColor = color;
        this.playerControl = playerControl;
        this.isTrading = false;
        this.isTalking = false;
        this.isInInventory = false;

        this.id = Territory.GetInstance().GetHamsters().Count;
        Territory.GetInstance().AddHamster(Instantiate(this));

        hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();

        this.DisplayName(this.isDisplayingName);
        this.DisplayHealth(this.isDisplayingHealth);
        this.DisplayEndurance(this.isDisplayingEndurance);
        Territory.GetInstance().UpdateHamsterProperties(this, createNameUI: true);
        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    public void Save()
    {
        string[] assets = AssetDatabase.FindAssets("hamster_");
        for (int i = 0; i < assets.Length; i++)
        {
            if (string.Compare(assets[i], "hamster_" + this.id + "_" + this.hamsterName + ".asset") == 0)
            {
                AssetDatabase.SaveAssets();
                return;
            }
        }
        AssetDatabase.CreateAsset(this, path + this.id + "_" + this.hamsterName + ".asset");
    }

    /*
     * For one Task
     */
    public void RandomMove()
    {
        System.Random rnd = new System.Random();
        int rndInt = rnd.Next(0, 2);

        if (rndInt == 0)
        {
            int rndTurnInt = rnd.Next(1, 4);
            for (int i = 0; i < rndTurnInt; i++)
            {
                this.TurnLeft();
                Territory.GetInstance().UpdateHamsterProperties(this);
            }
        }
        else
        {
            this.Move();
            Territory.GetInstance().UpdateHamsterProperties(this);
        }
    }

    public void SetRespawn(bool b)
    {
        this.respawn = b;
        this.startPoint = new Vector2(this.column, this.row);
        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    public void SetGodMode(bool b)
    {
        this.godMode = b;
        Territory.GetInstance().UpdateHamsterProperties(this);
    }


    /// <summary>
    /// Hamster turns to the left. (Anticlockwise: North -> West -> South -> East).
    /// </summary>
    public virtual void TurnLeft()
    {
        string turnLeftString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "TURN_LEFT").Result;

        int currentDirection = (int)this.direction;
        currentDirection -= 1;
        if(currentDirection < 0)
        {
            currentDirection = 3;
        }
        if (!this.isNPC)
            Debug.Log(this.hamsterName + turnLeftString);
        this.direction = (LookingDirection)currentDirection;
        Territory.GetInstance().UpdateRotation(this.direction, this);

        /* Check here if the player/hamster is moving, if yes, disable UIs like inventory/trade/dialogue */
        if (this.isTrading)
        {
            HamsterGameManager.isTrading = false;
            this.isTrading = false;
            DisplayTradeWindow(HamsterGameManager.hamster1, HamsterGameManager.hamster2);
        }

        if (this.isTalking)
        {
            SetWindows(dialogueUI: false);
        }

        if (this.isInInventory)
        {
            DisplayInventory();
        }

        if (HamsterGameManager.hamster1 != null &&
            HamsterGameManager.hamster2 != null)
        {
            Territory.GetInstance().UpdateHamsterProperties(HamsterGameManager.hamster1);
            Territory.GetInstance().UpdateHamsterProperties(HamsterGameManager.hamster2);
        }

    }

    /// <summary>
    /// Get the current Position of this hamster
    /// </summary>
    /// <returns></returns>
    public Vector2 GetHamsterPosition()
    {
        return new Vector2((int)this.column, (int)this.row);
    }

    /// <summary>
    /// Pick up one grain
    /// </summary>
    public virtual void PickUpGrain()
    {
        string pickUpGrainString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "PICK_UP_GRAIN").Result;
        string noGrainsString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "NO_GRAINS").Result;

        if (Territory.GetInstance().GetTileAt(this.column, this.row).hasGrain) // Territory.GetInstance().GetGrainCountAtTile(this.column, this.row) > 0
        {
            this.grainCount += 1;
            Territory.GetInstance().UpdateHamsterGrainCount(this);
            Territory.changeGrainCount = true;
            Territory.addGrainCount = false;
            Territory.GetInstance().UpdateTile(this.column, this.row, hamster: this);
            Debug.Log(this.hamsterName + pickUpGrainString);
        }
        else
        {
            if (this.playerControl || this.isNPC) return;
            Debug.LogError(this.hamsterName + noGrainsString + "\nPosition (" + this.column + ", " + this.row + ")");
        }
    }

    /// <summary>
    /// Pick up one item
    /// </summary>
    /*
        WARNING: Durig the creation of a new item, the itemname and the texturename /tilename
                 has to be the same, otherwise it is not gonna work!
     */
    public void PickUpItem()
    {
        string noItemString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "NO_ITEM_PICK_UP").Result;
        string pickUpItemString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "PICK_UP_ITEM").Result;

        Item item = Territory.GetItemAt(this.column, this.row);
        item = Instantiate(item);

        if (item != null)
        {
            this.AddItem(item);
            Territory.GetInstance().UpdateTile(this.column, this.row, true, this);
            Debug.Log(this.hamsterName + pickUpItemString + item.Name);
        }
        else
        {
            if (this.playerControl || this.isNPC) return;
            Debug.LogError(this.hamsterName + noItemString + "\nPosition (" + this.column + ", " + this.row + ")");
        }
    }

    /// <summary>
    /// Set the max amount of health <paramref name="points"/> the hamster has.
    /// </summary>
    /// <param name="points"></param>
    public void SetHealthPoints(int points)
    {
        points = Mathf.Abs(points);
        this.healthPoints = points;

        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    /// <summary>
    /// Set the max amount of endurance <paramref name="points"/> the hamster has.
    /// </summary>
    /// <param name="points"></param>
    public void SetEndurancePoints(int points)
    {
        points= Mathf.Abs(points);
        this.endurancePoints = points;

        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    /// <summary>
    /// Remove a specific amount of health <paramref name="points"/> (Not changing the max amount).
    /// </summary>
    /// <param name="points"></param>
    public void Damage(int points = 1)
    {
        string noMoreHealthString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "NO_HEALTH").Result;

        points = Mathf.Abs(points);
        this.healthPointsFull -= points;
        this.tookDamage = true;
        if (this.healthPointsFull <= 0 && this.respawn && !this.godMode)
        {
            Debug.Log(this.hamsterName + noMoreHealthString);
            this.column = (int)startPoint.x;
            this.row = (int)startPoint.y;

            this.healthPointsFull = this.healthPoints;
        }
        else if (this.healthPointsFull <= 0 && !this.respawn && !this.godMode)
        {
            this.hamsterObject.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;

            this.isDisplayingHealth = false;
            this.isDisplayingEndurance = false;
            this.isDisplayingName = false;

            Destroy(this.hamsterObject);
            Territory.GetInstance().RemoveHamster(this);
            return;
        }

        Territory.GetInstance().UpdateHamsterProperties(this, updateHealthUI: true);
    }

    /// <summary>
    /// Remove a specific amount of endurance <paramref name="points"/> the hamster has (Not changing the max amount).
    /// </summary>
    /// <param name="points"></param>
    public void DamageEndurance(int points = 1)
    {
        string noMoreEnduranceString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "NO_ENDURANCE").Result;

        points = Math.Abs(points);
        this.endurancePointsFull -= points;
        if (this.endurancePointsFull <= 0)
        {
            Debug.Log(this.hamsterName + noMoreEnduranceString);
        }

        Territory.GetInstance().UpdateHamsterProperties(this, updateEnduranceUI: true);
    }

    /// <summary>
    /// Heal a specific amount of health <paramref name="points"/> (Not changing the max amount)
    /// </summary>
    /// <param name="points"></param>
    public void HealHamster(int points = 1)
    {
        string healthFullString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "HEALTH_FULL").Result;

        points = Mathf.Abs(points);
        if (this.healthPointsFull == this.healthPoints)
        {
            Debug.Log(this.hamsterName + healthFullString);
            return;
        }
        this.healthPointsFull += points;
        if (this.healthPointsFull > this.healthPoints)
        {
            this.healthPointsFull = this.healthPoints;
        }
        
        Territory.GetInstance().UpdateHamsterProperties(this, updateHealthUI: true);
    }

    public void HealEndurance(int points = 1)
    {
        string enduranceFullString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "ENDURANCE_FULL").Result;

        points = Mathf.Abs(points);
        if (this.endurancePointsFull == this.endurancePoints)
        {
            Debug.Log(this.hamsterName + enduranceFullString);
            return;
        }
        this.endurancePointsFull += points;
        if (this.endurancePointsFull > this.endurancePoints)
        {
            this.endurancePointsFull = this.endurancePoints;
        }

        Territory.GetInstance().UpdateHamsterProperties(this, updateEnduranceUI: true);
    }


    /// <summary>
    /// Add a specific <paramref name="amount"/> of the item with the specific <paramref name="name"/> to the hamsters inventory. Default <paramref name="amount"/> = 1.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="amount"></param>
    public void AddItem(string name, int amount = 1)
    {
        ItemCollection itemCollection = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<ItemCollection>();
        Item item = itemCollection.GetItem(name);
        item = Instantiate(item);

        this.AddItem(item, amount);
    }

    /// <summary>
    /// Add a specific <paramref name="amount"/> of the item with the specific <paramref name="id"/> to the hamsters inventory. Default <paramref name="amount"/> = 1.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="amount"></param>
    public void AddItem(int id, int amount = 1)
    {
        ItemCollection itemCollection = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<ItemCollection>();
        Item item = itemCollection.GetItem(id);

        this.AddItem(item, amount);
    }

    /// <summary>
    /// Füge ein <paramref name="item"/> dem Inventar hinzu, <paramref name="amount"/> kann optional mitangeben werden. Andernfalls wird ein "1" <paramref name="item"/> hinzugefügt.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    public void AddItem(Item item, int amount = 1)
    {
        if (item == null) return;

        /* 
         * Check if this item already exists in the hamsters inventory.
         */
        for (int i = 0; i < this.inventory.Count; i++)
        {
            /* 
             * Found the item, and the stack isn't full. 
             */
            if (this.inventory[i].item.Id == item.Id &&
                this.inventory[i].quantity < item.StackAmount &&
                this.inventory[i].item.SlotId == item.SlotId)
            {
                /* 
                 * If you can just add the amount of items to the stack without overloading the stack.
                 */
                if (this.inventory[i].quantity + amount <= item.StackAmount)
                {
                    this.inventory[i].quantity += amount;
                }
                /* If you can't add the specific amount without overloading the current itemSlot */
                else
                {
                    int tmpAmount = item.StackAmount - (this.inventory[i].quantity + amount);
                    this.inventory[i].quantity = item.StackAmount;
                    Territory.GetInstance().UpdateHamsterProperties(this);
                    /* Call AddItem again with new values (Difference). */
                    this.AddItem(item, tmpAmount);
                }
                Territory.GetInstance().UpdateHamsterProperties(this);
                return;
            }
            /* If the item was found, but the slotId does not fit
             * Hint: (This else is only for the case if two hamsters trading)
             *          Each item is connected to a slot through a slotId,
             *          in case buying an item from another hamster, 
             *          the slotIds can or cannot be equal.
             *          For this reason we are entering this else
             *          
             */
            else if (this.inventory[i].item.Id == item.Id &&
                this.inventory[i].quantity < item.StackAmount &&
                this.inventory[i].item.SlotId != item.SlotId)
            {
                if (this.inventory[i].quantity + amount <= item.StackAmount)
                {
                    this.inventory[i].quantity += amount;
                }
                else
                {
                    int tmpAmount = item.StackAmount - (this.inventory[i].quantity + amount);
                    this.inventory[i].quantity = item.StackAmount;
                    Territory.GetInstance().UpdateHamsterProperties(this);
                    this.AddItem(item, tmpAmount);
                }
                Territory.GetInstance().UpdateHamsterProperties(this);
                return;
            }
        }
        /* This section is only, if the hamster doesn't own this item. */

        ItemSlot slot = new ItemSlot();

        /* Find next highest SlotId in the inventory */
        int newSlotId = int.MinValue;
        foreach (ItemSlot itemSlot in this.inventory)
        {
            if (itemSlot.slotId >= newSlotId)
            {
                newSlotId = itemSlot.slotId + 1;
            }
        }

        /* Item and ItemSlot have the same SlotId */
        slot.slotId = newSlotId;
        item.SlotId = newSlotId;
        slot.item = Instantiate(item);

        /* 
         * Überprüfe hier ob der itemStack überlaufen würde.
         * Rest ist dann wieder wie oben
         */
        if (item.StackAmount >= amount)
        {
            slot.quantity = amount;
        }
        else
        {
            slot.quantity = item.StackAmount;
            this.inventory.Add(slot);
            this.inventory.Sort();

            Territory.GetInstance().UpdateHamsterProperties(this);

            this.AddItem(item, Mathf.Abs(item.StackAmount - amount));
            return;
        }
        

        this.inventory.Add(slot);
        this.inventory.Sort();
        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    /// <summary>
    /// Entferne ein <paramref name="item"/> aus dem Inventar, <paramref name="amount"/> kann optional mitangeben werden. Andernfalls wird ein "1" <paramref name="item"/> entfernt.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    public void RemoveItem(Item item, int amount = 1)
    {
        ItemSlot slot = null;

        /* Finde das Item mit der SlotId und der item ID */
        foreach (ItemSlot m_slot in this.inventory)
        {
            if (m_slot.item.Id == item.Id)
            {
                slot = m_slot;
                break;
            }
        }

        /* Falls das resultat der suche oben null ist darf nicht weiter ausgeführt werden. */
        if (slot == null) return;
        for (int i = 0; i < amount; i++)
        {
            slot.quantity -= 1;

            if (slot.quantity == 0)
            {
                RemoveItemFromList(slot.slotId);
                return;
            }

            Territory.GetInstance().UpdateHamsterProperties(this);
        }
    }

    /// <summary>
    /// Entferne ein <paramref name="item"/> mit dem <paramref name="name"/>n aus dem Inventar, <paramref name="amount"/> kann optional mitangeben werden. Andernfalls wird ein "1" <paramref name="item"/> entfernt.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="amount"></param>
    public void RemoveItem(string name, int amount = 1)
    {
        ItemCollection itemCollection = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<ItemCollection>();
        Item item = itemCollection.GetItem(name);

        this.RemoveItem(item, amount);
    }

    /// <summary>
    /// Entferne ein Item mit der speziellen <paramref name="id"/> aus dem Inventar, <paramref name="amount"/> kann optional mitangeben werden. Andernfalls wird ein "1" <paramref name="item"/> entfernt.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="amount"></param>
    public void RemoveItem(int id, int amount = 1)
    {
        ItemCollection itemCollection = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<ItemCollection>();
        Item item = itemCollection.GetItem(id);

        this.RemoveItem(item, amount);
    }

    private void RemoveItemFromList(int slotId)
    {
        for (int i = 0; i < this.inventory.Count; i++)
        {
            if (slotId == this.inventory[i].slotId)
            {
                if (this.inventory[i].quantity == 0)
                {
                    this.inventory.RemoveAt(i);
                    this.inventory.Sort();
                    Territory.GetInstance().UpdateHamsterProperties(this);
                    return;
                }
                Territory.GetInstance().UpdateHamsterProperties(this);
            }
        }
    }

    public void SetCameraSnap(bool b)
    {
        this.snapCamera = b;
        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    /// <summary>
    /// Aktiviere/Deaktiviere bestimmte UI's.
    /// <para>Allgemeine UI, für Aufgaben etc. <paramref name="generalUI"/></para>
    /// <para>UI fürs Handeln <paramref name="tradeUI"/></para>
    /// <para>Inventar UI <paramref name="inventoryUI"/></para>
    /// <para>Dialog UI <paramref name="dialogueUI"/></para>
    /// </summary>
    /// <param name="generalUI"></param>
    /// <param name="tradeUI"></param>
    /// <param name="inventoryUI"></param>
    /// <param name="dialogueUI"></param>
    private void SetWindows(bool generalUI = false, bool tradeUI = false, bool inventoryUI = false, bool dialogueUI = false)
    {
        hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();

        hamsterGameManager.SetCanvasVisibility(hamsterGameManager.tradeCanvas, tradeUI);
        hamsterGameManager.SetCanvasVisibility(hamsterGameManager.inventoryCanvas, inventoryUI);
        hamsterGameManager.SetCanvasVisibility(hamsterGameManager.dialogueCanvas, dialogueUI);
        hamsterGameManager.SetCanvasVisibility(hamsterGameManager.generalUI, generalUI);
    }

    /*
     * Hamster[] max size is 2, because max. 2 hamsters can trade.
     */
    private void SetHamsterUI(Hamster[] hamster)
    {
        hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();

        switch (hamster[0].Color)
        {
            case HamsterColor.Orange:
                if (hamster.Length == 1)
                {
                    hamsterGameManager.hamsterImage.sprite = hamsterGameManager.hamsterSprites[1];
                }
                else
                {
                    hamsterGameManager.tradeHamster1Image.sprite = hamsterGameManager.hamsterSprites[1];
                }
                break;
            case HamsterColor.Black:
                if (hamster.Length == 1)
                {
                    hamsterGameManager.hamsterImage.sprite = hamsterGameManager.hamsterSprites[5];
                }
                else
                {
                    hamsterGameManager.tradeHamster1Image.sprite = hamsterGameManager.hamsterSprites[5];
                }
                break;
            case HamsterColor.Blue:
                if (hamster.Length == 1)
                {
                    hamsterGameManager.hamsterImage.sprite = hamsterGameManager.hamsterSprites[9];
                }
                else
                {
                    hamsterGameManager.tradeHamster1Image.sprite = hamsterGameManager.hamsterSprites[9];
                }
                break;
            case HamsterColor.White:
                if (hamster.Length == 1)
                {
                    hamsterGameManager.hamsterImage.sprite = hamsterGameManager.hamsterSprites[13];
                }
                else
                {
                    hamsterGameManager.tradeHamster1Image.sprite = hamsterGameManager.hamsterSprites[13];
                }
                break;
            case HamsterColor.Grey:
                if (hamster.Length == 1)
                {
                    hamsterGameManager.hamsterImage.sprite = hamsterGameManager.hamsterSprites[17];
                }
                else
                {
                    hamsterGameManager.tradeHamster1Image.sprite = hamsterGameManager.hamsterSprites[17];
                }
                break;
            case HamsterColor.Purple:
                if (hamster.Length == 1)
                {
                    hamsterGameManager.hamsterImage.sprite = hamsterGameManager.hamsterSprites[21];
                }
                else
                {
                    hamsterGameManager.tradeHamster1Image.sprite = hamsterGameManager.hamsterSprites[21];
                }
                break;
            case HamsterColor.Pink:
                if (hamster.Length == 1)
                {
                    hamsterGameManager.hamsterImage.sprite = hamsterGameManager.hamsterSprites[25];
                }
                else
                {
                    hamsterGameManager.tradeHamster1Image.sprite = hamsterGameManager.hamsterSprites[25];
                }
                break;
            case HamsterColor.FullBrown:
                if (hamster.Length == 1)
                {
                    hamsterGameManager.hamsterImage.sprite = hamsterGameManager.hamsterSprites[29];
                }
                else
                {
                    hamsterGameManager.tradeHamster1Image.sprite = hamsterGameManager.hamsterSprites[29];
                }
                break;
            case HamsterColor.FullBlue:
                if (hamster.Length == 1)
                {
                    hamsterGameManager.hamsterImage.sprite = hamsterGameManager.hamsterSprites[33];
                }
                else
                {
                    hamsterGameManager.tradeHamster1Image.sprite = hamsterGameManager.hamsterSprites[33];
                }
                break;
            case HamsterColor.FullGrey:
                if (hamster.Length == 1)
                {
                    hamsterGameManager.hamsterImage.sprite = hamsterGameManager.hamsterSprites[37];
                }
                else
                {
                    hamsterGameManager.tradeHamster1Image.sprite = hamsterGameManager.hamsterSprites[37];
                }
                break;
            case HamsterColor.FullWhite:
                if (hamster.Length == 1)
                {
                    hamsterGameManager.hamsterImage.sprite = hamsterGameManager.hamsterSprites[41];
                }
                else
                {
                    hamsterGameManager.tradeHamster1Image.sprite = hamsterGameManager.hamsterSprites[41];
                }
                break;
            case HamsterColor.Evil:
                if (hamster.Length == 1)
                {
                    hamsterGameManager.hamsterImage.sprite = hamsterGameManager.hamsterSprites[45];
                }
                else
                {
                    hamsterGameManager.tradeHamster1Image.sprite = hamsterGameManager.hamsterSprites[45];
                }
                break;
            default:
                if (hamster.Length == 1)
                {
                    hamsterGameManager.hamsterImage.sprite = hamsterGameManager.hamsterSprites[2];
                }
                else
                {
                    hamsterGameManager.tradeHamster1Image.sprite = hamsterGameManager.hamsterSprites[2];
                }
                break;

        }

        if (hamster.Length > 1)
        {
            switch (hamster[1].Color)
            {
                case HamsterColor.Orange:
                    hamsterGameManager.tradeHamster2Image.sprite = hamsterGameManager.hamsterSprites[1];
                    break;
                case HamsterColor.Black:
                    hamsterGameManager.tradeHamster2Image.sprite = hamsterGameManager.hamsterSprites[5];
                    break;
                case HamsterColor.Blue:
                    hamsterGameManager.tradeHamster2Image.sprite = hamsterGameManager.hamsterSprites[9];
                    break;
                case HamsterColor.White:
                    hamsterGameManager.tradeHamster2Image.sprite = hamsterGameManager.hamsterSprites[13];
                    break;
                case HamsterColor.Grey:
                    hamsterGameManager.tradeHamster2Image.sprite = hamsterGameManager.hamsterSprites[17];
                    break;
                case HamsterColor.Purple:
                    hamsterGameManager.tradeHamster2Image.sprite = hamsterGameManager.hamsterSprites[21];
                    break;
                case HamsterColor.Pink:
                    hamsterGameManager.tradeHamster2Image.sprite = hamsterGameManager.hamsterSprites[25];
                    break;
                case HamsterColor.FullBrown:
                    hamsterGameManager.tradeHamster2Image.sprite = hamsterGameManager.hamsterSprites[29];
                    break;
                case HamsterColor.FullBlue:
                    hamsterGameManager.tradeHamster2Image.sprite = hamsterGameManager.hamsterSprites[33];
                    break;
                case HamsterColor.FullGrey:
                    hamsterGameManager.tradeHamster2Image.sprite = hamsterGameManager.hamsterSprites[37];
                    break;
                case HamsterColor.FullWhite:
                    hamsterGameManager.tradeHamster2Image.sprite = hamsterGameManager.hamsterSprites[41];
                    break;
                case HamsterColor.Evil:
                    hamsterGameManager.tradeHamster2Image.sprite = hamsterGameManager.hamsterSprites[45];
                    break;
                default:
                    hamsterGameManager.tradeHamster2Image.sprite = hamsterGameManager.hamsterSprites[2];
                    break;
            }
        }
        

        for (int i = 0; i < hamster.Length; i++)
        {
            if (hamster.Length == 1)
            {
                hamsterGameManager.hamsterGrains.SetText(hamster[i].GrainCount.ToString());
                hamsterGameManager.hamsterName.SetText(hamster[i].Name);
            }
            else
            {
                if (i == 0)
                {
                    hamsterGameManager.tradeHamster1Grains.SetText(hamster[i].GrainCount.ToString());
                    hamsterGameManager.tradeHamster1Name.SetText(hamster[i].Name);
                }
                else
                {
                    hamsterGameManager.tradeHamster2Grains.SetText(hamster[i].GrainCount.ToString());
                    hamsterGameManager.tradeHamster2Name.SetText(hamster[i].Name);
                }
            }
        }
    }

    /// <summary>
    /// Lege ein Korn ab
    /// </summary>
    public virtual void DropGrain()
    {
        string noGrainsDropString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "NO_GRAINS_DROP").Result;
        string dropGrainString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "DROP_GRAIN").Result;
        string tileFullString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "TILE_FULL").Result;

        if (this.grainCount == 0)
        {
            /* Falls die Spielerkontrolle aktiv ist, zeige kein Error im Log und pausiere das spiel auch nicht. */
            if (this.playerControl) return;
            Debug.LogError(this.hamsterName + noGrainsDropString);
            return;
        }
        
        /* Überprüfe ob das Feld schon voll ist (Maximale Kornanzahl pro Feld ist 9) */
        if (Territory.GetInstance().GetGrainCountAtTile(this.column, this.row) < 9)
        {
            this.grainCount -= 1;
            Territory.GetInstance().UpdateHamsterGrainCount(this);
            Territory.changeGrainCount = true;
            Territory.addGrainCount = true;
            Territory.GetInstance().UpdateTile(this.column, this.row, hamster: this);
            Debug.Log(this.hamsterName + dropGrainString);
        }
        else
        {
            Debug.Log(this.hamsterName + tileFullString);
        }
    }


    /// <summary>
    /// <para>Starte einen Handel mit einem anderen Hamster. Falls es keinen Hamster zum handeln gibt wird ein Fehler geworfen.</para>
    /// <para>Dieser Hamster kann nur einen Handel starten, wenn sich vor ihm, in Blickrichtung + 1, ein anderer Hamster befindet.</para>
    /// </summary>
    public void Trade()
    {
        string noTradeString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "NO_TRADE").Result;
        string noTradeInterString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "NO_TRADE_INTER").Result;

        Hamster hamster = null;

        switch (this.direction)
        {
            case LookingDirection.North:
                hamster = Territory.GetInstance().GetHamsterAt(this.column, this.row + 1);
                
                if (hamster == null)
                {
                    if (this.playerControl || this.isNPC) return;
                    Debug.LogError(this.hamsterName + noTradeString);
                    return;
                }

                if (hamster != null && hamster.CanTrade)
                {
                    this.isTrading = true;
                    hamster.IsTrading = true;
                    
                    HamsterGameManager.hamster1 = this;
                    HamsterGameManager.hamster2 = hamster;

                    // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
                    Territory.GetInstance().UpdateHamsterProperties(hamster);
                    Territory.GetInstance().UpdateHamsterProperties(this);
                    LookAtHamster(this, hamster);

                    HamsterGameManager.isTrading = true;
                    DisplayTradeWindow(this, hamster);
                }
                else if(hamster != null && !hamster.CanTrade)
                {
                    Debug.Log(this.hamsterName + ": Hamster " + hamster.Name + noTradeInterString);
                }
                break;
            case LookingDirection.South:
                hamster = Territory.GetInstance().GetHamsterAt(this.column, this.row - 1);

                if (hamster == null)
                {
                    if (this.playerControl || this.isNPC) return;
                    Debug.LogError(this.hamsterName + noTradeString);
                    return;
                }

                if (hamster != null && hamster.CanTrade)
                {
                    this.isTrading = true;
                    hamster.IsTrading = true;

                    HamsterGameManager.hamster1 = this;
                    HamsterGameManager.hamster2 = hamster;

                    // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
                    Territory.GetInstance().UpdateHamsterProperties(hamster);
                    Territory.GetInstance().UpdateHamsterProperties(this);
                    LookAtHamster(this, hamster);

                    HamsterGameManager.isTrading = true;
                    DisplayTradeWindow(this, hamster);
                }
                else if (hamster != null && !hamster.CanTrade)
                {
                    Debug.Log(this.hamsterName + ": Hamster " + hamster.Name + noTradeInterString);
                }
                break;
            case LookingDirection.East:
                hamster = Territory.GetInstance().GetHamsterAt(this.column + 1, this.row);

                if (hamster == null)
                {
                    if (this.playerControl || this.isNPC) return;
                    Debug.LogError(this.hamsterName + noTradeString);
                    return;
                }

                if (hamster != null && hamster.CanTrade)
                {
                    this.isTrading = true;
                    hamster.IsTrading = true;

                    HamsterGameManager.hamster1 = this;
                    HamsterGameManager.hamster2 = hamster;

                    // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
                    Territory.GetInstance().UpdateHamsterProperties(hamster);
                    Territory.GetInstance().UpdateHamsterProperties(this);
                    LookAtHamster(this, hamster);

                    HamsterGameManager.isTrading = true;
                    DisplayTradeWindow(this, hamster);
                }
                else if (hamster != null && !hamster.CanTrade)
                {
                    Debug.Log(this.hamsterName + ": Hamster " + hamster.Name + noTradeInterString);
                }
                break;
            case LookingDirection.West:
                hamster = Territory.GetInstance().GetHamsterAt(this.column - 1, this.row);

                if (hamster == null)
                {
                    if (this.playerControl || this.isNPC) return;
                    Debug.LogError(this.hamsterName + noTradeString);
                    return;
                }

                if (hamster != null && hamster.CanTrade)
                {
                    this.isTrading = true;
                    hamster.IsTrading = true;

                    HamsterGameManager.hamster1 = this;
                    HamsterGameManager.hamster2 = hamster;

                    // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
                    Territory.GetInstance().UpdateHamsterProperties(hamster);
                    Territory.GetInstance().UpdateHamsterProperties(this);
                    LookAtHamster(this, hamster);

                    HamsterGameManager.isTrading = true;
                    DisplayTradeWindow(this, hamster);
                }
                else if (hamster != null && !hamster.CanTrade)
                {
                    Debug.Log(this.hamsterName + ": Hamster " + hamster.Name + noTradeInterString);
                }
                break;
        }
    }

    /// <summary>
    /// Buy an <paramref name="item"/> from <paramref name="hamster"/>.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="hamster"></param>
    public void BuyItem(Item item, Hamster hamster)
    {
        // Noch nicht vollständig implementiert...
        hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();

        // Hamster 1 ist der verkäufer
        if (hamster.Id == 0)
        {
            for(int i = 0; i < hamsterGameManager.tradeItemContentHamster1.childCount; i++)
            {
                if (hamsterGameManager.tradeItemContentHamster1.GetChild(i).GetComponent<ItemHolder>().item.SlotId == item.SlotId)
                {
                    hamsterGameManager.tradeItemContentHamster1.GetChild(i).GetComponent<ItemHolder>().SellItem();
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="hamster"></param>
    public void SellItem(Item item, Hamster hamster)
    {

    }

    /// <summary>
    /// Use <paramref name="item"/> once.
    /// </summary>
    /// <param name="item"></param>
    public void UseItem(Item item)
    {
        if (item == null) return;

        // Funktioniert noch nicht so ganz, funktioniert nur temporär
        foreach(ItemSlot slot in inventory)
        {
            if (slot.item.Id == item.Id)
            {
                this.isUsingItem = true;
                Territory.GetInstance().UpdateHamsterProperties(this);

                slot.item.OnUse();
                this.isUsingItem = false;
                /*
                    this.isUsingItem = false;
                    Territory.UpdateHamsterProperties(this);
                */
                this.RemoveItem(item);
                return;
            }
        }
        if (this.playerControl) return;
        Debug.LogError(this.hamsterName + ": Ich besitze das Item " + item.Name + " nicht!");
    }

    /// <summary>
    /// Verwende ein bestimmtes item mit speziellen <paramref name="itemName"/>.
    /// </summary>
    /// <param name="itemName"></param>
    public void UseItem(string itemName)
    {
        ItemCollection itemCollection = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<ItemCollection>();
        Item item = itemCollection.GetItem(itemName);

        UseItem(item);
    }

    /// <summary>
    /// Verwende ein bestimmtes item mit spezieller <paramref name="itemId"/>.
    /// </summary>
    /// <param name="itemId"></param>
    public void UseItem(int itemId)
    {
        ItemCollection itemCollection = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<ItemCollection>();
        Item item = itemCollection.GetItem(itemId);

        UseItem(item);
    }

    /// <summary>
    /// Aktiviere / Deaktiviere den Ausdauerverbrauch (Default = off)
    /// </summary>
    /// <param name="b"></param>
    public void SetEnduranceConsumption(bool b)
    {
        this.isUsingEndurance = b;
        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    /// <summary>
    /// Das Inventar wird angezeigt, ist das Inventar bereits offen wird das Inventar geschlossen
    /// </summary>
    public void DisplayInventory()
    {
        hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();

        /*
         * Entferne alle equipment sprites
         */
        for (int j = 1; j < hamsterGameManager.Equipment.childCount; j++)
        {
            hamsterGameManager.Equipment.GetChild(j).GetChild(1).GetChild(0).GetComponent<Image>().sprite = null;
        }

        if (!isInInventory)
        {
            
            for (int i = 0; i < this.inventory.Count; i++)
            {
                GameObject itemSlot = Instantiate(hamsterGameManager.itemPrefab, hamsterGameManager.itemContent);
                itemSlot.GetComponent<ItemHolder>().item = this.inventory[i].item;
                itemSlot.GetComponent<ItemHolder>().quantity = this.inventory[i].quantity;
                itemSlot.GetComponent<ItemHolder>().itemName.SetText(this.inventory[i].item.Name);
                itemSlot.GetComponent<ItemHolder>().itemBuyPrice.SetText(this.inventory[i].item.BuyPrice.ToString());
                itemSlot.GetComponent<ItemHolder>().itemSellPrice.SetText(this.inventory[i].item.SellPrice.ToString());
                itemSlot.GetComponent<ItemHolder>().itemQuantity.SetText(itemSlot.GetComponent<ItemHolder>().quantity + "/" + this.inventory[i].item.StackAmount.ToString());
                itemSlot.GetComponent<ItemHolder>().itemImage.sprite = itemSlot.GetComponent<ItemHolder>().item.ItemImage;
                if (itemSlot.GetComponent<ItemHolder>().item.IsEquipped)
                {
                    itemSlot.gameObject.GetComponent<Image>().color = itemSlot.GetComponent<ItemHolder>().EquipColor;
                }
                else
                {
                    switch (itemSlot.GetComponent<ItemHolder>().item.ItemRarity)
                    {
                        case Item.Rarity.Normal: itemSlot.gameObject.GetComponent<Image>().color = itemSlot.GetComponent<ItemHolder>().item.Normal; break;
                        case Item.Rarity.Rare: itemSlot.gameObject.GetComponent<Image>().color = itemSlot.GetComponent<ItemHolder>().item.Rare; break;
                        case Item.Rarity.Epic: itemSlot.gameObject.GetComponent<Image>().color = itemSlot.GetComponent<ItemHolder>().item.Epic; break;
                        case Item.Rarity.Legendary: itemSlot.gameObject.GetComponent<Image>().color = itemSlot.GetComponent<ItemHolder>().item.Legendary; break;
                        case Item.Rarity.Unique: itemSlot.gameObject.GetComponent<Image>().color = itemSlot.GetComponent<ItemHolder>().item.Unique; break;
                        default: break;
                    }
                }

                if (itemSlot.GetComponent<ItemHolder>().item.IsEquipped)
                {
                    /*
                     * Aktualisiere hier noch die equipmentslots
                     */
                    switch(itemSlot.GetComponent<ItemHolder>().item.EquipType)
                    {
                        case Item.EquipmentType.Head:
                            hamsterGameManager.Equipment.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().sprite = itemSlot.GetComponent<ItemHolder>().item.ItemImage;
                            break;
                        case Item.EquipmentType.Hands:
                            hamsterGameManager.Equipment.GetChild(2).GetChild(1).GetChild(0).GetComponent<Image>().sprite = itemSlot.GetComponent<ItemHolder>().item.ItemImage;
                            break;
                        case Item.EquipmentType.Foot:
                            hamsterGameManager.Equipment.GetChild(3).GetChild(1).GetChild(0).GetComponent<Image>().sprite = itemSlot.GetComponent<ItemHolder>().item.ItemImage;
                            break;
                        case Item.EquipmentType.Extra1:
                            hamsterGameManager.Equipment.GetChild(4).GetChild(1).GetChild(0).GetComponent<Image>().sprite = itemSlot.GetComponent<ItemHolder>().item.ItemImage;
                            break;
                        case Item.EquipmentType.Extra2:
                            hamsterGameManager.Equipment.GetChild(5).GetChild(1).GetChild(0).GetComponent<Image>().sprite = itemSlot.GetComponent<ItemHolder>().item.ItemImage;
                            break;
                    }
                }
            }
            


            // Lebenspunkte
            for (int i = 1; i <= healthPoints; i++)
            {
                GameObject heartGo = Instantiate(hamsterGameManager.heartPrefab, hamsterGameManager.hamsterHealthPoints);
                if (i <= this.healthPointsFull)
                {
                    heartGo.GetComponent<Image>().sprite = hamsterGameManager.healthSprite[0];
                }
                else if (i > this.healthPointsFull)
                {
                    heartGo.GetComponent<Image>().sprite = hamsterGameManager.healthSprite[1];
                }
            }

            // Ausdauer
            for (int i = 1; i <= endurancePoints; i++)
            {
                GameObject heartGo = Instantiate(hamsterGameManager.enduranceHeartPrefab, hamsterGameManager.hamsterEndurancePoints);
                if (i <= this.endurancePointsFull)
                {
                    heartGo.GetComponent<Image>().sprite = hamsterGameManager.enduranceSprite[0];
                }
                else if (i > this.endurancePointsFull)
                {
                    heartGo.GetComponent<Image>().sprite = hamsterGameManager.enduranceSprite[1];
                }
            }

            SetWindows(inventoryUI: true);

            /* Setze die Namen, Bilder und Kornanzahl der Hamster oben an das UI des Handelfensters. */
            SetHamsterUI(new Hamster[] { this });

            this.isInInventory = true;
        }
        else
        {
            int childCount = hamsterGameManager.itemContent.childCount;

            SetWindows(generalUI: true);

            this.isInInventory = false;

            for (int i = 0; i < childCount; i++)
            {
                Destroy(hamsterGameManager.itemContent.GetChild(i).gameObject);
            }

            for (int i = 0; i < hamsterGameManager.hamsterHealthPoints.childCount; i++)
            {
                Destroy(hamsterGameManager.hamsterHealthPoints.GetChild(i).gameObject);
            }

            for (int i = 0; i < hamsterGameManager.hamsterEndurancePoints.childCount; i++)
            {
                Destroy(hamsterGameManager.hamsterEndurancePoints.GetChild(i).gameObject);
            }

            /*
             * Entferne alle equipment sprites
             */
            for (int j = 1; j < hamsterGameManager.Equipment.childCount; j++)
            {
                hamsterGameManager.Equipment.GetChild(j).GetChild(1).GetChild(0).GetComponent<Image>().sprite = null;
            }
        }
        // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    public void DisplayName(bool b)
    {
        if (this.isDisplayingName != b)
        {
            this.isDisplayingName = b;
            // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
            Territory.GetInstance().UpdateHamsterProperties(this, createNameUI: true);
        }
    }

    public void DisplayHealth(bool b)
    {
        if (this.isDisplayingHealth != b)
        {
            this.isDisplayingHealth = b;
            // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
            Territory.GetInstance().UpdateHamsterProperties(this, createHealthUI: true);
        }
            
    }

    public void DisplayEndurance(bool b)
    {
        if (this.isDisplayingEndurance)
        {
            this.isDisplayingEndurance = b;
            // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
            Territory.GetInstance().UpdateHamsterProperties(this, createEnduranceUI: true);
        }
            
    }

    public void DisplayTradeWindow(Hamster hamster1, Hamster hamster2)
    {
        hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();

        if (hamster1.isTrading && hamster2.isTrading)
        {
            /* Aktiviere den Canvas für den Handel und deaktivere alle anderen UIs */
            SetWindows(tradeUI: true);

            /* Setze die Namen, Bilder und Kornanzahl der Hamster oben an das UI des Handelfensters. */
            SetHamsterUI(new Hamster[] { hamster1, hamster2 });

            /* Füge alle items in die inventare ein "Hamster1" */
            if (hamster1.inventory != null && hamster1.inventory.Count > 0)
            {
                for (int i = 0; i < hamster1.inventory.Count; i++)
                {
                    GameObject itemSlot = Instantiate(hamsterGameManager.itemPrefab, hamsterGameManager.tradeItemContentHamster1);
                    itemSlot.GetComponent<ItemHolder>().item = hamster1.inventory[i].item;
                    itemSlot.GetComponent<ItemHolder>().quantity = hamster1.inventory[i].quantity;
                    itemSlot.GetComponent<ItemHolder>().itemName.SetText(hamster1.inventory[i].item.Name);
                    itemSlot.GetComponent<ItemHolder>().itemBuyPrice.SetText(hamster1.inventory[i].item.BuyPrice.ToString());
                    itemSlot.GetComponent<ItemHolder>().itemSellPrice.SetText(hamster1.inventory[i].item.SellPrice.ToString());
                    itemSlot.GetComponent<ItemHolder>().itemQuantity.SetText(itemSlot.GetComponent<ItemHolder>().quantity + "/" + hamster1.inventory[i].item.StackAmount.ToString());
                    itemSlot.GetComponent<ItemHolder>().itemImage.sprite = itemSlot.GetComponent<ItemHolder>().item.ItemImage;
                }
            }
                

            /* Füge alle items in die inventare ein "Hamster2" */
            if (hamster2.inventory != null && hamster2.inventory.Count > 0)
            {
                for (int i = 0; i < hamster2.inventory.Count; i++)
                {
                    GameObject itemSlot = Instantiate(hamsterGameManager.itemPrefab, hamsterGameManager.tradeItemContentHamster2);
                    itemSlot.GetComponent<ItemHolder>().item = hamster2.inventory[i].item;
                    itemSlot.GetComponent<ItemHolder>().quantity = hamster2.inventory[i].quantity;
                    itemSlot.GetComponent<ItemHolder>().itemName.SetText(hamster2.inventory[i].item.Name);
                    itemSlot.GetComponent<ItemHolder>().itemBuyPrice.SetText(hamster2.inventory[i].item.BuyPrice.ToString());
                    itemSlot.GetComponent<ItemHolder>().itemSellPrice.SetText(hamster2.inventory[i].item.SellPrice.ToString());
                    itemSlot.GetComponent<ItemHolder>().itemQuantity.SetText(itemSlot.GetComponent<ItemHolder>().quantity + "/" + hamster2.inventory[i].item.StackAmount.ToString());
                    itemSlot.GetComponent<ItemHolder>().itemImage.sprite = itemSlot.GetComponent<ItemHolder>().item.ItemImage;
                }
            }
            

            hamster1.canMove = false;
            hamster2.canMove = false;
            if (hamster1.playerControl || hamster2.playerControl)
            {
                Territory.playerCanMove = false;
                Territory.player2CanMove = false;
            }

            //HamsterGameManager.isTrading = true;
        }
        else
        {
            /* Aktiviere den Canvasdie General UI und deaktivere alle anderen UIs */
            SetWindows(generalUI: hamsterGameManager.displayQuestLog);

            hamster1.isTrading = false;
            hamster2.isTrading = false;

            hamster1.canMove = true;
            hamster2.canMove = true;
            if (hamster1.playerControl || hamster2.playerControl)
            {
                Territory.playerCanMove = true;
                // Aktiviere auch wieder die Bewegung von Spieler 2
                Territory.player2CanMove = true;
            }

            for (int i = 0; i < hamsterGameManager.tradeItemContentHamster1.childCount; i++)
            {
                Destroy(hamsterGameManager.tradeItemContentHamster1.GetChild(i).gameObject);
            }

            for (int i = 0; i < hamsterGameManager.tradeItemContentHamster2.childCount; i++)
            {
                Destroy(hamsterGameManager.tradeItemContentHamster2.GetChild(i).gameObject);
            }

            //HamsterGameManager.isTrading = false;
        }
        Territory.GetInstance().UpdateHamsterProperties(HamsterGameManager.hamster1);
        Territory.GetInstance().UpdateHamsterProperties(HamsterGameManager.hamster2);
    }

    private void LookAtHamster(Hamster hamster1, Hamster hamster2)
    {
        if (hamster2.turnToSpeaker)
        {
            /* Change the looking direction of hamster2 */
            if (hamster1.direction == LookingDirection.North)
                hamster2.direction = LookingDirection.South;
            else if (hamster1.direction == LookingDirection.South)
                hamster2.direction = LookingDirection.North;
            else if (hamster1.direction == LookingDirection.East)
                hamster2.direction = LookingDirection.West;
            else if (hamster1.direction == LookingDirection.West)
                hamster2.direction = LookingDirection.East;

            Territory.GetInstance().UpdateHamsterProperties(hamster2);
        }
    }

    public Dialogue GetDialogue()
    {
        return this.dialogue;
    }


    /// <summary>
    /// <para>Starte einen Unterhaltung mit einem anderen Hamster. Falls es keinen Hamster für eine Unterhaltung gibt wird ein Fehler geworfen.</para>
    /// <para>this Hamster kann nur eine Unterhaltung starten, wenn sich vor ihm, in Blickrichtung + 1, ein anderer Hamster befindet.</para>
    /// </summary>
    public void Talk()
    {
        string noDialogueInterString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "NO_DIALOGUE_INTER").Result;
        string noDialogueString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "NO_DIALOGUE").Result;

        Hamster hamster = null;

        switch (this.direction)
        {
            case LookingDirection.North:
                hamster = Territory.GetInstance().GetHamsterAt(this.column, this.row + 1);

                if (hamster == null)
                {
                    if (this.playerControl) return;
                    Debug.LogError(this.hamsterName + noDialogueString);
                    return;
                }

                if (hamster != null && hamster.CanTalk)
                {
                    this.isTalking = true;
                    hamster.IsTalking = true;
                    Territory.GetInstance().UpdateHamsterProperties(hamster);
                    Territory.GetInstance().UpdateHamsterProperties(this);

                    HamsterGameManager.hamster1 = this;
                    HamsterGameManager.hamster2 = hamster;

                    LookAtHamster(this, hamster);

                    //Territory.GetInstance().DisplayDialogueWindow(this, hamster);
                    /* Display the dialogue UI and insert all information */
                    SetWindows(dialogueUI: true);

                    // Find default
                    foreach (Dialogue dialogue in HamsterGameManager.hamster2.dialogues)
                    {
                        foreach (DialogueCondition condition in dialogue.conditions)
                        {
                            if (condition.IsDefault)
                            {
                                this.dialogue = dialogue;
                                break;
                            }
                        }
                        if (this.dialogue != null)
                        {
                            break;
                        }
                    }

                    foreach (Dialogue dialogue in HamsterGameManager.hamster2.dialogues)
                    {
                        foreach (DialogueCondition condition in dialogue.conditions)
                        {
                            if (condition.NeedsItem && condition.NeededItem != null && !condition.IsDone)
                            {
                                for (int i = 0; i < this.inventory.Count; i++)
                                {
                                    if (this.inventory[i].item.Id == condition.NeededItem.Id)
                                    {
                                        condition.IsDone = true;
                                        this.dialogue = dialogue;
                                        Debug.Log(this.dialogue);
                                        break;
                                    }
                                }
                                if (this.dialogue != null)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    FindObjectOfType<DialogueManager>().StartDialogue(this.dialogue, HamsterGameManager.hamster2, this);
                }
                else if (hamster != null && !hamster.CanTalk)
                {
                    Debug.Log(this.hamsterName + ": Hamster " + hamster.Name + noDialogueInterString);
                }
                break;
            case LookingDirection.South:
                hamster = Territory.GetInstance().GetHamsterAt(this.column, this.row - 1);

                if (hamster == null)
                {
                    if (this.playerControl) return;
                    Debug.LogError(this.hamsterName + noDialogueString);
                    return;
                }

                if (hamster != null && hamster.CanTalk)
                {
                    this.isTalking = true;
                    hamster.IsTalking = true;
                    Territory.GetInstance().UpdateHamsterProperties(hamster);
                    Territory.GetInstance().UpdateHamsterProperties(this);

                    HamsterGameManager.hamster1 = this;
                    HamsterGameManager.hamster2 = hamster;

                    LookAtHamster(this, hamster);

                    //Territory.GetInstance().DisplayDialogueWindow(this, hamster);
                    /* Display the dialogue UI and insert all information */
                    SetWindows(dialogueUI: true);

                    // Find default
                    foreach (Dialogue dialogue in HamsterGameManager.hamster2.dialogues)
                    {
                        foreach (DialogueCondition condition in dialogue.conditions)
                        {
                            if (condition.IsDefault)
                            {
                                condition.IsDone = true;
                                this.dialogue = dialogue;
                                break;
                            }
                        }
                        if (this.dialogue != null)
                        {
                            break;
                        }
                    }

                    foreach (Dialogue dialogue in HamsterGameManager.hamster2.dialogues)
                    {
                        foreach (DialogueCondition condition in dialogue.conditions)
                        {
                            if (condition.NeedsItem && condition.NeededItem != null && !condition.IsDone)
                            {
                                for (int i = 0; i < this.inventory.Count; i++)
                                {
                                    if (this.inventory[i].item.Id == condition.NeededItem.Id)
                                    {
                                        condition.IsDone = true;
                                        this.dialogue = dialogue;
                                        break;
                                    }
                                }
                                if (this.dialogue != null)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    FindObjectOfType<DialogueManager>().StartDialogue(this.dialogue, HamsterGameManager.hamster2, this);
                }
                else if (hamster != null && !hamster.CanTalk)
                {
                    Debug.Log(this.hamsterName + ": Hamster " + hamster.Name + noDialogueInterString);
                }
                break;
            case LookingDirection.East:
                hamster = Territory.GetInstance().GetHamsterAt(this.column + 1, this.row);

                if (hamster == null)
                {
                    if (this.playerControl) return;
                    Debug.LogError(this.hamsterName + noDialogueString);
                    return;
                }

                if (hamster != null && hamster.CanTalk)
                {
                    this.isTalking = true;
                    hamster.IsTalking = true;
                    Territory.GetInstance().UpdateHamsterProperties(hamster);
                    Territory.GetInstance().UpdateHamsterProperties(this);

                    HamsterGameManager.hamster1 = this;
                    HamsterGameManager.hamster2 = hamster;

                    LookAtHamster(this, hamster);

                    //Territory.GetInstance().DisplayDialogueWindow(this, hamster);
                    /* Display the dialogue UI and insert all information */
                    SetWindows(dialogueUI: true);

                    // Find default
                    foreach (Dialogue dialogue in HamsterGameManager.hamster2.dialogues)
                    {
                        foreach (DialogueCondition condition in dialogue.conditions)
                        {
                            if (condition.IsDefault)
                            {
                                condition.IsDone = true;
                                this.dialogue = dialogue;
                                break;
                            }
                        }
                        if (this.dialogue != null)
                        {
                            break;
                        }
                    }

                    foreach (Dialogue dialogue in HamsterGameManager.hamster2.dialogues)
                    {
                        foreach (DialogueCondition condition in dialogue.conditions)
                        {
                            if (condition.NeedsItem && condition.NeededItem != null && !condition.IsDone)
                            {
                                for (int i = 0; i < this.inventory.Count; i++)
                                {
                                    if (this.inventory[i].item.Id == condition.NeededItem.Id)
                                    {
                                        condition.IsDone = true;
                                        this.dialogue = dialogue;
                                        break;
                                    }
                                }
                                if (this.dialogue != null)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    FindObjectOfType<DialogueManager>().StartDialogue(this.dialogue, HamsterGameManager.hamster2, this);
                }
                else if (hamster != null && !hamster.CanTalk)
                {
                    Debug.Log(this.hamsterName + ": Hamster " + hamster.Name + noDialogueInterString);
                }
                break;
            case LookingDirection.West:
                hamster = Territory.GetInstance().GetHamsterAt(this.column - 1, this.row);

                if (hamster == null)
                {
                    if (this.playerControl) return;
                    Debug.LogError(this.hamsterName + noDialogueString);
                    return;
                }

                if (hamster != null && hamster.CanTalk)
                {
                    this.isTalking = true;
                    hamster.IsTalking = true;
                    Territory.GetInstance().UpdateHamsterProperties(hamster);
                    Territory.GetInstance().UpdateHamsterProperties(this);

                    HamsterGameManager.hamster1 = this;
                    HamsterGameManager.hamster2 = hamster;

                    LookAtHamster(this, hamster);

                    //Territory.GetInstance().DisplayDialogueWindow(this, hamster);
                    /* Display the dialogue UI and insert all information */
                    SetWindows(dialogueUI: true);

                    // Find default
                    foreach (Dialogue dialogue in HamsterGameManager.hamster2.dialogues)
                    {
                        foreach (DialogueCondition condition in dialogue.conditions)
                        {
                            if (condition.IsDefault)
                            {
                                this.dialogue = dialogue;
                                break;
                            }
                        }
                        if (this.dialogue != null)
                        {
                            break;
                        }
                    }

                    foreach (Dialogue dialogue in HamsterGameManager.hamster2.dialogues)
                    {
                        foreach (DialogueCondition condition in dialogue.conditions)
                        {
                            if (condition.NeedsItem && condition.NeededItem != null && !condition.IsDone)
                            {
                                for (int i = 0; i < this.inventory.Count; i++)
                                {
                                    if (this.inventory[i].item.Id == condition.NeededItem.Id)
                                    {
                                        condition.IsDone = true;
                                        this.dialogue = dialogue;
                                        break;
                                    }
                                }
                                if (this.dialogue != null)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    FindObjectOfType<DialogueManager>().StartDialogue(this.dialogue, HamsterGameManager.hamster2, this);
                }
                else if (hamster != null && !hamster.CanTalk)
                {
                    Debug.Log(this.hamsterName + ": Hamster " + hamster.Name + noDialogueInterString);
                }
                break;
        }

        // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
        Territory.GetInstance().UpdateHamsterProperties(hamster);
        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    /// <summary>
    /// Erhalte die Anzahl der Körner, auf dem sich der Hamster befindet.
    /// </summary>
    /// <returns></returns>
    public int GetGrainCountAtCurrentPosition()
    {
        return Territory.GetInstance().GetGrainCountAtTile(this.column, this.row);
    }

    /// <summary>
    /// Erhalte die Anzahl der Körner, die der Hamster bis jetzt aufgesammelt hat.
    /// </summary>
    /// <returns></returns>
    public int GetGrainCount()
    {
        return this.grainCount;
    }

    /// <summary>
    /// Überprüfe ob der Hamster sich auf einem Korn befindet.
    /// </summary>
    /// <returns></returns>
    public bool IsOnGrain()
    {
        if(Territory.GetInstance().GetTileAt(this.column, this.row).type == Tile.TileType.Grain)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Entferne aktive Effekte vom Hamster
    /// </summary>
    /// <param name="useEffect"></param>
    public void SetEquipmentEffects(bool useEffect)
    {
        this.effectsActiv = useEffect;
        // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    /// <summary>
    /// Hamster is moving in the direction, he/she looks at. Move moves the hamster one tile forward.
    /// </summary>
    public virtual void Move()
    {
        string movingFString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "MOVE").Result;
        string noEnduranceFString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "NO_ENDURANCE").Result;
        string frontNotClearFString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "FRONT_NOT_CLEAR").Result;

        if (FrontIsClear())
        {
            switch (this.direction)
            {
                case LookingDirection.East:
                    /* 
                     * Falls der moveSpeed > 1 ist, muss überprüft werden, ob der Hamster durch
                     * eine Wand laufen würde
                     */
                    if (this.moveSpeed > 1)
                    {
                        for (int i = 1; i <= this.moveSpeed; i++)
                        {
                            if (Territory.GetInstance().GetTileAt(this.column + i, this.row).type == Tile.TileType.Wall)
                            {
                                if (this.isUsingEndurance && this.endurancePointsFull >= 1)
                                {
                                    this.column += i - 1;
                                    this.endurancePointsFull -= 1;
                                }
                                else if (this.isUsingEndurance && this.endurancePointsFull == 0)
                                    Debug.LogError(this.hamsterName + noEnduranceFString);

                                if (!this.isUsingEndurance)
                                {
                                    this.column += i - 1;
                                }
                                return;
                            }
                        }
                    }
                    if (this.isUsingEndurance && this.endurancePointsFull >= 1)
                    {
                        this.column += this.moveSpeed;
                        this.endurancePointsFull -= 1;
                    }
                    else if (this.isUsingEndurance && this.endurancePointsFull == 0)
                        Debug.LogError(this.hamsterName + noEnduranceFString);
                    else if (!this.isUsingEndurance)
                        this.column += this.moveSpeed;
                    break;
                case LookingDirection.North:
                    /* 
                     * Falls der moveSpeed > 1 ist, muss überprüft werden, ob der Hamster durch
                     * eine Wand laufen würde
                     */
                    if (this.moveSpeed > 1)
                    {
                        for (int i = 1; i <= this.moveSpeed; i++)
                        {
                            if (Territory.GetInstance().GetTileAt(this.column, this.row + i).type == Tile.TileType.Wall)
                            {
                                if (this.isUsingEndurance && this.endurancePointsFull >= 1)
                                {
                                    this.row += i - 1;
                                    this.endurancePointsFull -= 1;
                                }
                                else if (this.isUsingEndurance && this.endurancePointsFull == 0)
                                    Debug.LogError(this.hamsterName + noEnduranceFString);

                                if (!this.isUsingEndurance)
                                {
                                    this.row += i - 1;
                                }
                                return;
                            }
                        }
                    }
                    if (this.isUsingEndurance && this.endurancePointsFull >= 1)
                    {
                        this.row += this.moveSpeed;
                        this.endurancePointsFull -= 1;
                    }
                    else if (this.isUsingEndurance && this.endurancePointsFull == 0)
                        Debug.LogError(this.hamsterName + noEnduranceFString);
                    else if (!this.isUsingEndurance)
                        this.row += this.moveSpeed;
                    break;
                case LookingDirection.West:
                    /* 
                     * Falls der moveSpeed > 1 ist, muss überprüft werden, ob der Hamster durch
                     * eine Wand laufen würde
                     */
                    if (this.moveSpeed > 1)
                    {
                        for (int i = 1; i <= this.moveSpeed; i++)
                        {
                            if (Territory.GetInstance().GetTileAt(this.column - i, this.row).type == Tile.TileType.Wall)
                            {
                                if (this.isUsingEndurance && this.endurancePointsFull >= 1)
                                {
                                    this.column -= i - 1;
                                    this.endurancePointsFull -= 1;
                                }
                                else if (this.isUsingEndurance && this.endurancePointsFull == 0)
                                    Debug.LogError(this.hamsterName + noEnduranceFString);

                                if (!this.isUsingEndurance)
                                {
                                    this.column -= i - 1;
                                }
                                return;
                            }
                        }
                    }
                    if (this.isUsingEndurance && this.endurancePointsFull >= 1)
                    {
                        this.column -= this.moveSpeed;
                        this.endurancePointsFull -= 1;
                    }
                    else if (this.isUsingEndurance && this.endurancePointsFull == 0)
                        Debug.LogError(this.hamsterName + noEnduranceFString);
                    else if (!this.isUsingEndurance)
                        this.column -= this.moveSpeed;
                    break;
                case LookingDirection.South:
                    /* 
                     * Falls der moveSpeed > 1 ist, muss überprüft werden, ob der Hamster durch
                     * eine Wand laufen würde
                     */
                    if (this.moveSpeed > 1)
                    {
                        for (int i = 1; i <= this.moveSpeed; i++)
                        {
                            if (Territory.GetInstance().GetTileAt(this.column, this.row - i).type == Tile.TileType.Wall)
                            {
                                if (this.isUsingEndurance && this.endurancePointsFull >= 1)
                                {
                                    this.row -= i - 1;
                                    this.endurancePointsFull -= 1;
                                }
                                else if (this.isUsingEndurance && this.endurancePointsFull == 0)
                                    Debug.LogError(this.hamsterName + noEnduranceFString);

                                if (!this.isUsingEndurance)
                                {
                                    this.row -= i - 1;
                                }
                                return;
                            }
                        }
                    }
                    if (this.isUsingEndurance && this.endurancePointsFull >= 1)
                    {
                        this.row -= this.moveSpeed;
                        this.endurancePointsFull -= 1;
                    }
                    else if (this.isUsingEndurance && this.endurancePointsFull == 0)
                        Debug.LogError(this.hamsterName + noEnduranceFString);
                    else if (!this.isUsingEndurance)
                        this.row -= this.moveSpeed;
                    break;
                default:
                    Debug.LogError("No valid direction!\n Given direction " + this.direction);
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    break;
            }
            if (!this.isNPC)
                Debug.Log(this.hamsterName + movingFString);
            this.canMove = true;
            Territory.GetInstance().UpdateHamsterPosition(this);


            if (this.isInInventory)
            {
                DisplayInventory();
            }
            if (this.isTalking)
            {
                SetWindows(generalUI: true, dialogueUI: false);
                this.isTalking = false;
            }
            if (this.isTrading)
            {
                Trade();
            }

            Territory.GetInstance().UpdateHamsterProperties(this, updateEnduranceUI: true);
        }
        else
        {
            if (this.playerControl || this.isNPC) return;
            Debug.LogError(this.hamsterName + frontNotClearFString);
        }
    }

    /// <summary>
    /// Der hamster frisst eine bestimmte Anzahl <paramref name="amount"/> an Körner direkt vom Boden.
    /// </summary>
    /// <param name="amount"></param>
    public void EatGrainsFromGround(int amount = 1)
    {
        string noGrains = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "NO_GRAINS").Result;

        if (this.IsOnGrain())
        {
            // Friss korn
        }
        else
        {
            Debug.LogError(this.hamsterName + noGrains + "\nPosition (" + this.column + ", " + this.row + ")");
        }
    }

    /// <summary>
    /// Der hamster frisst eine bestimmte Anzahl <paramref name="amount"/> an Körner direkt vom Inventar.
    /// </summary>
    /// <param name="amount"></param>
    public void EatGrainsFromInventory(int amount = 1)
    {
        string noGrains = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DebugMsg", "NOT_OWNING_GRAINS").Result;

        if (this.grainCount > 0)
        {
            // Friss korn
        }
        else
        {
            Debug.LogError(this.hamsterName + noGrains);
        }
    }

    /// <summary>
    /// Ändere den namen des Hamsters
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        this.hamsterName = name;
        // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    /// <summary>
    /// Ändere die aktuelle Position des Hamsters, <paramref name="row"/> = y, <paramref name="column"/> = x
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    public void SetPosition(int column, int row)
    {
        this.row = row;
        this.column = column;
        // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    /// <summary>
    /// Lege die Anzahl der Körner fest, die der Hamster besitzt.
    /// </summary>
    /// <param name="grainCount"></param>
    public void SetGrainCount(int grainCount)
    {
        this.grainCount = grainCount;
        // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    /// <summary>
    /// Verändere die <paramref name="column"/> = x Position des Hamsters.
    /// </summary>
    /// <param name="column"></param>
    public void SetColumn(int column)
    {
        this.column = column;
        // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    /// <summary>
    /// Verändere die <paramref name="column"/> = x Position des Hamsters.
    /// </summary>
    /// <param name="column"></param>
    public void SetX(int column)
    {
        this.column = column;
        // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    /// <summary>
    /// Verändere die <paramref name="row"/> = x Position des Hamsters.
    /// </summary>
    /// <param name="row"></param>
    public void SetRow(int row)
    {
        this.row = row;
        // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    /// <summary>
    /// Verändere die <paramref name="row"/> = x Position des Hamsters.
    /// </summary>
    /// <param name="row"></param>
    public void SetY(int row)
    {
        this.row = row;
        // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    /// <summary>
    /// Aktiviere / Deaktiviere die Spielerkontrolle des Hamsters mit <paramref name="playerControl"/>.
    /// Falls die Kamera diesen Hamster folgen soll, setze <paramref name="cameraFollow"/> auf true.
    /// </summary>
    /// <param name="playerControl"></param>
    /// <param name="cameraFollow"></param>
    public void SetPlayerControls(bool playerControl, bool cameraFollow = false)
    {
        this.playerControl = playerControl;
        this.snapCamera = cameraFollow;

        // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    /// <summary>
    /// <para>Verändere die Richtung, in die der Hamster schaut.</para>
    /// <para>Westen = Hamster.LookingDirection.West</para>
    /// <para>Süden = Hamster.LookingDirection.South</para>
    /// <para>Osten = Hamster.LookingDirection.East</para>
    /// <para>Norden = Hamster.LookingDirection.North</para>
    /// </summary>
    /// <param name="direction"></param>
    public void SetLookingDirection(LookingDirection direction)
    {
        this.direction = direction;
        // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    /// <summary>
    /// <para>Verändere die Farbe des Hamsters</para>
    /// <para>Orange = Hamster.HamsterColor.Orange</para>
    /// <para>Schwarz = Hamster.HamsterColor.Black</para>
    /// <para>Blau = Hamster.HamsterColor.Blue</para>
    /// <para>Weiß = Hamster.HamsterColor.White</para>
    /// <para>Grau = Hamster.HamsterColor.Grey</para>
    /// <para>Lila = Hamster.HamsterColor.Purple</para>
    /// <para>Pink = Hamster.HamsterColor.Pink</para>
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(HamsterColor color)
    {
        this.hamsterColor = color;
        // Nach jeder Änderung am Hamster, muss das Territory den Hamster aktualisieren.
        Territory.GetInstance().UpdateHamsterProperties(this);
    }

    /* 
     * For one task
     * 
     */
    public void Hit()
    {
        int damage = 0;
        foreach (ItemSlot slot in inventory)
        {
            if (slot.item.AttackPower > 0)
            {
                damage += slot.item.AttackPower;
            }
        }

        if (!FrontIsClear() && !FrontIsClearNoHamster())
        {
            Hamster ham = null;
            switch (this.direction)
            {
                case LookingDirection.North:
                    ham = Territory.GetInstance().GetHamsterAt(this.column, this.row + 1);
                    ham.Damage(damage);
                    return;
                case LookingDirection.West:
                    ham = Territory.GetInstance().GetHamsterAt(this.column - 1, this.row);
                    ham.Damage(damage);
                    return;
                case LookingDirection.South:
                    ham = Territory.GetInstance().GetHamsterAt(this.column, this.row - 1);
                    ham.Damage(damage);
                    return;
                case LookingDirection.East:
                    ham = Territory.GetInstance().GetHamsterAt(this.column + 1, this.row);
                    ham.Damage(damage);
                    return;
            }
        }
    }
    

    public void Say(string sentence, float displayTime = 2f)
    {
        Territory.GetInstance().UpdateHamsterProperties(this, showSpeechBubble: true, speechText: sentence, speechTimer: displayTime);

        Debug.Log(this.Name + ": " + sentence);
    }

    /// <summary>
    /// Überprüfe ob der Weg vor dem Hamster frei ist.
    /// </summary>
    /// <returns></returns>
    public bool FrontIsClear()
    {
        switch (this.direction)
        {
            case LookingDirection.East:
                if(Territory.GetInstance().GetTileAt(this.column + 1, this.row).type != Tile.TileType.Wall &&
                   Territory.GetInstance().GetTileAt(this.column + 1, this.row).type != Tile.TileType.Water &&
                   FrontIsClearNoHamster() &&
                   this.currentLevel == Territory.GetInstance().GetTileAt(this.column + 1, this.row).tileLevel)
                {
                    // Debug.Log(this.name + ": FrontIsClear(): true");
                    return true;
                }
                // Debug.Log(this.name + ": FrontIsClear(): false");
                return false;
            case LookingDirection.North:
                if (Territory.GetInstance().GetTileAt(this.column, this.row + 1).type != Tile.TileType.Wall &&
                    Territory.GetInstance().GetTileAt(this.column, this.row + 1).type != Tile.TileType.Water &&
                    FrontIsClearNoHamster() &&
                    this.currentLevel == Territory.GetInstance().GetTileAt(this.column, this.row + 1).tileLevel)
                {
                    // Debug.Log(this.name + ": FrontIsClear(): true");
                    return true;
                }
                // Debug.Log(this.name + ": FrontIsClear(): false");
                return false;
            case LookingDirection.West:
                if (Territory.GetInstance().GetTileAt(this.column - 1, this.row).type != Tile.TileType.Wall &&
                    Territory.GetInstance().GetTileAt(this.column - 1, this.row).type != Tile.TileType.Water &&
                    FrontIsClearNoHamster() &&
                    this.currentLevel == Territory.GetInstance().GetTileAt(this.column - 1, this.row).tileLevel)
                {
                    // Debug.Log(this.name + ": FrontIsClear(): true");
                    return true;
                }
                // Debug.Log(this.name + ": FrontIsClear(): false");
                return false;
            case LookingDirection.South:
                if (Territory.GetInstance().GetTileAt(this.column, this.row - 1).type != Tile.TileType.Wall &&
                    Territory.GetInstance().GetTileAt(this.column, this.row - 1).type != Tile.TileType.Water &&
                    FrontIsClearNoHamster() &&
                    this.currentLevel == Territory.GetInstance().GetTileAt(this.column, this.row - 1).tileLevel)
                {
                    // Debug.Log(this.name + ": FrontIsClear(): true");
                    return true;
                }
                // Debug.Log(this.name + ": FrontIsClear(): false");
                return false;
            default:
                return false;
        }
    }

    private bool FrontIsClearNoHamster()
    {
        List<Hamster> hamsters = Territory.activHamsters;

        foreach(Hamster hamster in hamsters)
        {
            switch (this.direction)
            {
                case LookingDirection.East:
                    if (Vector2.Equals(hamster.GetHamsterPosition(), new Vector2(this.column + 1, this.row)))
                    {
                        return false;
                    }
                    break;
                case LookingDirection.North:
                    if (Vector2.Equals(hamster.GetHamsterPosition(), new Vector2(this.column, this.row + 1)))
                    {
                        return false;
                    }
                    break;
                case LookingDirection.West:
                    if (Vector2.Equals(hamster.GetHamsterPosition(), new Vector2(this.column - 1, this.row)))
                    {
                        return false;
                    }
                    break;
                case LookingDirection.South:
                    if (Vector2.Equals(hamster.GetHamsterPosition(), new Vector2(this.column, this.row - 1)))
                    {
                        return false;
                    }
                    break;
                default:
                    break;
            }
        }
        return true;
    }

    public enum HamsterColor
    {
        Orange,
        Black,
        Blue,
        White,
        Grey,
        Purple,
        Pink,
        FullBrown,
        FullWhite,
        FullGrey,
        FullBlue,
        Evil
    };

    public enum LookingDirection
    {
        North,
        East,
        South,
        West
    };

    public enum MovementPattern
    {
        Random,
        LeftRight,
        UpDown,
        Rotate,
        None
    };
}
