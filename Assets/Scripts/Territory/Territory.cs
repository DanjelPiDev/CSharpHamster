/************************************************
 * Created by:  Danjel Galic
 * 
 * Modified by: -
 ************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

/************************************************
 * Handles everything on the territory
 ************************************************/
public class Territory : MonoBehaviour
{
    [Header("Hamsters")]
    [SerializeField] private List<Hamster> hamsters = new List<Hamster>();
    [Header("Walls")]
    [SerializeField] private GameObject wall3D;
    [SerializeField] private Transform wall3DTransform;
    [Header("Lights")]
    [SerializeField] private GameObject lampLight01;
    [SerializeField] private Transform lampLightsTransform;
    [SerializeField] private GameObject lampLightReplace;
    [Header("Items")]
    [SerializeField] private Transform itemsTransform;
    
    [SerializeField] private int grainCount = 0;
    [SerializeField] private GameObject cam;
    private float gameSpeed;

    

    public ItemCollection itemCollection;

    public static Transform tileCollection;
    private static Transform hamsterCollection;

    private const float TILESIZE = 2.56f;
    private const string GRASS_TILE = "Grass";
    private const string SAND_TILE = "Sand";
    private const string LAMP_TILE = "Lamp";
    private const string WALL_TILE = "mauer";
    private const string GRAIN1_TILE = "Grain_1";
    private const string GRAIN2_TILE = "Grain_2";
    private const string GRAIN3_TILE = "Grain_3";
    private const string GRAIN4_TILE = "Grain_4";
    private const string GRAIN5_TILE = "Grain_5";
    private const string GRAIN6_TILE = "Grain_6";
    private const string GRAIN7_TILE = "Grain_7";
    private const string GRAIN8_TILE = "Grain_8";
    private const string GRAIN9_TILE = "Grain_9";


    public static Territory territory = new Territory();
    public static bool changeGrainCount = false;
    public static bool addGrainCount = false;
    public static bool canUpdateHamster = false;
    public static int globalGrainCount = 0;
    public static List<Hamster> activHamsters = new List<Hamster>();
    public static bool playerCanMove = false;
    public static bool player2CanMove = false;

    private static Tilemap tilemap;
    private static Tilemap lampTilemap;
    private static Tilemap itemsTilemap;
    private static HamsterGameManager hamsterGameManager;
    private static int row = 10;
    private static int column = 10;
    private static Transform g_itemsTransform;
    private static WaitForSeconds wait;

