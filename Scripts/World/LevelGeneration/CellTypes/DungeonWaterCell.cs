using System.Collections.Generic;
using UnityEngine;

public class DungeonWaterCell : DungeonCell
{
    public DungeonWaterCell()
    {
    }

    public override void DrawRoom(List<Directions> directions, Vector2Int position)
    {
        TileDrawer.Instance.DrawWaterRoom(floorTilePositions, DrawTileType.Water, roomSize, position);
        ObjectPlacement.Instance.PlaceObjects(floorTilePositions);
    }
}