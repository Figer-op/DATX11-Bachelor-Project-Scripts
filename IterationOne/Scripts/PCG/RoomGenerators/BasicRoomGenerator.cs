using System.Collections.Generic;
using UnityEngine;

public class BasicRoomGenerator : MonoBehaviour
{
    [SerializeField]
    private DrawTiles drawTiles;
    [SerializeField]
    private Vector2Int position = Vector2Int.zero;
    [SerializeField]
    private int size = 8;
    [SerializeField]
    private List<Directions> directions = new();
    [SerializeField]
    private int cellSize;
    [SerializeField]
    private ObjectPlacement objectPlacement;
    [SerializeField]
    private ObjectPlacementSO dungeonExit;

    private readonly HashSet<Vector2Int> floorPositions = new();
    private readonly HashSet<Vector2Int> cellWalls = new();
    private void GenerateRoomPositions()
    {
        GenerateWallPositions();
        GenerateFloorPosition();

        if (directions.Contains(Directions.North)) HandleNorth();
        if (directions.Contains(Directions.South)) HandleSouth();
        if (directions.Contains(Directions.East)) HandleEast();
        if (directions.Contains(Directions.West)) HandleWest();
    }

    private void GenerateWallPositions()
    {
        cellWalls.Clear();
        int halfCellSize = cellSize / 2;
        for (int i = position.x - halfCellSize; i <= position.x + halfCellSize; i++)
        {
            for (int j = position.y - halfCellSize; j <= position.y + halfCellSize; j++)
            {
                cellWalls.Add(new Vector2Int(i, j));
            }
        }
    }

    private void GenerateWallPositions(int cellSize, Vector2Int position)
    {
        cellWalls.Clear();
        int halfCellSize = cellSize / 2;
        for (int i = position.x - halfCellSize; i <= position.x + halfCellSize; i++)
        {
            for (int j = position.y - halfCellSize; j <= position.y + halfCellSize; j++)
            {
                cellWalls.Add(new Vector2Int(i, j));
            }
        }
    }

    private void GenerateFloorPosition(int size, Vector2Int position)
    {
        floorPositions.Clear();
        int halfSize = size / 2;
        for (int x = position.x - halfSize; x <= position.x + halfSize; x++)
        {
            for (int y = position.y - halfSize; y <= position.y + halfSize; y++)
            {
                floorPositions.Add(new Vector2Int(x, y));
            }
        }
    }

    private void GenerateFloorPosition()
    {
        floorPositions.Clear();
        int halfSize = size / 2;
        for (int x = position.x - halfSize; x <= position.x + halfSize; x++)
        {
            for (int y = position.y - halfSize; y <= position.y + halfSize; y++)
            {
                floorPositions.Add(new Vector2Int(x, y));
            }
        }
    }

    private void HandleNorth()
    {
        int yPos = position.y + (size / 2) + 1;
        float limit = Mathf.Ceil((cellSize - size) * 0.5f);
        for (int i = 0; i < limit; i++)
        {
            floorPositions.Add(new Vector2Int(position.x, yPos + i));
            floorPositions.Add(new Vector2Int(position.x + 1, yPos + i));
            floorPositions.Add(new Vector2Int(position.x - 1, yPos + i));
        }
    }

    private void HandleSouth()
    {
        int yPos = position.y - (size / 2) - 1;
        float limit = Mathf.Ceil((cellSize - size) * 0.5f);
        for (int i = 0; i < limit; i++)
        {
            floorPositions.Add(new Vector2Int(position.x, yPos - i));
            floorPositions.Add(new Vector2Int(position.x + 1, yPos - i));
            floorPositions.Add(new Vector2Int(position.x - 1, yPos - i));
        }
    }

    private void HandleEast()
    {
        int xPos = position.x + (size / 2) + 1;
        float limit = Mathf.Ceil((cellSize - size) * 0.5f);
        for (int i = 0; i < limit; i++)
        {
            floorPositions.Add(new Vector2Int(xPos + i, position.y));
            floorPositions.Add(new Vector2Int(xPos + i, position.y + 1));
            floorPositions.Add(new Vector2Int(xPos + i, position.y - 1));
        }
    }