    private void Awake()
    {
        territory = new Territory();
        tileCollection = GameObject.Find("TileCollection").transform;
        hamsterCollection = GameObject.Find("HamsterCollection").transform;
        hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();
        tilemap = GameObject.FindGameObjectWithTag("TileMap").transform.GetChild(0).GetComponent<Tilemap>();
        lampTilemap = GameObject.FindGameObjectWithTag("TileMap").transform.GetChild(1).GetComponent<Tilemap>();
        itemsTilemap = GameObject.FindGameObjectWithTag("TileMap").transform.GetChild(2).GetComponent<Tilemap>();
        g_itemsTransform = itemsTransform;

        cam = GameObject.Find("Main Camera 2");

        BoundsInt bound = tilemap.cellBounds;

        column = (Mathf.Abs(bound.xMin) + Mathf.Abs(bound.xMax));
        row = (Mathf.Abs(bound.yMin) + Mathf.Abs(bound.yMax));
        for (int x = bound.xMin; x < bound.xMax; x++)
        {
            for (int y = bound.yMin; y < bound.yMax; y++)
            {
                Vector3Int currentTile = new Vector3Int(x, y, 0);
                if (tilemap.GetTile(currentTile) != null)
                {
                    GameObject go = new GameObject(tilemap.GetTile(currentTile).name + " (" + x + ", " + y + ")");
                    go.AddComponent<TileHolder>();
                    Tile tile = ScriptableObject.CreateInstance<Tile>();
                    tile.Column = x;
                    tile.Row = y;
                    tile.type = Tile.TileType.Floor;
                    go.GetComponent<TileHolder>().tile = tile;
                    // new Tile(x, y, Tile.TileType.Floor);


                    if (string.Compare(tilemap.GetTile(currentTile).name, GRASS_TILE) == 0 ||
                        string.Compare(tilemap.GetTile(currentTile).name, SAND_TILE) == 0)
                    {
                        go.GetComponent<TileHolder>().tile.type = Tile.TileType.Floor;
                    }
                    else if (tilemap.GetTile(currentTile).name.Contains("Grain"))
                    {
                        go.GetComponent<TileHolder>().tile.type = Tile.TileType.Grain;
                        go.GetComponent<TileHolder>().tile.hasGrain = true;
                        if (tilemap.GetTile(currentTile).name.Contains("1"))
                        {
                            go.GetComponent<TileHolder>().tile.grainCount = 1;
                        }
                        else if (tilemap.GetTile(currentTile).name.Contains("2"))
                        {
                            go.GetComponent<TileHolder>().tile.grainCount = 2;
                        }
                        else if (tilemap.GetTile(currentTile).name.Contains("3"))
                        {
                            go.GetComponent<TileHolder>().tile.grainCount = 3;
                        }
                        else if (tilemap.GetTile(currentTile).name.Contains("4"))
                        {
                            go.GetComponent<TileHolder>().tile.grainCount = 4;
                        }
                        else if (tilemap.GetTile(currentTile).name.Contains("5"))
                        {
                            go.GetComponent<TileHolder>().tile.grainCount = 5;
                        }
                        else if (tilemap.GetTile(currentTile).name.Contains("6"))
                        {
                            go.GetComponent<TileHolder>().tile.grainCount = 6;
                        }
                        else if (tilemap.GetTile(currentTile).name.Contains("7"))
                        {
                            go.GetComponent<TileHolder>().tile.grainCount = 7;
                        }
                        else if (tilemap.GetTile(currentTile).name.Contains("8"))
                        {
                            go.GetComponent<TileHolder>().tile.grainCount = 8;
                        }
                        else if (tilemap.GetTile(currentTile).name.Contains("9"))
                        {
                            go.GetComponent<TileHolder>().tile.grainCount = 9;
                        }
                    }
                    else if (tilemap.GetTile(currentTile).name.Contains("Water"))
                    {
                        go.GetComponent<TileHolder>().tile.type = Tile.TileType.Water;
                    }
                    else
                    {
                        go.GetComponent<TileHolder>().tile.type = Tile.TileType.Wall;
                        /* Set 3D Objects as walls, to cast shadows and move the hamster in z axis further to the camera */

                        GameObject wall = Instantiate(wall3D);
                        wall.transform.parent = wall3DTransform;
                        wall.transform.position = new Vector3((x * TILESIZE) + (TILESIZE / 2), (y * TILESIZE) + (TILESIZE / 2), 0);
                    }

                    if (lampTilemap.GetTile(currentTile) != null)
                    {
                        if (lampTilemap.GetTile(currentTile).name.Contains(LAMP_TILE))
                        {
                            GameObject light = Instantiate(lampLight01);
                            light.transform.parent = lampLightsTransform;
                            if (tilemap.GetTile(currentTile).name.Contains(WALL_TILE))
                            {
                                light.transform.position = new Vector3((x * TILESIZE) + (TILESIZE / 2), (y * TILESIZE) + (TILESIZE / 2), -8.5f);

                                /* Create the light on top of the wall */
                                GameObject lightReplace = Instantiate(lampLightReplace);
                                lightReplace.transform.parent = lampLightsTransform;
                                lightReplace.transform.position = new Vector3((x * TILESIZE) + (TILESIZE / 2), (y * TILESIZE) + (TILESIZE / 2), -3.15f);
                                lightReplace.transform.rotation = Quaternion.Euler(-19f, 0, 0);
                            }
                            else
                            {
                                light.transform.position = new Vector3((x * TILESIZE) + (TILESIZE / 2), (y * TILESIZE) + (TILESIZE / 2), -6f);
                            }
                        }
                    }

                    if (itemsTilemap.GetTile(currentTile) != null)
                    {
                        GameObject itemSprite = Instantiate(lampLightReplace);
                        itemSprite.transform.parent = itemsTransform;
                        itemSprite.name = itemsTilemap.GetTile(currentTile).name + " (" + x + ", " + y + ")";
                        go.AddComponent<ItemHolder>();
                        // Add item...
                        foreach (Item item in itemCollection.items)
                        {
                            if (itemsTilemap.GetTile(currentTile).name.Contains(item.Name))
                            {
                                go.GetComponent<ItemHolder>().item = item;
                            }
                        }

                        itemSprite.GetComponent<SpriteRenderer>().sprite = itemsTilemap.GetSprite(currentTile);
                        itemSprite.transform.position = new Vector3((x * TILESIZE) + (TILESIZE / 2), (y * TILESIZE) + (TILESIZE / 2), -0.25f);
                        itemSprite.transform.rotation = Quaternion.Euler(-19f, 0, 0);
                    }
                        

                    go.transform.parent = tileCollection; 
                }
            }
        }
        UpdateGrainCount();
        globalGrainCount = grainCount;

        playerCanMove = true;
        player2CanMove = true;
        gameSpeed = HamsterGameManager.hamsterGameSpeed;
        wait = new WaitForSeconds(gameSpeed);
        base.StartCoroutine(PlayerControlMovementTimer());
        base.StartCoroutine(Player2ControlMovementTimer());
        base.StartCoroutine(RandomHamsterMovements());
    }

