using System.Collections.Generic;
using UnityEngine;

public class DungeonIslandCell : DungeonCell
{

    public DungeonIslandCell()
    {
    }
    
    public override void DrawRoom(List<Directions> directions, Vector2Int position)
    {
        TileDrawer.Instance.DrawIslandRoom(roomTilePositions, roomSize, position, cellSize, directions);
        ObjectPlacement.Instance.PlaceObjects(floorTilePositions);
    }

    public override void GenerateRoomSize()
    {
        int randomSize = Random.Range(15, 20);
        SetRoomSize(randomSize);
    }
}