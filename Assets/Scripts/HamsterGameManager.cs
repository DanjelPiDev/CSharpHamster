using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class HamsterGameManager : MonoBehaviour
{
#if UNITY_EDITOR
    [Help("The lower this value, the faster the simulation.", UnityEditor.MessageType.Info)]
#endif
    [Header("Global Gamespeed"), Range(0f, 2f)]
    [Tooltip("The lower this value, the faster the simulation.")]public float gameSpeed;

    [Header("Hamster Gamemanager references")]
    [Space(50)]
    public GameObject hamsterInstance;
    public List<Sprite> hamsterSprites = new List<Sprite>();
    public List<TileBase> grainSpritesTileBase = new List<TileBase>();
    public GameObject heartPrefab;
    public GameObject enduranceHeartPrefab;
    public GameObject miniHeartPrefab;
    public GameObject miniEndurancePrefab;
    public List<Sprite> healthSprite = new List<Sprite>();
    public List<Sprite> enduranceSprite = new List<Sprite>();

    public TextMeshProUGUI grainAmountUI;
    public TextMeshProUGUI hamsterAmountUI;

    public Transform questContent;
    public GameObject questContainer;

    [Header("UI")]
    public GameObject generalUI;
    public GameObject dialogueCanvas;
    public GameObject tradeCanvas;
    public GameObject inventoryCanvas;

    [Header("Inventory")]
    public GameObject itemPrefab;
    public Transform itemContent;

    public Image hamsterImage;
    public TextMeshProUGUI hamsterName;
    public TextMeshProUGUI hamsterGrains;

    public Transform hamsterHealthPoints;
    public Transform hamsterEndurancePoints;

    public Transform Equipment;

    [Header("Trading (Hamster 1)")]
    public Transform tradeItemContentHamster1;
    public Image tradeHamster1Image;
    public TextMeshProUGUI tradeHamster1Name;
    public TextMeshProUGUI tradeHamster1Grains;

    [Header("Trading (Hamster 2)")]
    public Transform tradeItemContentHamster2;
    public Image tradeHamster2Image;
    public TextMeshProUGUI tradeHamster2Name;
    public TextMeshProUGUI tradeHamster2Grains;

    public static Hamster hamster1;
    public static Hamster hamster2;

    public static bool isTrading = false;
    public static bool isTalking = false;

    public static float hamsterGameSpeed;

    private void Awake()
    {
        hamsterGameSpeed = gameSpeed;
    }

    private void Start()
    {
        List<Quest> quests = this.GetComponent<QuestHolder>().quests;

        foreach (Quest quest in quests)
        {
            quest.questDone = false;
            quest.questFailed = false;
            GameObject n_quest = Instantiate(questContainer, questContent);
            n_quest.GetComponent<TextMeshProUGUI>().text = quest.stageInfo.stageDescription;
        }

        SetCanvasVisibility(generalUI, true);
        SetCanvasVisibility(dialogueCanvas, false);
        SetCanvasVisibility(tradeCanvas, false);
        SetCanvasVisibility(inventoryCanvas, false);

        base.StartCoroutine(EnableQuestConditions());
    }

    private IEnumerator EnableQuestConditions()
    {
        yield return new WaitForSeconds(1f);
        List<Quest> quests = this.GetComponent<QuestHolder>().quests;

        foreach (Quest quest in quests)
        {
            if (quest.stageInfo.condition.displayEndurance)
            {
                foreach (Hamster hamster in Territory.activHamsters)
                {
                    hamster.SetEndurancePoints(quest.stageInfo.condition.maxEndurancePoints);
                    hamster.HealEndurance(quest.stageInfo.condition.maxEndurancePoints);
                    hamster.SetEnduranceConsumption(true);
                    hamster.DisplayEndurance(true);
                }
            }
        }
    }

    public void RefreshTradeWindow()
    {
        /* 
         * Remove all items 
         */
        for (int i = 0; i < tradeItemContentHamster1.childCount; i++)
        {
            Destroy(tradeItemContentHamster1.GetChild(i).gameObject);
        }

        for (int i = 0; i < tradeItemContentHamster2.childCount; i++)
        {
            Destroy(tradeItemContentHamster2.GetChild(i).gameObject);
        }


        /* 
         * Add all items into the inventory of "Hamster1" 
         */
        for (int i = 0; i < hamster1.Inventory.Count; i++)
        {
            GameObject itemSlot = Instantiate(this.itemPrefab, this.tradeItemContentHamster1);
            itemSlot.GetComponent<ItemHolder>().item = hamster1.Inventory[i].item;
            itemSlot.GetComponent<ItemHolder>().quantity = hamster1.Inventory[i].quantity;
            itemSlot.GetComponent<ItemHolder>().itemName.SetText(hamster1.Inventory[i].item.Name);
            itemSlot.GetComponent<ItemHolder>().itemBuyPrice.SetText(hamster1.Inventory[i].item.BuyPrice.ToString());
            itemSlot.GetComponent<ItemHolder>().itemSellPrice.SetText(hamster1.Inventory[i].item.SellPrice.ToString());
            itemSlot.GetComponent<ItemHolder>().itemQuantity.SetText(itemSlot.GetComponent<ItemHolder>().quantity + "/" + hamster1.Inventory[i].item.StackAmount.ToString());
            itemSlot.GetComponent<ItemHolder>().itemImage.sprite = hamster1.Inventory[i].item.ItemImage;
            itemSlot.GetComponent<ItemHolder>().item.IsEquipped = hamster1.Inventory[i].item.IsEquipped;
        }

        /* 
         * Add all items into the inventory of "Hamster2"  
         */
        for (int i = 0; i < hamster2.Inventory.Count; i++)
        {
            GameObject itemSlot = Instantiate(this.itemPrefab, this.tradeItemContentHamster2);
            itemSlot.GetComponent<ItemHolder>().item = hamster2.Inventory[i].item;
            itemSlot.GetComponent<ItemHolder>().quantity = hamster2.Inventory[i].quantity;
            itemSlot.GetComponent<ItemHolder>().itemName.SetText(hamster2.Inventory[i].item.Name);
            itemSlot.GetComponent<ItemHolder>().itemBuyPrice.SetText(hamster2.Inventory[i].item.BuyPrice.ToString());
            itemSlot.GetComponent<ItemHolder>().itemSellPrice.SetText(hamster2.Inventory[i].item.SellPrice.ToString());
            itemSlot.GetComponent<ItemHolder>().itemQuantity.SetText(itemSlot.GetComponent<ItemHolder>().quantity + "/" + hamster2.Inventory[i].item.StackAmount.ToString());
            itemSlot.GetComponent<ItemHolder>().itemImage.sprite = hamster2.Inventory[i].item.ItemImage;
            itemSlot.GetComponent<ItemHolder>().item.IsEquipped = hamster2.Inventory[i].item.IsEquipped;
        }
    }

    public void RefreshInventoryWindow()
    {
        Hamster hamster = null;

        /* 
         * Remove all items  
         */
        for (int i = 0; i < tradeItemContentHamster1.childCount; i++)
        {
            Destroy(itemContent.GetChild(i).gameObject);
        }

        /* Finde den Hamster der im inventar ist */
        foreach(Hamster ham in Territory.activHamsters)
        {
            if (ham.IsInInventory)
            {
                hamster = ham;
                break;
            }
        }

        for (int i = 0; i < itemContent.childCount; i++)
        {
            Destroy (itemContent.GetChild(i).gameObject);
        }

        /* Füge alle items dem Inventar hinzu */
        for (int i = 0; i < hamster.Inventory.Count; i++)
        {
            GameObject itemSlot = Instantiate(this.itemPrefab, this.itemContent);
            itemSlot.GetComponent<ItemHolder>().item = hamster.Inventory[i].item;
            itemSlot.GetComponent<ItemHolder>().quantity = hamster.Inventory[i].quantity;
            itemSlot.GetComponent<ItemHolder>().itemName.SetText(hamster.Inventory[i].item.Name);
            itemSlot.GetComponent<ItemHolder>().itemBuyPrice.SetText(hamster.Inventory[i].item.BuyPrice.ToString());
            itemSlot.GetComponent<ItemHolder>().itemSellPrice.SetText(hamster.Inventory[i].item.SellPrice.ToString());
            itemSlot.GetComponent<ItemHolder>().itemQuantity.SetText(itemSlot.GetComponent<ItemHolder>().quantity + "/" + hamster.Inventory[i].item.StackAmount.ToString());
            itemSlot.GetComponent<ItemHolder>().itemImage.sprite = hamster.Inventory[i].item.ItemImage;
            itemSlot.GetComponent<ItemHolder>().item.IsEquipped = hamster.Inventory[i].item.IsEquipped;

            if (hamster.Inventory[i].item.IsEquipped)
            {
                itemSlot.GetComponent<ItemHolder>().gameObject.GetComponent<Image>().color = itemSlot.GetComponent<ItemHolder>().EquipColor;
            }
        }

        /* Aktualisiere die Herzen für den Hamster */
        for (int i = 0; i < hamsterHealthPoints.childCount; i++)
        {
            if (i < hamster.HealthPointsFull)
            {
                hamsterHealthPoints.GetChild(i).GetComponent<Image>().sprite = healthSprite[0];
            }
            else
            {
                hamsterHealthPoints.GetChild(i).GetComponent<Image>().sprite = healthSprite[1];
            }
        }

        /* Aktualisiere die Herzen der Ausdauer für den Hamster */
        for (int i = 0; i < hamsterEndurancePoints.childCount; i++)
        {
            if (i < hamster.EndurancePointsFull)
            {
                hamsterEndurancePoints.GetChild(i).GetComponent<Image>().sprite = enduranceSprite[0];
            }
            else
            {
                hamsterEndurancePoints.GetChild(i).GetComponent<Image>().sprite = enduranceSprite[1];
            }
        }
    }

    private void CheckQuestState()
    {
        List<Quest> quests = this.GetComponent<QuestHolder>().quests;

        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].questDone) // && !questImages[i].activeSelf
            {
                for (int j = 0; j < questContent.childCount; j++)
                {
                    if (string.Compare(questContent.GetChild(j).GetComponent<TextMeshProUGUI>().text, quests[i].stageInfo.stageDescription) == 0)
                    {
                        questContent.GetChild(j).GetChild(0).GetChild(0).gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public bool GetCanvasVisibility(Canvas canvas)
    {
        return canvas.GetComponent<CanvasGroup>().alpha == 1;
    }

    public bool GetCanvasVisibility(GameObject canvas)
    {
        return canvas.GetComponent<CanvasGroup>().alpha == 1;
    }

    /// <summary>
    /// Aktiviere/Deaktiviere einen <paramref name="canvas"/>
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="b"></param>
    public void SetCanvasVisibility(GameObject canvas, bool b)
    {
        canvas.GetComponent<CanvasGroup>().alpha = b ? 1 : 0;
        canvas.GetComponent<CanvasGroup>().interactable = b;
        canvas.GetComponent<CanvasGroup>().blocksRaycasts = b;
    }

    /// <summary>
    /// Aktiviere/Deaktiviere einen <paramref name="canvas"/>
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="b"></param>
    public void SetCanvasVisibility(Canvas canvas, bool b)
    {
        canvas.GetComponent<CanvasGroup>().alpha = b ? 1 : 0;
        canvas.GetComponent<CanvasGroup>().interactable = b;
        canvas.GetComponent<CanvasGroup>().blocksRaycasts = b;
    }

    private void Update()
    {
        CheckQuestState();
        hamsterGameSpeed = gameSpeed;
    }
}