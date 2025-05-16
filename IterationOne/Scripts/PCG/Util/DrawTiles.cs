using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawTiles : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTileMap;
    [SerializeField]
    private TileBase floorTile;
    [SerializeField]
    private TileBase wallTile;
    [SerializeField]
    private TileBase abyssTile;
    [SerializeField]
    [Range(0, 1000)]
    private int abyssPercentage;
    [SerializeField]
    private int pitSizeX;
    [SerializeField]
    private int pitSizeY;

    [SerializeField]
    private TileBase waterTile;
    [SerializeField]
    [Range(0, 1000)]
    private int waterPercentage;
    [SerializeField]
    private bool clearTiles = true;

    private readonly HashSet<Vector2Int> painted = new(); // Inaccessible tiles


    public void DrawRoom(HashSet<Vector2Int> positions, DrawTileType tileType)
    {
        if (clearTiles)
        {
            ClearTiles();
            clearTiles = false;
        }
        TileBase tile = GetTileBase(tileType);
        PaintTiles(positions, floorTileMap, tile);
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

private void PaintTiles(HashSet<Vector2Int> positions, Tilemap tilemap, TileBase tile)
{
    bool isAbyssTile = tile == abyssTile;
    bool isWaterTile = tile == waterTile;
    bool isBaseTile = tile == floorTile || tile == wallTile;

    foreach (var position in positions)
    {
        int random = Random.Range(1, 1001);

        if ((isAbyssTile && random < abyssPercentage) || (isWaterTile && random < waterPercentage))
        {
            int xSize = pitSizeX;
            int ySize = pitSizeY;
            if (Random.Range(0, 2) == 0)
            {
                xSize = pitSizeY;
                ySize = pitSizeX;
            }

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    Vector2Int tilePos = new(position.x + x, position.y + y);

                    if (positions.Contains(tilePos) && !painted.Contains(tilePos))
                    {
                        PaintSingleTile(tilemap, tile, tilePos);
                        painted.Add(tilePos);
                    }
                }
            }
            CreateBufferZone(position, xSize, ySize);
        }
        else if (isBaseTile)
        {
            PaintSingleTile(tilemap, tile, position);
        }
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
        floorTileMap.ClearAllTiles();
        painted.Clear();
        painted.Add(Vector2Int.zero);
    }
}
