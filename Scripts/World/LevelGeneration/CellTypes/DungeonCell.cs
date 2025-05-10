using System.Collections.Generic;
using UnityEngine;

public class DungeonCell
{
    public int cellSize = 30;   
    public int cellMargin = 4;
    private int maxRoomSize; 
    public int minRoomSize = 8;
    public int roomSize = 10;

    protected readonly HashSet<Vector2Int> floorTilePositions = new();
    protected readonly HashSet<Vector2Int> roomTilePositions = new();
    protected readonly HashSet<Vector2Int> corridorTilePositions = new();
    protected readonly HashSet<Vector2Int> wallTilePositions = new();
    
    public DungeonCell()
    {
        maxRoomSize = cellSize - cellMargin;
    }

    public virtual void GenerateRoom(Vector2 position, List<Directions> directions)
    {
        GenerateRoomSize();
        Vector2Int worldPosition = CalculateWorldPosition(position);
        PopulateTilePositions(directions, worldPosition);
        DrawBaseRoom();
        DrawRoom(directions, worldPosition);
    }

    public virtual void DrawRoom(List<Directions> directions, Vector2Int position)
    {
        ObjectPlacement.Instance.PlaceObjects(floorTilePositions);
    }

    public virtual void DrawBaseRoom()
    {
        TileDrawer.Instance.DrawRoom(wallTilePositions, DrawTileType.Wall);
        TileDrawer.Instance.DrawRoom(floorTilePositions, DrawTileType.Floor);
    }

    private void PopulateTilePositions(List<Directions> directions, Vector2Int position)
    {
        PopulateRoomTilePositions(position);
        PopulateCorridorTilePositions(directions, position);
        PopulateWallTilePositions(position);
        PopulateFloorTilePositions();
    }

    private void PopulateRoomTilePositions(Vector2Int position)
    {
        roomTilePositions.Clear();
        int halfSize = roomSize / 2;
        for (int x = position.x - halfSize; x <= position.x + halfSize; x++)
        {
            for (int y = position.y - halfSize; y <= position.y + halfSize; y++)
            {
                roomTilePositions.Add(new Vector2Int(x, y));
            }
        }
    }

    private void PopulateCorridorTilePositions(List<Directions> directions, Vector2Int position)
    {
        corridorTilePositions.Clear();
        if (directions.Contains(Directions.North)) HandleNorth(position);
        if (directions.Contains(Directions.South)) HandleSouth(position);
        if (directions.Contains(Directions.East)) HandleEast(position);
        if (directions.Contains(Directions.West)) HandleWest(position);

    }

    private void PopulateFloorTilePositions()
    {
        floorTilePositions.Clear();
        floorTilePositions.UnionWith(roomTilePositions);
        floorTilePositions.UnionWith(corridorTilePositions);
    }

    private void PopulateWallTilePositions(Vector2Int position)
    {
        wallTilePositions.Clear();
        int halfCellSize = cellSize / 2;
        for (int i = position.x - halfCellSize; i <= position.x + halfCellSize; i++)
        {
            for (int j = position.y - halfCellSize; j <= position.y + halfCellSize; j++)
            {
                wallTilePositions.Add(new Vector2Int(i, j));
            }
        }
    }

    private void HandleNorth(Vector2Int position)
    {
        int yPos = position.y + (roomSize / 2) + 1;
        float limit = Mathf.Ceil((cellSize - roomSize) * 0.5f);
        for (int i = 0; i < limit; i++)
        {
            corridorTilePositions.Add(new Vector2Int(position.x, yPos + i));
            corridorTilePositions.Add(new Vector2Int(position.x + 1, yPos + i));
            corridorTilePositions.Add(new Vector2Int(position.x - 1, yPos + i));
        }
    }

    private void HandleSouth(Vector2Int position)
    {
        int yPos = position.y - (roomSize / 2) - 1;
        float limit = Mathf.Ceil((cellSize - roomSize) * 0.5f);
        for (int i = 0; i < limit; i++)
        {
            corridorTilePositions.Add(new Vector2Int(position.x, yPos - i));
            corridorTilePositions.Add(new Vector2Int(position.x + 1, yPos - i));
            corridorTilePositions.Add(new Vector2Int(position.x - 1, yPos - i));
        }
    }

    private void HandleEast(Vector2Int position)
    {
        int xPos = position.x + (roomSize / 2) + 1;
        float limit = Mathf.Ceil((cellSize - roomSize) * 0.5f);
        for (int i = 0; i < limit; i++)
        {
            corridorTilePositions.Add(new Vector2Int(xPos + i, position.y));
            corridorTilePositions.Add(new Vector2Int(xPos + i, position.y + 1));
            corridorTilePositions.Add(new Vector2Int(xPos + i, position.y - 1));
        }
    }

    private void HandleWest(Vector2Int position)
    {
        int xPos = position.x - (roomSize / 2) - 1;
        float limit = Mathf.Ceil((cellSize - roomSize) * 0.5f);
        for (int i = 0; i < limit; i++)
        {
            corridorTilePositions.Add(new Vector2Int(xPos - i, position.y));
            corridorTilePositions.Add(new Vector2Int(xPos - i, position.y + 1));
            corridorTilePositions.Add(new Vector2Int(xPos - i, position.y - 1));
        }
    }

    public Vector2Int CalculateWorldPosition(Vector2 position)
    {
        return new Vector2Int((int)(position.x * cellSize), (int)(position.y * cellSize));
    }

    public virtual void GenerateRoomSize()
    {
        maxRoomSize = 16;
        roomSize = Random.Range(maxRoomSize, minRoomSize);
    }

    public void SetRoomSize(int newRoomSize)
    {
        roomSize = newRoomSize;
    }

    public override bool Equals(object obj)
    {
        return obj is DungeonCell otherCell && Matches(otherCell);
    }

    public bool Matches(DungeonCell otherCell)
    {
        return otherCell != null && otherCell.GetType().IsAssignableFrom(GetType());
    }
}