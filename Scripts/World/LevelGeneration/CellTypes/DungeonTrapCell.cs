using System.Collections.Generic;
using UnityEngine;

public class DungeonTrapCell : DungeonCell
{
    public DungeonTrapCell()
    {

    }

    public override void DrawRoom(List<Directions> directions, Vector2Int position)
    {
        TileDrawer.Instance.DrawWallRoom(roomTilePositions, position, cellSize, directions);
        ObjectPlacement.Instance.PlaceTrapRandom(floorTilePositions);
        ObjectPlacement.Instance.PlaceFireTrapRandom(floorTilePositions);
        ObjectPlacement.Instance.PlaceObjects(floorTilePositions);
    }

    public override void GenerateRoomSize()
    {
        SetRoomSize(20);
    }
}