using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Floor,
    Wall,
    Pit
}

public enum TileFlag
{
    Walkable,
    SeeThrough,
    Blocking
}

[CreateAssetMenu(fileName = "New LevelRuleTile", menuName = "Tiles/LevelRuleTile")]
public class LevelRuleTile : RuleTile<LevelRuleTile.Neighbor>
{

    [field: SerializeField]
    public TileType TileType { get; private set; }
    [field: SerializeField]
    public List<TileFlag> TileFlags { get; private set; } = new ();

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int Floor = 3;
        public const int Wall = 4;
        public const int Pit = 5;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        LevelRuleTile levelRuleTile = tile as LevelRuleTile;
        if (levelRuleTile == null)
        {
            return false;
        }
        switch (neighbor)
        {
            case Neighbor.Floor: return levelRuleTile.TileType == TileType.Floor;
            case Neighbor.Wall: return levelRuleTile.TileType == TileType.Wall;
            case Neighbor.Pit: return levelRuleTile.TileType == TileType.Pit;
        }
        return base.RuleMatch(neighbor, tile);
    }
}
