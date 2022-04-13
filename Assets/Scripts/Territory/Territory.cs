using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

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
                    go.GetComponent<TileHolder>().tile = new Tile(x, y, Tile.TileType.Floor);


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
        gameSpeed = HamsterGameManager.hamsterGameSpeed;
        wait = new WaitForSeconds(gameSpeed);
        base.StartCoroutine(PlayerControlMovementTimer());
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
    /// Aktualisiere ein bestimmtes Teil des Spielfelds z.B. nach dem aufheben eines Korns / Körner
    /// <para><paramref name="column"/> = x</para>
    /// <para><paramref name="row"/> = y</para>
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    public void UpdateTile(int column, int row, bool addingItem = false, Hamster hamster = null)
    {
        /* Loop durch alle Tiles die es gibt */
        for(int i = 0; i < tileCollection.childCount; i++)
        {
            if(tileCollection.GetChild(i).name.Contains(" (" + column + ", " + row + ")"))
            {
                /* 
                 * Falls einem Hamster ein Item aufhebt und dem inventar hinzugefügt wird, 
                 * oder einfach nur ein Korn aufsammelt 
                 */
                if (!addingItem)
                {
                    int tileGrainCount;
                    if (!addGrainCount)
                    {
                        tileCollection.GetChild(i).GetComponent<TileHolder>().tile.grainCount -= 1;
                        tileGrainCount = tileCollection.GetChild(i).GetComponent<TileHolder>().tile.grainCount;
                        List<Quest> quests = hamsterGameManager.GetComponent<QuestHolder>().quests;

                        foreach (Quest quest in quests)
                        {
                            quest.stageInfo.condition.OnRemoveGrain();
                        }
                    }
                    else
                    {
                        tileCollection.GetChild(i).GetComponent<TileHolder>().tile.grainCount += 1;
                        tileGrainCount = tileCollection.GetChild(i).GetComponent<TileHolder>().tile.grainCount;
                        List<Quest> quests = hamsterGameManager.GetComponent<QuestHolder>().quests;

                        foreach (Quest quest in quests)
                        {
                            quest.stageInfo.condition.OnAddGrain();
                        }
                    }

                    if (hamster == null) return;

                    /* 
                     * Hier können weitere Bodentexturen eingefügt werden, bis jetzt gibt es Sand und Grass 
                     * Index Beschreibung:
                     * 0 (+10) = Keine Körner mehr
                     * 1 (+10) = 1 Korn
                     * 2 (+10) = 2 Körner
                     * usw.
                     * 
                     * Nächster Boden wäre dann +20, übernächsterboden +30...
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
                    // ================================ WEITERE TILES ===========%20%================
                    // ...
                    // ...
                }
                else
                {
                    tileCollection.GetChild(i).GetComponent<ItemHolder>().item = null;
                    Destroy(tileCollection.GetChild(i).GetComponent<ItemHolder>());
                    RemoveItemFromTile(column, row);
                    SetTileAt(column, row, itemsTilemap, null);
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
        /* Aktiviere den Canvas für den Handel und deaktivere alle anderen UIs */
        hamsterGameManager.SetCanvasVisibility(hamsterGameManager.tradeCanvas, true);

        hamsterGameManager.SetCanvasVisibility(hamsterGameManager.inventoryCanvas, false);
        hamsterGameManager.SetCanvasVisibility(hamsterGameManager.dialogueCanvas, false);
        hamsterGameManager.SetCanvasVisibility(hamsterGameManager.generalUI, false);
    }

    public void DisplayDialogueWindow(Hamster hamster1, Hamster hamster2)
    {

    }

    /// <summary>
    /// Erhalte den Inhalt eines speziellen Teils des Spielfelds an der Position(<paramref name="column"/>, <paramref name="row"/>).
    /// Falls sich das Feld nicht im Spielfeld befindet / nicht gesetzt wurde wird null zurückgegeben
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
    /// Setze ein <paramref name="tile"/> für eine bestimmte Tilemap <paramref name="map"/> an der
    /// Position(<paramref name="column"/>, <paramref name="row"/>)
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    /// <param name="map"></param>
    /// <param name="tile"></param>
    public void SetTileAt(int column, int row, Tilemap map, TileBase tile)
    {
        map.SetTile(new Vector3Int(column, row, 0), tile);
    }

    /// <summary>
    /// Entferne ein Item vom Tile an der Position(<paramref name="column"/>, <paramref name="row"/>) und entferne das
    /// GameObject
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
    /// Erhalte das Item das sich an der Position(<paramref name="column"/>, <paramref name="row"/>) befindet.
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
    /// Verändere die Anzahl <paramref name="grainCount"/> der Körner an der Position(<paramref name="column"/>, <paramref name="row"/>)
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
    /// Gehe durch alle Tiles und aktualisiere diese (Falls ein Hamster ein Korn abgelegt oder aufgehoben hat)
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
    /// Erhalte die Anzahl der Hamster die sich in der Welt befinden (Aktiv/Inaktiv)
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
    /// Gebe den Hamster an einer gewissen Position auf dem Spielfeld zurück.
    /// Falls sich kein Hamster auf dieser Position befindet wird null zurückgegeben.
    /// <para><paramref name="column"/> = x</para>
    /// <para><paramref name="row"/> = y</para>
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
    /// Erhalte den Hamster mit einer bestimmten <paramref name="id"/>
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
    /// Erhalte das Hamster GameObject anhand der hamster <paramref name="id"/>
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
    /// Aktualisiere die Rotation (Richtung in die der Hamster schaut) eines bestimmten Hamster <paramref name="hamster"/>
    /// in eine spezielle Richtung <paramref name="direction"/>
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
    /// Aktualisiere die Positionen aller Hamster auf dem Spielfeld
    /// </summary>
    public void UpdateHamsterPosition()
    {
        if (hamsterCollection == null) return;

        foreach (Transform transform in hamsterCollection)
        {
            transform.parent = hamsterCollection;
            Hamster hamster = transform.GetChild(1).GetComponent<HamsterHolder>().hamster;

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
                    break;
            }
        }
    }

    /// <summary>
    /// Aktualisiere die Position eines bestimmten Hamsters
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

    /// <summary>
    /// Aktualisiere die Anzahl an Körner, die der Hamster besitzt.
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

    public void UpdateHamsterProperties(Hamster hamster, bool createNameUI = false, bool createHealthUI = false, bool createEnduranceUI = false, bool updateNameUI = false, bool updateHealthUI = false, bool updateEnduranceUI = false)
    {
        foreach (Transform transform in hamsterCollection)
        {
            Hamster ham = transform.GetChild(1).GetComponent<HamsterHolder>().hamster;

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
                ham.IsInInventory           = hamster.IsInInventory;
                ham.IsUsingEndurance        = hamster.IsUsingEndurance;
                ham.IsDisplayingName        = hamster.IsDisplayingName;
                ham.IsDisplayingHealth      = hamster.IsDisplayingHealth;
                ham.IsDisplayingEndurance   = hamster.IsDisplayingEndurance;
                ham.IsUsingItem             = hamster.IsUsingItem;
                ham.EffectsActiv            = hamster.EffectsActiv;


                // Aktualisiere den Bewegungseffekt
                if (!ham.EffectsActiv)
                {
                    ham.MoveSpeed = 1;
                }
                else
                {
                    foreach(ItemSlot slot in ham.Inventory)
                    {
                        if (slot.item.IsEquipped && slot.item.hasSpecialEffects && slot.item.MoveSpeed > 0)
                        {
                            ham.MoveSpeed = slot.item.MoveSpeed;
                        }
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

    /// <summary>
    /// Erhalte die Anzahl aller Körner auf dem Spielfeld
    /// </summary>
    /// <returns></returns>
    public int GetGrainCount()
    {
        return this.grainCount;
    }

    /// <summary>
    /// Erhalte die Anzahl der Körner auf der <paramref name="row"/>=y (Reihe) und der <paramref name="column"/>=x (Spalte)
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
         * Überprüfe ob ein Hamster mit der selben Id bereits existiert. (Jeder Hamster sollte eine einzigartige Id besitzen)
         * Ein Fehler kann im grunde nicht passieren, trotzdem sollte es überprüft werden.
         * Da es ansonsten später zu Fehler führen könnte.
         */
        foreach(Hamster ham in hamsters)
        {
            if (hamster.Id == ham.Id)
            {
                Debug.LogError("Es gibt bereits einen Hamster mit der Id " + hamster.Id);
                return;
            }
        }

        this.hamsters.Add(hamster);
        activHamsters = this.hamsters;
        CreateHamsterGo(hamster);
    }

    /// <summary>
    /// Erstelle einen Hamster GameObject und füge in als Kind zum hamsterCollection Transform hinzu.
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

    private void Update()
    {
        // Aktualisiere die UI jeden Frame
        UpdateUI();

        // Aktualisiere die Positionen aller Hamster jeden Frame
        UpdateHamsterPosition();

        // Überprüfe und aktualisiere die Körner in der Spielewelt
        RemoveGrain();
        AddGrain();

        // Check if it is night or not. If it is night -> turn on lights
        CheckLight();

        // Überprüfe ob die gamespeed verändert wurde
        if (gameSpeed != HamsterGameManager.hamsterGameSpeed)
        {
            gameSpeed = HamsterGameManager.hamsterGameSpeed;
            wait = new WaitForSeconds(gameSpeed);
        }
    }
}
