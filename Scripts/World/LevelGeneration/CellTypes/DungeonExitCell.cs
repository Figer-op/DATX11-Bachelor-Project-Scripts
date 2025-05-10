using System.Collections.Generic;
using UnityEngine;

public class DungeonExitCell : DungeonCell
{
    public DungeonExitCell()
    {
    }

    public override void DrawRoom(List<Directions> directions, Vector2Int position)
    {
        ObjectPlacement.Instance.PlaceObjects(floorTilePositions);
        ObjectPlacement.Instance.PlaceExit(floorTilePositions);
    }

    public override void GenerateRoomSize()
    {
        SetRoomSize(8);
    }
}