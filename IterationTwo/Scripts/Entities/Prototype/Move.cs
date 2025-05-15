using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;

public class Move : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private Transform target;

    private TilemapPathfinder pathfinder;

    void Start()
    {
        pathfinder = new TilemapPathfinder(tilemap);
    }

    void Update()
    {
        Stack<Vector3> path = pathfinder.FindPath(transform.position, target.position);
        if (path != null)
        {
            string debug = "";
            foreach (Vector3 pos in path)
            {
                debug += pos + " ";
            }
            Debug.Log(debug);
            transform.position = Vector3.MoveTowards(transform.position, path.Peek(), Time.deltaTime * 4);
        }
    }
}
