using System.Collections.Generic;
using UnityEngine;

public class DungeonLootCell : DungeonCell
{
    public override void DrawRoom(List<Directions> directions, Vector2Int position)
    {
        TileDrawer.Instance.DrawWaterRoom(floorTilePositions, DrawTileType.Water, roomSize, position);
        int chestOffset = 2;
        Vector2Int[] corners = new Vector2Int[]
        {
            new(position.x - chestOffset, position.y + chestOffset),
            new(position.x + chestOffset, position.y + chestOffset),
            new(position.x - chestOffset, position.y - chestOffset),
            new(position.x + chestOffset, position.y - chestOffset)      
        };

        foreach (Vector2Int chestPosition in corners)
        {
            ObjectPlacement.Instance.PlaceChest(chestPosition);
        }
    }

    public override void GenerateRoomSize()
    {
        SetRoomSize(10);
    }
}