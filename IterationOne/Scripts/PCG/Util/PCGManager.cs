using UnityEngine;
using System.Collections.Generic;
public class PCGManager : MonoBehaviour
{
    [SerializeField]
    private List<int> sizeList;
    [SerializeField]
    private int cellSize;
    [SerializeField]
    private BasicRoomGenerator basicRoomGenerator;

    private readonly HashSet<Vector2Int> occupiedCells = new();
    private readonly List<RoomInfo> placedRooms = new();

    private void Awake()
    {
        Vector2Int currentCell = Vector2Int.zero;
        occupiedCells.Add(currentCell);

        RoomInfo firstRoom = new()
        {
            Cell = currentCell,
            InDirections = new List<Directions>(),
            OutDirections = new List<Directions>()
        };

        placedRooms.Add(firstRoom);

        for (int i = 1; i < sizeList.Count; i++)
        {
            bool placed = false;
            int maxAttempts = 10;
            int attempt = 0;

            while (!placed && attempt < maxAttempts)
            {
                Directions dir = DirectionsExtensions.GetRandom();
                Vector2Int offset = dir.GetOffset();
                Vector2Int nextCell = currentCell + offset;

                if (!occupiedCells.Contains(nextCell))
                {
                    RoomInfo newRoom = new()
                    {
                        Cell = nextCell,
                        InDirections = new List<Directions> { dir.GetOpposite() },
                        OutDirections = new List<Directions>()
                    };

                    placedRooms[^1].OutDirections.Add(dir);

                    placedRooms.Add(newRoom);
                    occupiedCells.Add(nextCell);
                    currentCell = nextCell;

                    placed = true;
                }

                attempt++;
            }

            if (!placed)
            {
                Debug.LogWarning($"Room {i} skipped — no available direction.");
            }
        }

        for (int i = 0; i < placedRooms.Count; i++)
        {
            Vector2Int worldPos = placedRooms[i].Cell * cellSize;
            int size = sizeList[i];
            bool isLastRoom = i == placedRooms.Count - 1;
            bool isFirstRoom = i == 0;
            basicRoomGenerator.DrawRoom(size, placedRooms[i].InDirections, placedRooms[i].OutDirections, worldPos, cellSize, isLastRoom, isFirstRoom);
        }
    }
}

