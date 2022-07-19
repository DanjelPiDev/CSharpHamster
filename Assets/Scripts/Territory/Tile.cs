using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : ScriptableObject
{
    public TileType type;
    public bool hasGrain = false;
    public bool canDamageHealth = false;
    public bool canDamageEndurance = false;
    public int grainCount = 0;
    /*
     * Level -1: Water
     * Level 0: Ground
     * Level 1: Wall 1x
     * Level 2: Wall 2x
     * Level 3: Wall 3x
     * Level 4: Wall 4x
     */
    public int tileLevel = 0;
    private int row;
    private int column;

    #region Getter and Setter
    public int Row
    {
        get { return row; }
        set { row = value; }
    }

    public int Column
    {
        get { return column; }
        set { column = value; }
    }
    #endregion

    public Tile(int row, int column, TileType type = TileType.Wall)
    {
        this.row = row;
        this.column = column;
        this.type = type;
    }

    public enum TileType
    {
        Floor,
        Wall,
        Grain,
        Water,
        None
    };
}