    private void CheckLight()
    {
        if (DayNightCycle.GlobalIsNight)
        {
            for (int i = 0; i < lampLightsTransform.childCount; i++)
            {
                if (!lampLightsTransform.GetChild(i).gameObject.activeSelf && lampLightsTransform.GetChild(i).GetComponent<Light>())
                {
                    lampLightsTransform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < lampLightsTransform.childCount; i++)
            {
                if (lampLightsTransform.GetChild(i).gameObject.activeSelf && lampLightsTransform.GetChild(i).GetComponent<Light>())
                {
                    lampLightsTransform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }

    public Vector2 GetTerritorySize()
    {
        return new Vector2(column, row);
    }

    /// <summary>
    /// Refresh a specific tile in the world (E.g., after picking up a grain)
    /// <para>x = <paramref name="column"/></para>
    /// <para>y = <paramref name="row"/></para>
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    public void UpdateTile(int column, int row, bool addingItem = false, Hamster hamster = null)
    {
        /* 
         * Loop through all available tiles in the world
         * Not really efficient.
         */
        for(int i = 0; i < tileCollection.childCount; i++)
        {
            if(tileCollection.GetChild(i).name.Contains(" (" + column + ", " + row + ")"))
            {
                /* 
                 * If a hamster is picking up an item, and adds it afterwards to the inventory,
                 * or if a hamster picks up a grain.
                 */
                if (!addingItem)
                {
                    int tileGrainCount;
                    if (!addGrainCount)
                    {
                        tileCollection.GetChild(i).GetComponent<TileHolder>().tile.grainCount -= 1;
                        tileGrainCount = tileCollection.GetChild(i).GetComponent<TileHolder>().tile.grainCount;
                        List<Quest> quests = hamsterGameManager.GetComponent<QuestCollection>().quests;

                        foreach (Quest quest in quests)
                        {
                            foreach (StageInfo stageInfo in quest.stageInfos)
                            {
                                if (stageInfo.isActive)
                                {
                                    stageInfo.condition.OnRemoveGrain();
                                }
                            }
                        }
                    }
                    else
                    {
                        tileCollection.GetChild(i).GetComponent<TileHolder>().tile.grainCount += 1;
                        tileGrainCount = tileCollection.GetChild(i).GetComponent<TileHolder>().tile.grainCount;
                        List<Quest> quests = hamsterGameManager.GetComponent<QuestCollection>().quests;

                        foreach (Quest quest in quests)
                        {
                            if (quest.questStarted)
                            {
                                foreach (StageInfo stageInfo in quest.stageInfos)
                                {
                                    if (stageInfo.isActive)
                                    {
                                        stageInfo.condition.OnAddGrain();
                                    }
                                }
                            }
                        }
                    }

                    if (hamster == null) return;

                    /* 
                     * Here you can add more floor textures.
                     * Currently available: Sand, Grass.
                     * Writing Indizies like:
                     * 0 (+10) = No more grains
                     * 1 (+10) = 1 Grain
                     * 2 (+10) = 2 Grains
                     * etc.
                     * 
                     * --> Next floor would be +20, than +30...
                     */
                    // ================================ SAND TILES ==============%00%================
                    if (tilemap.GetTile(new Vector3Int(hamster.Column, hamster.Row, 0)).name.Contains(SAND_TILE))
                    {
                        switch (tileGrainCount)
                        {
                            case 0:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[0]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = false;
                                break;
                            case 1:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[1]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = true;
                                break;
                            case 2:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[2]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = true;
                                break;
                            case 3:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[3]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = true;
                                break;
                            case 4:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[4]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = true;
                                break;
                            case 5:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[5]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = true;
                                break;
                            case 6:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[6]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = true;
                                break;
                            case 7:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[7]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = true;
                                break;
                            case 8:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[8]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = true;
                                break;
                            case 9:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[9]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = true;
                                break;
                            default:
                                break;
                        }
                    }
                    // ================================ GRASS TILES =============%10%================
                    else if (tilemap.GetTile(new Vector3Int(hamster.Column, hamster.Row, 0)).name.Contains(GRASS_TILE))
                    {
                        switch (tileGrainCount)
                        {
                            case 0:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[10]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = false;
                                break;
                            case 1:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[11]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = true;
                                break;
                            case 2:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[12]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = true;
                                break;
                            case 3:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[13]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = true;
                                break;
                            case 4:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[14]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = true;
                                break;
                            case 5:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[15]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = true;
                                break;
                            case 6:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[16]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = true;
                                break;
                            case 7:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[17]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = true;
                                break;
                            case 8:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[18]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = true;
                                break;
                            case 9:
                                tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[19]);
                                tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = true;
                                break;
                            default:
                                break;
                        }
                    }
                    // ================================ ADDITIONAL TILES ===========%20%================
                    // ...
                    // ...
                }
                else
                {
                    tileCollection.GetChild(i).GetComponent<ItemHolder>().item = null;
                    Destroy(tileCollection.GetChild(i).GetComponent<ItemHolder>());
                    RemoveItemFromTile(column, row);
                    SetTileAt(column, row, itemsTilemap, null);
                    //tilemap.SetTile(new Vector3Int(column, row, 0), null);
                }
            }
        }
    }


    public static Territory GetInstance()
    {
        return territory;
    }

    public void UpdateUI()
    {
        if (hamsterGameManager == null) return;
        hamsterGameManager.grainAmountUI.SetText(grainCount.ToString());
        hamsterGameManager.hamsterAmountUI.SetText(GetHamsterCount().ToString());
    }

    public void DisplayTradeWindow(Hamster hamster1, Hamster hamster2)
    {
        /* 
         * Activate/Deactivate the trading canvas and disable all the other UIs 
         */
        hamsterGameManager.SetCanvasVisibility(hamsterGameManager.tradeCanvas, true);

        hamsterGameManager.SetCanvasVisibility(hamsterGameManager.inventoryCanvas, false);
        hamsterGameManager.SetCanvasVisibility(hamsterGameManager.dialogueCanvas, false);
        hamsterGameManager.SetCanvasVisibility(hamsterGameManager.generalUI, false);
    }

    public void DisplayDialogueWindow(Hamster hamster1, Hamster hamster2)
    {

    }

    /// <summary>
    /// Get the content of a specific tile at the Position(<paramref name="column"/>, <paramref name="row"/>).
    /// If tile is not set (Not in the world), this method will return null.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public Tile GetTileAt(int column, int row)
    {
        
        for (int i = 0; i < tileCollection.childCount; i++)
        {
            if(tileCollection.GetChild(i).name.Contains(" (" + column + ", " + row + ")"))
            {
                return tileCollection.GetChild(i).GetComponent<TileHolder>().tile;
            }
        }
        return null;
    }

    /// <summary>
    /// Change a <paramref name="tile"/> to an other for a specific <paramref name="tilemap"/>,
    /// Position(<paramref name="column"/>, <paramref name="row"/>).
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    /// <param name="tilemap"></param>
    /// <param name="tile"></param>
    public void SetTileAt(int column, int row, Tilemap tilemap, TileBase tile)
    {
        tilemap.SetTile(new Vector3Int(column, row, 0), tile);
    }

    /// <summary>
    /// Remove an item from a tile at the Position(<paramref name="column"/>, <paramref name="row"/>),
    /// and remove the GameObject.
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    public void RemoveItemFromTile(int column, int row)
    {
        for(int i = 0; i < g_itemsTransform.childCount; i++)
        {
            if (g_itemsTransform.GetChild(i).name.Contains(" (" + column + ", " + row + ")"))
            {
                Destroy(g_itemsTransform.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// Get an item at the Position(<paramref name="column"/>, <paramref name="row"/>)
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    public static Item GetItemAt(int column, int row)
    {
        for (int i = 0; i < tileCollection.childCount; i++)
        {
            if (tileCollection.GetChild(i).name.Contains(" (" + column + ", " + row + ")"))
            {
                if (!tileCollection.GetChild(i).GetComponent<ItemHolder>()) return null;

                return tileCollection.GetChild(i).GetComponent<ItemHolder>().item;
            }
        }
        return null;
    }

    /// <summary>
    /// Change the amount (<paramref name="grainCount"/>) of grains at the Position(<paramref name="column"/>, <paramref name="row"/>)
    /// </summary>
    /// <param name="grainCount"></param>
    /// <param name="column"></param>
    /// <param name="row"></param>
    public void SetGrainsAt(int grainCount, int column, int row)
    {
        for (int i = 0; i < tileCollection.childCount; i++)
        {
            if (tileCollection.GetChild(i).name.Contains(" (" + column + ", " + row + ")"))
            {
                switch (grainCount)
                {
                    case 0:
                        tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[0]);
                        tileCollection.GetChild(i).GetComponent<TileHolder>().tile.hasGrain = false;
                        break;
                    case 1:
                        tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[1]);
                        break;
                    case 2:
                        tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[2]);
                        break;
                    case 3:
                        tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[3]);
                        break;
                    case 4:
                        tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[4]);
                        break;
                    case 5:
                        tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[5]);
                        break;
                    case 6:
                        tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[6]);
                        break;
                    case 7:
                        tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[7]);
                        break;
                    case 8:
                        tilemap.SetTile(new Vector3Int(column, row, 0), hamsterGameManager.grainSpritesTileBase[8]);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Go through all tiles and refresh each one (Extra for grains)
    /// </summary>
    public void UpdateGrainCount()
    {
        for(int i = 0; i < tileCollection.childCount; i++)
        {
            if (tileCollection.GetChild(i).name.Contains(GRAIN1_TILE))
            {
                grainCount += 1;
            }
            else if (tileCollection.GetChild(i).name.Contains(GRAIN2_TILE))
            {
                grainCount += 2;
            }
            else if (tileCollection.GetChild(i).name.Contains(GRAIN3_TILE))
            {
                grainCount += 3;
            }
            else if (tileCollection.GetChild(i).name.Contains(GRAIN4_TILE))
            {
                grainCount += 4;
            }
            else if (tileCollection.GetChild(i).name.Contains(GRAIN5_TILE))
            {
                grainCount += 5;
            }
            else if (tileCollection.GetChild(i).name.Contains(GRAIN6_TILE))
            {
                grainCount += 6;
            }
            else if (tileCollection.GetChild(i).name.Contains(GRAIN7_TILE))
            {
                grainCount += 7;
            }
            else if (tileCollection.GetChild(i).name.Contains(GRAIN8_TILE))
            {
                grainCount += 8;
            }
            else if (tileCollection.GetChild(i).name.Contains(GRAIN9_TILE))
            {
                grainCount += 9;
            }
        }
        globalGrainCount = grainCount;
    }

    /// <summary>
    /// Get the amount of hamsters' available in the world (Aktiv/Inaktiv)
    /// </summary>
    /// <returns></returns>
    public int GetHamsterCount()
    {
        return hamsterCollection.childCount;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Hamster GetHamsterAt(int index)
    {
        return hamsterCollection.GetChild(index).GetChild(1).GetComponent<HamsterHolder>().hamster;
    }

    public List<Hamster> GetHamsters()
    {
        return this.hamsters;
    }

    /// <summary>
    /// Get a hamster at the specific Position(<paramref name="column"/>, <paramref name="row"/>).
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public Hamster GetHamsterAt(int column, int row)
    {
        Hamster hamster = null;

        for(int i = 0; i < hamsterCollection.childCount; i++)
        {
            Hamster tmpHamster = hamsterCollection.GetChild(i).GetChild(1).GetComponent<HamsterHolder>().hamster;
            if (tmpHamster.Row == row && tmpHamster.Column == column)
            {
                hamster = tmpHamster;
                break;
            }
        }
        return hamster;
    }

    /// <summary>
    /// Get a hamster with a specific <paramref name="id"/>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Hamster GetHamster(int id)
    {
        foreach(Hamster hamster in hamsters)
        {
            if (hamster.Id == id)
            {
                return hamster;
            }
        }
        return null;
    }

    /// <summary>
    /// Get the hamster gameObject by the hamsters' <paramref name="id"/>.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GameObject GetHamsterObject(int id)
    {
        foreach (Transform hamster in hamsterCollection)
        {
            if (hamster.GetChild(1).GetComponent<HamsterHolder>().hamster.Id == id)
            {
                return hamster.gameObject;
            }
        }
        return null;
    }

    /// <summary>
    /// Refresh the rotation (Direction in which the hamster is looking at), for a specific <paramref name="hamster"/>
    /// in a specific <paramref name="direction"/>
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="hamster"></param>
    public void UpdateRotation(Hamster.LookingDirection direction, Hamster hamster)
    {
        foreach (Transform transform in hamsterCollection)
        {
            transform.parent = hamsterCollection;
            Hamster ham = transform.GetChild(1).GetComponent<HamsterHolder>().hamster;

            if (ham.Id == hamster.Id)
            {
                ham.Direction = hamster.Direction;
            }
        }
    }

    /// <summary>
    /// Refresh the position of all hamsters' in the world. Also refresh the display of name, health and endurance.
    /// </summary>
    public void UpdateHamsterPosition()
    {
        if (hamsterCollection == null) return;

        foreach (Transform transform in hamsterCollection)
        {
            transform.parent = hamsterCollection;
            Hamster hamster = transform.GetChild(1).GetComponent<HamsterHolder>().hamster;

            hamster.DisplayHealth(hamster.IsDisplayingHealth);
            hamster.DisplayName(hamster.IsDisplayingName);
            hamster.DisplayEndurance(hamster.IsDisplayingEndurance);

            transform.position = new Vector3((hamster.Column * TILESIZE), (hamster.Row * TILESIZE), transform.position.z);

            switch (hamster.Direction)
            {
                case Hamster.LookingDirection.East:
                    // Orange
                    if (hamster.Color == Hamster.HamsterColor.Orange)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[0];
                    // Black
                    else if (hamster.Color == Hamster.HamsterColor.Black)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[4];
                    // Blue
                    else if (hamster.Color == Hamster.HamsterColor.Blue)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[8];
                    // White
                    else if (hamster.Color == Hamster.HamsterColor.White)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[12];
                    // Grey
                    else if (hamster.Color == Hamster.HamsterColor.Grey)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[16];
                    // Purple
                    else if (hamster.Color == Hamster.HamsterColor.Purple)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[20];
                    // Pink
                    else if (hamster.Color == Hamster.HamsterColor.Pink)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[24];
                    // Brown (Full)
                    else if (hamster.Color == Hamster.HamsterColor.FullBrown)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[28];
                    // Blue (Full)
                    else if (hamster.Color == Hamster.HamsterColor.FullBlue)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[32];
                    // Grey (Full)
                    else if (hamster.Color == Hamster.HamsterColor.FullGrey)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[36];
                    // White (Full)
                    else if (hamster.Color == Hamster.HamsterColor.FullWhite)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[40];
                    // Evil
                    else if (hamster.Color == Hamster.HamsterColor.Evil)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[44];
                    break;
                case Hamster.LookingDirection.North:
                    // Orange
                    if (hamster.Color == Hamster.HamsterColor.Orange)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[3];
                    // Black
                    else if (hamster.Color == Hamster.HamsterColor.Black)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[7];
                    // Blue
                    else if (hamster.Color == Hamster.HamsterColor.Blue)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[11];
                    // White
                    else if (hamster.Color == Hamster.HamsterColor.White)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[15];
                    // Grey
                    else if (hamster.Color == Hamster.HamsterColor.Grey)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[19];
                    // Purple
                    else if (hamster.Color == Hamster.HamsterColor.Purple)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[23];
                    // Pink
                    else if (hamster.Color == Hamster.HamsterColor.Pink)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[27];
                    // Brown (Full)
                    else if (hamster.Color == Hamster.HamsterColor.FullBrown)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[31];
                    // Blue (Full)
                    else if (hamster.Color == Hamster.HamsterColor.FullBlue)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[35];
                    // Grey (Full)
                    else if (hamster.Color == Hamster.HamsterColor.FullGrey)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[39];
                    // White (Full)
                    else if (hamster.Color == Hamster.HamsterColor.FullWhite)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[43];
                    // Evil
                    else if (hamster.Color == Hamster.HamsterColor.Evil)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[47];
                    break;
                case Hamster.LookingDirection.West:
                    // Orange
                    if (hamster.Color == Hamster.HamsterColor.Orange)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[2];
                    // Black
                    else if (hamster.Color == Hamster.HamsterColor.Black)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[6];
                    // Blue
                    else if (hamster.Color == Hamster.HamsterColor.Blue)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[10];
                    // White
                    else if (hamster.Color == Hamster.HamsterColor.White)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[14];
                    // Grey
                    else if (hamster.Color == Hamster.HamsterColor.Grey)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[18];
                    // Purple
                    else if (hamster.Color == Hamster.HamsterColor.Purple)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[22];
                    // Pink
                    else if (hamster.Color == Hamster.HamsterColor.Pink)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[26];
                    // Brown (Full)
                    else if (hamster.Color == Hamster.HamsterColor.FullBrown)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[30];
                    // Blue (Full)
                    else if (hamster.Color == Hamster.HamsterColor.FullBlue)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[34];
                    // Grey (Full)
                    else if (hamster.Color == Hamster.HamsterColor.FullGrey)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[38];
                    // White (Full)
                    else if (hamster.Color == Hamster.HamsterColor.FullWhite)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[42];
                    // Evil
                    else if (hamster.Color == Hamster.HamsterColor.Evil)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[46];
                    break;
                case Hamster.LookingDirection.South:
                    // Orange
                    if (hamster.Color == Hamster.HamsterColor.Orange)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[1];
                    // Black
                    else if (hamster.Color == Hamster.HamsterColor.Black)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[5];
                    // Blue
                    else if (hamster.Color == Hamster.HamsterColor.Blue)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[9];
                    // White
                    else if (hamster.Color == Hamster.HamsterColor.White)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[13];
                    // Grey
                    else if (hamster.Color == Hamster.HamsterColor.Grey)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[17];
                    // Purple
                    else if (hamster.Color == Hamster.HamsterColor.Purple)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[21];
                    // Pink
                    else if (hamster.Color == Hamster.HamsterColor.Pink)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[25];
                    // Brown (Full)
                    else if (hamster.Color == Hamster.HamsterColor.FullBrown)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[29];
                    // Blue (Full)
                    else if (hamster.Color == Hamster.HamsterColor.FullBlue)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[33];
                    // Grey (Full)
                    else if (hamster.Color == Hamster.HamsterColor.FullGrey)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[37];
                    // White (Full)
                    else if (hamster.Color == Hamster.HamsterColor.FullWhite)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[41];
                    // Evil
                    else if (hamster.Color == Hamster.HamsterColor.Evil)
                        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.hamsterSprites[45];
                    break;
            }
        }
    }

    /// <summary>
    /// Refresh the position of a specific hamster in the world.
    /// </summary>
    /// <param name="hamster"></param>
    public void UpdateHamsterPosition(Hamster hamster)
    {
        Transform hamTransform = null;
        foreach (Transform transform in hamsterCollection)
        {
            Hamster ham = transform.GetChild(1).GetComponent<HamsterHolder>().hamster;

            if (hamster.Id == ham.Id)
            {
                ham.CanMove = hamster.CanMove;

                hamTransform = transform;
                ham.Row = hamster.Row;
                ham.Column = hamster.Column;
            }
        }

        hamTransform.position = new Vector3((hamster.Column * TILESIZE), (hamster.Row * TILESIZE), hamTransform.position.z);
    }


    public void LoadFromAssets(int id)
    {
        string[] assets = AssetDatabase.FindAssets("hamster_");

        for(int i = 0; i < assets.Length; i++)
        {
            if (assets[i].Contains("_" + id + "_"))
            {
                //
            }
        }
    }

    /// <summary>
    /// Refresh the amount of grains a hamster owns.
    /// </summary>
    /// <param name="hamster"></param>
    public void UpdateHamsterGrainCount(Hamster hamster)
    {
        foreach (Transform transform in hamsterCollection)
        {
            Hamster ham = transform.GetChild(1).GetComponent<HamsterHolder>().hamster;

            if (hamster.Id == ham.Id)
            {
                ham.GrainCount = hamster.GrainCount;
            }
        }
    }

    public void UpdateHamsterProperties(Hamster hamster, bool createNameUI = false, bool createHealthUI = false, bool createEnduranceUI = false, bool updateNameUI = false, bool updateHealthUI = false, bool updateEnduranceUI = false, bool showSpeechBubble = false, string speechText = "", float speechTimer = 2f)
    {
        foreach (Transform transform in hamsterCollection)
        {
            Hamster ham = transform.GetChild(1).GetComponent<HamsterHolder>().hamster;

            if (ham.MovePoint == null)
            {
                ham.MovePoint = transform.GetChild(0);
            }

            

            if (hamster.Id == ham.Id)
            {
                ham.Name                    = hamster.Name;
                ham.HealthPoints            = hamster.HealthPoints;
                ham.EndurancePoints         = hamster.EndurancePoints;
                ham.HealthPointsFull        = hamster.HealthPointsFull;
                ham.EndurancePointsFull     = hamster.EndurancePointsFull;
                ham.Row                     = hamster.Row;
                ham.Column                  = hamster.Column;
                ham.Color                   = hamster.Color;
                ham.Direction               = hamster.Direction;
                ham.GrainCount              = hamster.GrainCount;
                ham.PlayerControl           = hamster.PlayerControl;
                ham.Inventory               = hamster.Inventory;
                ham.CanTalk                 = hamster.CanTalk;
                ham.CanTrade                = hamster.CanTrade;
                ham.IsTrading               = hamster.IsTrading;
                ham.IsTalking               = hamster.IsTalking;
                ham.IsInInventory           = hamster.IsInInventory;
                ham.IsUsingEndurance        = hamster.IsUsingEndurance;
                ham.IsDisplayingName        = hamster.IsDisplayingName;
                ham.IsDisplayingHealth      = hamster.IsDisplayingHealth;
                ham.IsDisplayingEndurance   = hamster.IsDisplayingEndurance;
                ham.IsUsingItem             = hamster.IsUsingItem;
                ham.EffectsActiv            = hamster.EffectsActiv;
                ham.MoveSpeed               = hamster.MoveSpeed;
                ham.Respawn                 = hamster.Respawn;
                ham.GodMode                 = hamster.GodMode;
                ham.SnapCamera              = hamster.SnapCamera;

                if (ham.Respawn && ham.StartPoint == null)
                {
                    ham.StartPoint = hamster.StartPoint;
                }

                if (ham.SnapCamera)
                {
                    cam = GameObject.Find("Main Camera 2");
                    if (Camera.main.GetComponent<CameraFollow>().target == null)
                    {
                        Camera.main.GetComponent<CameraFollow>().target = ham.HamsterObject.transform;
                    }
                    else if (cam.GetComponent<CameraFollow>().target == null)
                    {
                        cam.GetComponent<CameraFollow>().target = ham.HamsterObject.transform;
                        cam.GetComponent<Camera>().enabled = true;
                        cam.tag = "MainCamera";

                        Camera.main.GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1f);
                    }
                }

                if (createNameUI)
                {
                    if (ham.IsDisplayingName)
                    {
                        transform.GetChild(2).gameObject.SetActive(true);
                        transform.GetChild(2).GetComponent<TextMeshPro>().SetText(ham.Name);
                    }
                    else
                    {
                        transform.GetChild(2).gameObject.SetActive(false);
                    }
                }

                if (showSpeechBubble)
                {
                    transform.GetChild(6).gameObject.SetActive(true);
                    transform.GetChild(7).gameObject.SetActive(true);
                    transform.GetChild(7).GetComponent<TextMeshPro>().SetText(speechText);

                    GameObject.Find("Grid").GetComponent<Territory>().StartCoroutine(ham.HamsterObject.GetComponent<HamsterHolder>().DisplaySpeechBubble(speechTimer));
                }

                if (createHealthUI)
                {
                    if (ham.IsDisplayingHealth)
                    {
                        transform.GetChild(3).gameObject.SetActive(true);
                        for (int j = 1; j <= ham.HealthPoints; j++)
                        {
                            GameObject heartGo = Instantiate(hamsterGameManager.miniHeartPrefab, transform.GetChild(3));
                            heartGo.SetActive(true);
                            
                            if (j <= ham.HealthPointsFull)
                            {
                                heartGo.GetComponent<SpriteRenderer>().sprite = hamsterGameManager.healthSprite[0];
                            }
                            else
                            {
                                heartGo.GetComponent<SpriteRenderer>().sprite = hamsterGameManager.healthSprite[1];
                            }
                        }
                    }
                }

                if (createEnduranceUI)
                {
                    if (ham.IsDisplayingEndurance)
                    {
                        transform.GetChild(4).gameObject.SetActive(true);
                        for (int j = 1; j <= ham.EndurancePoints; j++)
                        {
                            GameObject heartGo = Instantiate(hamsterGameManager.miniEndurancePrefab, transform.GetChild(4));
                            heartGo.SetActive(true);

                            if (j <= ham.EndurancePointsFull)
                            {
                                heartGo.GetComponent<SpriteRenderer>().sprite = hamsterGameManager.enduranceSprite[0];
                            }
                            else
                            {
                                heartGo.GetComponent<SpriteRenderer>().sprite = hamsterGameManager.enduranceSprite[1];
                            }
                        }
                    }
                }

                if (updateHealthUI)
                {
                    if (ham.IsDisplayingHealth)
                    {
                        for (int j = 0; j < transform.GetChild(3).childCount; j++)
                        {
                            if (j < ham.HealthPointsFull)
                            {
                                transform.GetChild(3).GetChild(j).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.healthSprite[0];
                            }
                            else
                            {
                                transform.GetChild(3).GetChild(j).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.healthSprite[1];
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < transform.GetChild(3).childCount; j++)
                        {
                            transform.GetChild(3).GetChild(j).GetComponent<SpriteRenderer>().sprite = null;
                        }
                    }
                }

                if (updateEnduranceUI)
                {
                    if (ham.IsDisplayingEndurance)
                    {
                        for (int j = 0; j < transform.GetChild(4).childCount; j++)
                        {
                            if (j < ham.EndurancePointsFull)
                            {
                                transform.GetChild(4).GetChild(j).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.enduranceSprite[0];
                            }
                            else
                            {
                                transform.GetChild(4).GetChild(j).GetComponent<SpriteRenderer>().sprite = hamsterGameManager.enduranceSprite[1];
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < transform.GetChild(4).childCount; j++)
                        {
                            transform.GetChild(4).GetChild(j).GetComponent<SpriteRenderer>().sprite = null;
                        }
                    }
                }
            }
        }
    }
    

    private IEnumerator PlayerControlMovementTimer()
    {
        while (true)
        {
            if (HamsterGameManager.isTrading || HamsterGameManager.isTalking)
            {
                yield return null;
            }

            if (!playerCanMove)
            {
                yield return wait;
                playerCanMove = true;
            }
            yield return null;
        }

        
    }

    private IEnumerator Player2ControlMovementTimer()
    {
        while (true)
        {
            if (HamsterGameManager.isTrading || HamsterGameManager.isTalking)
            {
                yield return null;
            }

            if (!player2CanMove)
            {
                yield return wait;
                player2CanMove = true;
            }
            yield return null;
        }

    }

    /// <summary>
    /// Get the amount of grains available in the world
    /// </summary>
    /// <returns></returns>
    public int GetGrainCount()
    {
        return this.grainCount;
    }

    /// <summary>
    /// Get the amount of grains available at a specific Position(<paramref name="column"/>, <paramref name="row"/>)
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public int GetGrainCountAtTile(int column, int row)
    {
        for (int i = 0; i < tileCollection.childCount; i++)
        {
            if (tileCollection.GetChild(i).name.Contains(" (" + column + ", " + row + ")"))
            {
                return tileCollection.GetChild(i).GetComponent<TileHolder>().tile.grainCount;
            }
        }
        return 0;
    }

    public void AddHamster(Hamster hamster)
    {
        if (hamster == null) return;

        /* 
         * Check if there is already a hamster with the same id. (Every hamster should have an unique id)
         */
        foreach(Hamster ham in hamsters)
        {
            if (hamster.Id == ham.Id)
            {
                Debug.LogError("Es gibt bereits einen Hamster mit der Id " + hamster.Id);
                return;
            }
        }

        
        foreach (Dialogue dialogue in hamster.NPCDialogues)
        {
            foreach (DialogueCondition condition in dialogue.conditions)
            {
                condition.IsDone = false;
            }
        }

        this.hamsters.Add(hamster);
        activHamsters = this.hamsters;
        //if (!hamster.IsNPC)
        CreateHamsterGo(hamster);
    }

    /// <summary>
    /// Create a hamster gameObject and add it as a child to the hamsterCollection Transform.
    /// </summary>
    /// <param name="hamster"></param>
    private void CreateHamsterGo(Hamster hamster)
    {
        if (hamster == null) return;

        GameObject go = Instantiate(hamsterGameManager.hamsterInstance);
        go.transform.GetChild(1).GetComponent<HamsterHolder>().hamster = hamster;
        go.transform.GetChild(1).GetComponent<HamsterHolder>().hamster.HamsterObject = go;
        go.transform.GetChild(1).GetComponent<HamsterHolder>().hamster.HamsterSpriteRenderer = go.transform.GetChild(1).GetComponent<SpriteRenderer>();

        go.transform.parent = hamsterCollection;
        go.transform.position = new Vector3((go.transform.position.x * TILESIZE), (go.transform.position.y * TILESIZE), go.transform.position.z);
        go.transform.rotation = Quaternion.Euler(-19f, 0, 0);
    }

    public void RemoveHamster(Hamster hamster)
    {
        if (hamster == null) return;

        this.hamsters.Remove(hamster);
        activHamsters = this.hamsters;

        for (int i = 0; i < hamsterGameManager.npcHamsterCollection.childCount; i++)
        {
            if (hamsterGameManager.npcHamsterCollection.GetChild(i).GetComponent<HamsterMB>().hamster.Id == hamster.Id)
            {
                Destroy(hamsterGameManager.npcHamsterCollection.GetChild(i).gameObject);
                return;
            }
        }
    }

    public void RemoveHamster(int index)
    {
        this.hamsters.RemoveAt(index);
        activHamsters = this.hamsters;
    }

    private void RemoveGrain()
    {
        if (changeGrainCount && !addGrainCount)
        {
            changeGrainCount = false;
            this.grainCount -= 1;
            globalGrainCount = grainCount;
        }
    }

    private void AddGrain()
    {
        if(changeGrainCount && addGrainCount)
        {
            changeGrainCount = false;
            addGrainCount = false;
            this.grainCount += 1;
            globalGrainCount = grainCount;
        }
    }

    private IEnumerator RandomHamsterMovements()
    {
        yield return new WaitForSeconds(2.5f);

        while(true)
        {
            foreach(Hamster ham in activHamsters)
            {
                if (ham.IsNPC && 
                    ham.MoveRandom && 
                    ham.Pattern == Hamster.MovementPattern.Random)
                {
                    ham.RandomMove();
                }
                else if (ham.IsNPC &&
                    (ham.Pattern == Hamster.MovementPattern.LeftRight || ham.Pattern == Hamster.MovementPattern.UpDown))
                {
                    if (ham.FrontIsClear())
                    {
                        ham.Move();
                    }
                    else
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            ham.TurnLeft();
                        }
                    }
                }
                else if (ham.IsNPC && ham.Pattern == Hamster.MovementPattern.Rotate)
                {
                    ham.TurnLeft();
                }
            }
            yield return wait;
            yield return null;
        }
    }

    private void Update()
    {
        // Refresh the UI each frame
        UpdateUI();

        // Refresh the Positions for every hamster for each frame
        UpdateHamsterPosition();

        // Check and refresh grains available in the world.
        RemoveGrain();
        AddGrain();

        // Check if it is night or not. If it is night -> turn on lights
        CheckLight();

        // Check if the gamespeed got changed.
        if (gameSpeed != HamsterGameManager.hamsterGameSpeed)
        {
            gameSpeed = HamsterGameManager.hamsterGameSpeed;
            wait = new WaitForSeconds(gameSpeed);
        }
    }
}
