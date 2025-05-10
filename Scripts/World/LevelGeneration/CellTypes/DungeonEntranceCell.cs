using System.Collections.Generic;
using UnityEngine;

public class DungeonEntranceCell : DungeonCell
{
    public DungeonEntranceCell()
    {
    }

    public override void DrawRoom(List<Directions> directions, Vector2Int position)
    {

    }

    public override void GenerateRoomSize()
    {
        SetRoomSize(8);
    }
}