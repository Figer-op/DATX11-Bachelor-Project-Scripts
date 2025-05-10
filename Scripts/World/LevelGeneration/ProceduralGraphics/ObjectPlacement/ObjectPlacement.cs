using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectPlacement : MonoBehaviour
{
    public static ObjectPlacement Instance { get; private set; }

    [SerializeField]
    private ObjectPlacementSO dungeonExit;

    [SerializeField]
    private ObjectPlacementSO trap;

    [SerializeField]
    private ObjectPlacementSO fireTrap;

    [SerializeField]
    private List<ObjectPlacementSO> gameObjects = new();
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private TileBase abyssTile;
    [SerializeField]
    private TileBase waterTile;
    [SerializeField]
    private TileBase wallTile;
    [SerializeField]
    private List<ObjectPlacementSO> bosses = new();
    [SerializeField]
    private List<ObjectPlacementSO> chests = new();

    private readonly HashSet<Vector3Int> occupiedPositions = new();

    private void OnEnable()
    {
        Instance = this;
    }

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
                        if (!((tilemap.GetTile(tilePos) == abyssTile) || tilemap.GetTile(tilePos) == waterTile || tilemap.GetTile(tilePos) == wallTile)
                            && !occupiedPositions.Contains(tilePos))
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
                        if (!((tilemap.GetTile(tilePos) == abyssTile) || tilemap.GetTile(tilePos) == waterTile || tilemap.GetTile(tilePos) == wallTile)
                            && !occupiedPositions.Contains(tilePos))
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

    public void PlaceExit(HashSet<Vector2Int> floorPositions)
    {
        int spawned = 0;
        while (spawned < dungeonExit.SpawnAmount)
        {
            foreach (var position in floorPositions)
            {
                int random = Random.Range(0, 1000);
                if (random <= dungeonExit.SpawnChance && spawned < dungeonExit.SpawnAmount)
                {
                    Vector3Int tilePos = new(position.x, position.y, 0);
                    if (!((tilemap.GetTile(tilePos) == abyssTile) || tilemap.GetTile(tilePos) == waterTile || tilemap.GetTile(tilePos) == wallTile)
                        && !occupiedPositions.Contains(tilePos))
                    {
                        Instantiate(dungeonExit.Prefab, new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
                        occupiedPositions.Add(tilePos);
                        spawned++;
                    }
                }
            }
        }
    }

    public void PlaceTrapRandom(HashSet<Vector2Int> floorPositions)
    {

        int spawned = 0;
        while (spawned < trap.SpawnAmount)
        {
            foreach (var position in floorPositions)
            {
                int random = Random.Range(0, 1000);
                if (random <= trap.SpawnChance && spawned < trap.SpawnAmount)
                {
                    Vector3Int tilePos = new(position.x, position.y, 0);
                    if (!((tilemap.GetTile(tilePos) == abyssTile) || tilemap.GetTile(tilePos) == waterTile || tilemap.GetTile(tilePos) == wallTile)
                      && !occupiedPositions.Contains(tilePos))
                    {
                        Instantiate(trap.Prefab, new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
                        occupiedPositions.Add(tilePos);
                        spawned++;
                    }
                }
            }
        }
    }

    public void PlaceFireTrapRandom(HashSet<Vector2Int> floorPositions)
    {
        int spawned = 0;
        while (spawned < fireTrap.SpawnAmount)
        {
            foreach (var position in floorPositions)
            {
                int random = Random.Range(0, 1000);
                if (random <= fireTrap.SpawnChance && spawned < fireTrap.SpawnAmount)
                {
                    Vector3Int tilePos = new(position.x, position.y, 0);
                    if (!((tilemap.GetTile(tilePos) == abyssTile) || tilemap.GetTile(tilePos) == waterTile || tilemap.GetTile(tilePos) == wallTile)
                        && !occupiedPositions.Contains(tilePos))
                    {
                        Quaternion rotation = GetRandomDirectionRotation();

                        Instantiate(fireTrap.Prefab, new Vector3(position.x + 0.5f, position.y + 0.5f, 0), rotation);

                        occupiedPositions.Add(tilePos);
                        spawned++;
                    }
                }
            }
        }
    }

    public void PlaceBoss(Vector2Int position)
    {
        foreach (var boss in bosses)
        {
            if (boss.SpawnIteration == CurrentIterationManager.Instance.CurrentIteration)
            {
                Instantiate(boss.Prefab, new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
            }
        }
    }

    public void PlaceChest(Vector2Int position)
    {
        foreach (var chest in chests)
        {
            if (chest.SpawnIteration == CurrentIterationManager.Instance.CurrentIteration)
            {
                Instantiate(chest.Prefab, new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
            }
        }
    }

    private Quaternion GetRandomDirectionRotation()
    {
        int direction = Random.Range(0, 4);
        return Quaternion.Euler(0, 0, direction * 90);
    }

}