using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectPlacement : MonoBehaviour
{
    [SerializeField]
    private List<ObjectPlacementSO> gameObjects = new();
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private TileBase abyssTile;
    [SerializeField]
    private TileBase waterTile;

    private readonly HashSet<Vector3Int> occupiedPositions = new();

    public void PlaceObjects(HashSet<Vector2Int> floorPositions)
    {

        foreach (var obj in gameObjects)
        {
            int spawned = 0;
            foreach (var position in floorPositions)
            {
                int random = Random.Range(0, 10000);
                if (obj.HasSpawnIteration)
                {
                    if (random <= obj.SpawnChance && spawned < obj.MaxSpawnAmount && obj.SpawnIteration == CurrentIterationManager.Instance.CurrentIteration)
                    {
                        Vector3Int tilePos = new(position.x, position.y, 0);
                        if (!((tilemap.GetTile(tilePos) == abyssTile) || tilemap.GetTile(tilePos) == waterTile) || occupiedPositions.Contains(tilePos))
                        {
                            Instantiate(obj.Prefab, new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
                            occupiedPositions.Add(tilePos);
                            spawned++;
                        }
                    }
                }
                else
                {
                    if (random <= obj.SpawnChance && spawned < obj.MaxSpawnAmount)
                    {
                        Vector3Int tilePos = new(position.x, position.y, 0);
                        if (!((tilemap.GetTile(tilePos) == abyssTile) || tilemap.GetTile(tilePos) == waterTile) || occupiedPositions.Contains(tilePos))
                        {
                            Instantiate(obj.Prefab, new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
                            occupiedPositions.Add(tilePos);
                            spawned++;
                        }
                    }
                }

            }
        }
    }

    public void PlaceObject(HashSet<Vector2Int> floorPositions, ObjectPlacementSO obj)
    {
        int spawned = 0;
        while (spawned < obj.SpawnAmount)
        {
            foreach (var position in floorPositions)
            {
                int random = Random.Range(0, 1000);
                if (random <= obj.SpawnChance && spawned < obj.SpawnAmount)
                {
                    Vector3Int tilePos = new(position.x, position.y, 0);
                    if (!((tilemap.GetTile(tilePos) == abyssTile) || tilemap.GetTile(tilePos) == waterTile))
                    {
                        Instantiate(obj.Prefab, new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
                        spawned++;
                    }
                }
            }
        }
    }
}