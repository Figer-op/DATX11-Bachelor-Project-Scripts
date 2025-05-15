using System.Collections.Generic;
using UnityEngine;

public class RoomInfo
{
    public Vector2Int Cell { get; set; }
    public List<Directions> InDirections { get; set; }
    public List<Directions> OutDirections { get; set; }
}