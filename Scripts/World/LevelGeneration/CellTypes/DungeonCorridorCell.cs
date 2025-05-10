using System.Collections.Generic;
using UnityEngine;

public class DungeonCorridorCell : DungeonCell
{
    public DungeonCorridorCell()
    {
    }
   
    public override void DrawRoom(List<Directions> directions, Vector2Int position)
    {
         
    }

    public override void GenerateRoomSize()
    {
        SetRoomSize(3);
    }
}