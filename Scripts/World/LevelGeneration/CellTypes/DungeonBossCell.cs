using System.Collections.Generic;
using UnityEngine;

public class DungeonBossCell : DungeonCell
{  
    public override void DrawRoom(List<Directions> directions, Vector2Int position)
    {
        TileDrawer.Instance.DrawWaterRoom(floorTilePositions, DrawTileType.Water, roomSize, position);
        ObjectPlacement.Instance.PlaceBoss(position);
    }

    public override void GenerateRoomSize()
    {
        SetRoomSize(15);
    }

}