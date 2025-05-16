using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDrawer : MonoBehaviour
{
    public static TileDrawer Instance { get; private set; }
    [SerializeField]
    private Tilemap floorTileMap;
    [SerializeField]
    private TileBase floorTile;
    [SerializeField]
    private TileBase wallTile;
    [SerializeField]
    private TileBase abyssTile;
    [SerializeField]
    private TileBase waterTile;
    [SerializeField]
    private bool clearTiles = true;

    private readonly HashSet<Vector2Int> painted = new(); // Inaccessible tiles

    private void OnEnable()
    {
        Instance = this;
    }

    public void DrawRoom(HashSet<Vector2Int> positions, DrawTileType tileType)
    {
        ClearTiles();
        TileBase tile = GetTileBase(tileType);
        PaintTiles(positions, tile);
    }

    public void DrawMazeRoom(HashSet<Vector2Int> roomPositions, Vector2Int roomCenter, int cellSize, List<Directions> corridoorDirections)
    {
        ClearTiles();

        HashSet<Vector2Int> visited = new();
        Stack<Vector2Int> stack = new();

        Vector2Int start = roomCenter;
        visited.Add(start);
        stack.Push(start);

        Vector2Int[] directions = new Vector2Int[]
        {
        Vector2Int.up * 2,
        Vector2Int.down * 2,
        Vector2Int.left * 2,
        Vector2Int.right * 2
        };

        System.Random rng = new();

        while (stack.Count > 0)
        {
            Vector2Int current = stack.Pop();
            directions = directions.OrderBy(d => rng.Next()).ToArray();

            foreach (Vector2Int dir in directions)
            {
                Vector2Int next = current + dir;

                Vector2Int between = current + dir / 2;

                if (!roomPositions.Contains(next) || !roomPositions.Contains(between) || visited.Contains(next))
                    continue;

                visited.Add(between);
                visited.Add(next);
                stack.Push(next);
            }
        }

        foreach (Vector2Int pos in roomPositions)
        {
            if (visited.Contains(pos))
            {
                PaintSingleTile(floorTileMap, floorTile, pos);
            }
            else
            {
                PaintSingleTile(floorTileMap, wallTile, pos);
            }
        }

        EnsurePlayability(cellSize, roomCenter, corridoorDirections, false);
    }

    public void DrawWallRoom(HashSet<Vector2Int> roomPositions, Vector2Int roomCenter, int cellSize, List<Directions> corridorDirections)
    {
        ClearTiles();

        int wallLength = 2;
        int spacing = 3;

        System.Random rng = new();

        foreach (Vector2Int pos in roomPositions)
        {
            if ((pos.y - roomPositions.Min(p => p.y)) % (wallLength + spacing) == 0)
            {

                bool canPlace = true;
                for (int i = 0; i < wallLength; i++)
                {
                    Vector2Int wallPos = new(pos.x + i, pos.y);
                    if (!roomPositions.Contains(wallPos)) canPlace = false;
                }

                if (canPlace)
                {
                    int doorIndex = rng.Next(0, wallLength);
                    for (int i = 0; i < wallLength; i++)
                    {
                        if (i == doorIndex) continue;
                        Vector2Int wallPos = new(pos.x + i, pos.y);
                        PaintSingleTile(floorTileMap, wallTile, wallPos);
                    }
                }
            }

            if ((pos.x - roomPositions.Min(p => p.x)) % (wallLength + spacing) == 0)
            {
                bool canPlace = true;
                for (int i = 0; i < wallLength; i++)
                {
                    Vector2Int wallPos = new(pos.x, pos.y + i);
                    if (!roomPositions.Contains(wallPos)) canPlace = false;
                }

                if (canPlace)
                {
                    int doorIndex = rng.Next(0, wallLength);
                    for (int i = 0; i < wallLength; i++)
                    {
                        if (i == doorIndex) continue;
                        Vector2Int wallPos = new(pos.x, pos.y + i);
                        PaintSingleTile(floorTileMap, wallTile, wallPos);
                    }
                }
            }
        }

        EnsurePlayability(cellSize, roomCenter, corridorDirections, false);
    }

    public void DrawWaterRoom(HashSet<Vector2Int> positions, DrawTileType tileType, int size, Vector2Int roomCenter)
    {
        ClearTiles();

        TileBase tile = GetTileBase(tileType);

        int halfSize = size / 2;
        int minX = roomCenter.x - halfSize;
        int maxX = roomCenter.x + halfSize;
        int minY = roomCenter.y - halfSize;
        int maxY = roomCenter.y + halfSize;

        HashSet<Vector2Int> edgePositions = new();

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                bool isEdge = (x - minX < 2) || (maxX - x < 2) || (y - minY < 2) || (maxY - y < 2);

                Vector2Int pos = new(x, y);
                if (isEdge && positions.Contains(pos))
                {
                    edgePositions.Add(pos);
                }
            }
        }

        PaintWaterRoomTiles(edgePositions, floorTileMap, tile);
    }

    public void DrawXRoom(HashSet<Vector2Int> roomPositions, int size, Vector2Int roomCenter, int cellSize, List<Directions> directions)
    {
        ClearTiles();
        foreach (Vector2Int pos in roomPositions)
        {
            PaintSingleTile(floorTileMap, abyssTile, pos);
        }
        EnsurePlayability(cellSize, roomCenter, directions, true);
    }

    public void DrawWeirdXRoom(HashSet<Vector2Int> roomPositions, int size, Vector2Int roomCenter)
    {
        ClearTiles();
        int halfSize = (int)MathF.Ceiling(size * 0.5f);
        HashSet<Vector2Int> path = new();

        for (int x = 0; x < halfSize; x++)
        {

            path.Add(new Vector2Int(roomCenter.x - x, roomCenter.y + x));
            path.Add(new Vector2Int(roomCenter.x + x, roomCenter.y - x));

            path.Add(new Vector2Int(roomCenter.x - x, roomCenter.y - x));
            path.Add(new Vector2Int(roomCenter.x + x, roomCenter.y + x));

            path.Add(new Vector2Int(roomCenter.x, roomCenter.y + x));
            path.Add(new Vector2Int(roomCenter.x, roomCenter.y - x));
            path.Add(new Vector2Int(roomCenter.x + x, roomCenter.y));
            path.Add(new Vector2Int(roomCenter.x - x, roomCenter.y));
        }

        foreach (Vector2Int pos in roomPositions)
        {
            PaintSingleTile(floorTileMap, waterTile, pos);
        }

        foreach (Vector2Int pos in path)
        {
            PaintSingleTile(floorTileMap, floorTile, pos);
        }
    }

    public bool EnsureWaterMargins(int islandSize, Vector2Int islandStartPosition)
    {
        // Adds island tiles and its surrounding tiles to islandPositions
        List<Vector2Int> islandPositions = new();
        for (int i = -1; i < islandSize+1; i++)
        {
            for (int j = -1; j < islandSize+1; j++)
            {
                islandPositions.Add(new Vector2Int(islandStartPosition.x + i, islandStartPosition.y + j));
            }
        }

        bool isPaintable = true;
        foreach (Vector2Int position in islandPositions)
        {
            if (floorTileMap.GetTile((Vector3Int) position) != waterTile)
            {
                isPaintable = false;
                break;
            }
        }
        return isPaintable;
    }

    public void DrawIslandRoom(HashSet<Vector2Int> roomPositions, int size, Vector2Int roomCenter, int cellSize, List<Directions> directions)
    {
        ClearTiles();

        foreach (Vector2Int pos in roomPositions)
        {
            PaintSingleTile(floorTileMap, waterTile, pos);
        }
        
        //Ensure level is playable
        EnsurePlayability(cellSize, roomCenter, directions, true);

        // Calculate room edges
        int halfSize = size / 2;
        int minX = roomCenter.x - halfSize;
        int maxX = roomCenter.x + halfSize;
        int minY = roomCenter.y - halfSize;
        int maxY = roomCenter.y + halfSize;

        HashSet<Vector2Int> edgePositions = new();

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                bool isEdge = (x - minX < 2) || (maxX - x < 2) || (y - minY < 2) || (maxY - y < 2);

                Vector2Int pos = new(x, y);
                if (isEdge && roomPositions.Contains(pos))
                {
                    edgePositions.Add(pos);
                }
            }
        }

        // Paint water zone around edges
        PaintTiles(edgePositions, waterTile);

        List<Vector2Int> shufflePositions = roomPositions.ToList();

        shufflePositions.Shuffle();

        foreach(Vector2Int pos in shufflePositions)
        {
            int islandSize = UnityEngine.Random.Range(2,5);
            if (EnsureWaterMargins(islandSize, pos))
            {
                HashSet<Vector2Int> islandPositions = new();
                for (int i = 0; i < islandSize; i++)
                {
                    for (int j = 0; j < islandSize; j++)
                    {
                        islandPositions.Add(new Vector2Int(pos.x + i, pos.y + j));
                    }
                }
                PaintTiles(islandPositions, floorTile);
            }
        }
    }


    private TileBase GetTileBase(DrawTileType tileType)
    {
        return tileType switch
        {
            DrawTileType.Floor => floorTile,
            DrawTileType.Water => waterTile,
            DrawTileType.Abyss => abyssTile,
            _ => wallTile,
        };
    }

    private void PaintWaterRoomTiles(HashSet<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var pos in positions)
        {
            if (!painted.Contains(pos))
            {
                PaintSingleTile(tilemap, tile, pos);
                painted.Add(pos);
            }
        }
    }

    private void PaintTiles(HashSet<Vector2Int> positions, TileBase tile)
    {
        foreach (Vector2Int pos in positions)
        {
            PaintSingleTile(floorTileMap, tile, pos);
        }
    }


    private void CreateBufferZone(Vector2Int position, int xSize, int ySize)
    {
        for (int bx = -1; bx <= xSize; bx++)
        {
            for (int by = -1; by <= ySize; by++)
            {
                Vector2Int bufferPos = new(position.x + bx, position.y + by);
                painted.Add(bufferPos);
            }
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    private void ClearTiles()
    {
        if (clearTiles)
        {
            floorTileMap.ClearAllTiles();
            painted.Clear();
            painted.Add(Vector2Int.zero);
            clearTiles = false;
        }
    }

    private void EnsurePlayability(int cellSize, Vector2Int roomCenter, List<Directions> directions, bool fillWidth)
    {
        ClearTiles();

        int halfSize = (int)MathF.Ceiling(cellSize * 0.5f);
        HashSet<Vector2Int> path = new();

        foreach (Directions dir in directions)
        {
            Vector2Int vectorDir = DirectionToVector(dir);

            Vector2Int perp = new(-vectorDir.y, vectorDir.x);

            for (int i = 0; i < halfSize; i++)
            {
                Vector2Int newPos = roomCenter + vectorDir * i;
                path.Add(newPos);

                if (fillWidth)
                {
                    Vector2Int newPos1 = newPos + perp;
                    Vector2Int newPos2 = newPos - perp;
                    path.Add(newPos1);
                    path.Add(newPos2);
                }
            }
        }

        foreach (Vector2Int pos in path)
        {
            PaintSingleTile(floorTileMap, floorTile, pos);
        }
    }

    private Vector2Int DirectionToVector(Directions direction)
    {
        return direction switch
        {
            Directions.North => Vector2Int.up,
            Directions.South => Vector2Int.down,
            Directions.East => Vector2Int.right,
            Directions.West => Vector2Int.left,
            _ => Vector2Int.zero
        };
    }
}
