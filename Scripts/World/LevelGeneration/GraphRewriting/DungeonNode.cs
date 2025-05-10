
using System.Collections.Generic;
using UnityEngine;

public class DungeonNode
{
    public Vector2 Position { get; set; }
    public DungeonCell Cell { get; set; } = new DungeonCell();

    public DungeonNode(DungeonCell cell, Vector2 position)
    {
        Cell = cell;
        Position = new Vector2(position.x, position.y);
    }

    public DungeonNode(DungeonNode node)
    {
        Cell = node.Cell;
        Position = node.Position;
    }

    public override bool Equals(object obj)
    {
        if (obj is DungeonNode otherNode)
        {
            return Position == otherNode.Position && Cell.Equals(otherNode.Cell);
        }
        return false;
    }

    public bool Matches(DungeonNode otherNode)
    {
        return Position == otherNode.Position && Cell.Matches(otherNode.Cell);
    }

    public bool CanReplace(DungeonNode otherNode)
    {
        return Position == otherNode.Position && Cell.Matches(otherNode.Cell);
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }
}