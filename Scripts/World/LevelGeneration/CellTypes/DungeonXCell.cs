using System.Collections.Generic;
using UnityEngine;

public class DungeonXCell : DungeonCell
{
    public DungeonXCell()
    {

    }

    public override void DrawRoom(List<Directions> directions, Vector2Int position)
    {
        TileDrawer.Instance.DrawXRoom(roomTilePositions, roomSize, position, cellSize, directions);
        ObjectPlacement.Instance.PlaceObjects(floorTilePositions);
    }
}