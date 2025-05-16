using UnityEngine;

public static class DirectionsExtensions
{
    public static Vector2Int GetOffset(this Directions dir)
    {
        return dir switch
        {
            Directions.North => new Vector2Int(0, 1),
            Directions.South => new Vector2Int(0, -1),
            Directions.East => new Vector2Int(1, 0),
            Directions.West => new Vector2Int(-1, 0),
            _ => Vector2Int.zero
        };
    }

    public static Directions GetOpposite(this Directions dir)
    {
        return dir switch
        {
            Directions.North => Directions.South,
            Directions.South => Directions.North,
            Directions.East => Directions.West,
            Directions.West => Directions.East,
            _ => dir
        };
    }

    public static Directions GetRandom()
    {
        return (Directions)Random.Range(0, 4);
    }
}
