using System.Collections.Generic;
using UnityEngine;

public class DungeonWallCell : DungeonCell
{
    public DungeonWallCell()
    {

    }
    // Should have a bigger sized room
    public override void DrawRoom(List<Directions> directions, Vector2Int position)
    {
        TileDrawer.Instance.DrawWallRoom(roomTilePositions, position, cellSize, directions);
        ObjectPlacement.Instance.PlaceObjects(floorTilePositions);
    }

    public override void GenerateRoomSize()
    {
        SetRoomSize(24);
    }
}