using System.Collections.Generic;
using UnityEngine;

public class DungeonMazeCell : DungeonCell
{
    public DungeonMazeCell()
    {

    }
    // Should have a bigger sized room
    public override void DrawRoom(List<Directions> directions, Vector2Int position)
    {
        TileDrawer.Instance.DrawMazeRoom(roomTilePositions, position, cellSize, directions);
        ObjectPlacement.Instance.PlaceObjects(floorTilePositions);
    }

    public override void GenerateRoomSize()
    {
        SetRoomSize(24);
    }
}