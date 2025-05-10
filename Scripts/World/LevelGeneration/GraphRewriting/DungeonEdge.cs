
using System;
using UnityEngine;

public class DungeonEdge
{
    public DungeonNode From { get; set; }
    public DungeonNode To { get; set; }

    public DungeonEdge(DungeonNode from, DungeonNode to)
    {
        From = from;
        To = to;
    }

    public DungeonEdge(DungeonEdge otherEdge)
    {
        From = new DungeonNode(otherEdge.From);
        To = new DungeonNode(otherEdge.To);
    }

    public Directions? GetDirection()
    {
        return ConvertVectorToDirection(To.Position - From.Position);
    }

    public Directions? GetReverseDirection()
    {
        return ConvertVectorToDirection(From.Position - To.Position);
    }

    private Directions? ConvertVectorToDirection(Vector2 vector)
    {
        return vector switch
        {
            var v when v == Vector2.up => (Directions?)Directions.North,
            var v when v == Vector2.down => (Directions?)Directions.South,
            var v when v == Vector2.right => (Directions?)Directions.East,
            var v when v == Vector2.left => (Directions?)Directions.West,
            _ => null,
        };
    }

    public override bool Equals(object obj)
    {
        if (obj is DungeonEdge otherEdge)
        {
            return (From.Equals(otherEdge.From) && To.Equals(otherEdge.To)) ||
                   (From.Equals(otherEdge.To) && To.Equals(otherEdge.From));
        }
        return false;
    }

    public bool Matches(DungeonEdge otherEdge)
    {
        return (From.Matches(otherEdge.From) && To.Matches(otherEdge.To)) ||
               (From.Matches(otherEdge.To) && To.Matches(otherEdge.From));
    }

    public bool CanReplace(DungeonEdge otherEdge)
    {
        return (From.CanReplace(otherEdge.From) && To.CanReplace(otherEdge.To)) ||
               (From.CanReplace(otherEdge.To) && To.CanReplace(otherEdge.From));
    }

    public override int GetHashCode()
    {
        int hashFrom = From.GetHashCode();
        int hashTo = To.GetHashCode();
        return hashFrom < hashTo ? hashFrom ^ hashTo : hashTo ^ hashFrom;
    }

    public override string ToString()
    {
        return $"Edge from {From.Position} {From.Cell} to {To.Position} {To.Cell}";
    }
}