    private void HandleWest()
    {
        int xPos = position.x - (size / 2) - 1;
        float limit = Mathf.Ceil((cellSize - size) * 0.5f);
        for (int i = 0; i < limit; i++)
        {
            floorPositions.Add(new Vector2Int(xPos - i, position.y));
            floorPositions.Add(new Vector2Int(xPos - i, position.y + 1));
            floorPositions.Add(new Vector2Int(xPos - i, position.y - 1));
        }
    }

    private void HandleNorth(int size, int cellSize, Vector2Int position)
    {
        int yPos = position.y + (size / 2) + 1;
        float limit = Mathf.Ceil((cellSize - size) * 0.5f);
        for (int i = 0; i < limit; i++)
        {
            floorPositions.Add(new Vector2Int(position.x, yPos + i));
            floorPositions.Add(new Vector2Int(position.x + 1, yPos + i));
            floorPositions.Add(new Vector2Int(position.x - 1, yPos + i));
        }
    }

    private void HandleSouth(int size, int cellSize, Vector2Int position)
    {
        int yPos = position.y - (size / 2) - 1;
        float limit = Mathf.Ceil((cellSize - size) * 0.5f);
        for (int i = 0; i < limit; i++)
        {
            floorPositions.Add(new Vector2Int(position.x, yPos - i));
            floorPositions.Add(new Vector2Int(position.x + 1, yPos - i));
            floorPositions.Add(new Vector2Int(position.x - 1, yPos - i));
        }
    }

    private void HandleEast(int size, int cellSize, Vector2Int position)
    {
        int xPos = position.x + (size / 2) + 1;
        float limit = Mathf.Ceil((cellSize - size) * 0.5f);
        for (int i = 0; i < limit; i++)
        {
            floorPositions.Add(new Vector2Int(xPos + i, position.y));
            floorPositions.Add(new Vector2Int(xPos + i, position.y + 1));
            floorPositions.Add(new Vector2Int(xPos + i, position.y - 1));
        }
    }

    private void HandleWest(int size, int cellSize, Vector2Int position)
    {
        int xPos = position.x - (size / 2) - 1;
        float limit = Mathf.Ceil((cellSize - size) * 0.5f);
        for (int i = 0; i < limit; i++)
        {
            floorPositions.Add(new Vector2Int(xPos - i, position.y));
            floorPositions.Add(new Vector2Int(xPos - i, position.y + 1));
            floorPositions.Add(new Vector2Int(xPos - i, position.y - 1));
        }
    }

    //Overload for Manager
    public void DrawRoom(int size, List<Directions> inDirections, List<Directions> outDirections, Vector2Int position,
    int cellSize, bool isLastRoom, bool isFirstRoom)
    {
        GenerateFloorPosition(size, position);
        GenerateWallPositions(cellSize, position);
        if (inDirections.Contains(Directions.North)) HandleNorth(size, cellSize, position);
        if (inDirections.Contains(Directions.South)) HandleSouth(size, cellSize, position);
        if (inDirections.Contains(Directions.East)) HandleEast(size, cellSize, position);
        if (inDirections.Contains(Directions.West)) HandleWest(size, cellSize, position);

        if (outDirections.Contains(Directions.North)) HandleNorth(size, cellSize, position);
        if (outDirections.Contains(Directions.South)) HandleSouth(size, cellSize, position);
        if (outDirections.Contains(Directions.East)) HandleEast(size, cellSize, position);
        if (outDirections.Contains(Directions.West)) HandleWest(size, cellSize, position);

        drawTiles.DrawRoom(cellWalls, DrawTileType.Wall);
        drawTiles.DrawRoom(floorPositions, DrawTileType.Floor);
        drawTiles.DrawRoom(floorPositions, DrawTileType.Water);
        drawTiles.DrawRoom(floorPositions, DrawTileType.Abyss);
        if (!isFirstRoom)
        {
            objectPlacement.PlaceObjects(floorPositions);
            if (isLastRoom)
            {
                objectPlacement.PlaceObject(floorPositions, dungeonExit);
            }
        }
    }
    //Overload for editor
    public void DrawRoom()
    {
        GenerateRoomPositions();
        drawTiles.DrawRoom(cellWalls, DrawTileType.Wall);
        drawTiles.DrawRoom(floorPositions, DrawTileType.Floor);
        drawTiles.DrawRoom(floorPositions, DrawTileType.Water);
        drawTiles.DrawRoom(floorPositions, DrawTileType.Abyss);
        objectPlacement.PlaceObjects(floorPositions);
    }
}
