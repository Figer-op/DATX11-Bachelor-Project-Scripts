using System.Collections.Generic;
using UnityEngine;

public class DungeonWeirdXCell : DungeonCell
{

    public DungeonWeirdXCell()
    {
    
    }
   
    public override void DrawRoom(List<Directions> directions, Vector2Int position)
    {
        TileDrawer.Instance.DrawWeirdXRoom(roomTilePositions, roomSize, position);
        ObjectPlacement.Instance.PlaceObjects(floorTilePositions);
    }
}