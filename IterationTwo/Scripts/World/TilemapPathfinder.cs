using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class TilemapPathfinder
{
    public Tilemap tilemap { get; private set; }
    int GridRowMin => tilemap.cellBounds.yMin;
    int GridRowMax => tilemap.cellBounds.yMax;
    int GridColMin => tilemap.cellBounds.xMin;
    int GridColMax => tilemap.cellBounds.xMax;

    private class TileNode
    {
        public TileNode Parent { get; set; }
        public Vector3Int Position { get; private set; }
        public float DistanceToTarget { get; set; } = -1;
        public float Cost { get; set; } = 1;
        public float Weight { get; set; } = 1;
        public float F => DistanceToTarget != -1 && Cost != -1 ? DistanceToTarget + Cost : -1;
        public bool Walkable { get; private set; }

        public TileNode(Vector3Int pos, Tilemap tilemap, float weight = 1)
        {
            Parent = null;
            Position = pos;
            DistanceToTarget = -1;
            Cost = 1;
            Weight = weight;
            LevelRuleTile tile = tilemap.GetTile(tilemap.WorldToCell(pos)) as LevelRuleTile;
            Walkable = tile != null && tile.TileFlags.Contains(TileFlag.Walkable);
        }

        public override bool Equals(object obj)
        {
            return obj is TileNode node && Position.Equals(node.Position);
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }

    public TilemapPathfinder(Tilemap tilemap)
    {
        this.tilemap = tilemap;
    }

    public Stack<Vector3> FindPath(Vector3 start, Vector3 end)
    {
        TileNode startNode = new(tilemap.WorldToCell(start), tilemap);
        TileNode endNode = new(tilemap.WorldToCell(end), tilemap);

        PriorityQueue<TileNode, float> openList = new();
        HashSet<TileNode> closedList = new();

        TileNode result = ProcessNodes(openList, closedList, startNode, endNode);

        if (result == null || !closedList.Contains(endNode))
        {
            return null;
        }

        return ConstructPath(result, startNode);
    }

    private TileNode ProcessNodes(PriorityQueue<TileNode, float> openSet, HashSet<TileNode> closedSet, TileNode start, TileNode end)
    {
        openSet.Enqueue(start, start.F);
        TileNode current = null;
        while (openSet.Count != 0 && !closedSet.Contains(end))
        {
            current = openSet.Dequeue();
            closedSet.Add(current);
            List<TileNode> adjacencies = GetAdjacentNodes(current);

            foreach (TileNode n in adjacencies)
            {
                if (!closedSet.Contains(n) && n.Walkable)
                {
                    bool isFound = false;
                    foreach (var oLNode in openSet.UnorderedItems)
                    {
                        if (oLNode.Key == n)
                        {
                            isFound = true;
                        }
                    }
                    if (!isFound)
                    {
                        n.Parent = current;
                        n.DistanceToTarget = Math.Abs(n.Position.x - end.Position.x) + Math.Abs(n.Position.y - end.Position.y);
                        n.Cost = n.Weight + n.Parent.Cost;
                        openSet.Enqueue(n, n.F);
                    }
                }
            }
        }
        return current;
    }

    private Stack<Vector3> ConstructPath(TileNode current, TileNode start)
    {
        Stack<Vector3> path = new();
        TileNode temp = current;
        do
        {
            path.Push(tilemap.GetCellCenterWorld(temp.Position));
            temp = temp.Parent;
        } while (temp != start && temp != null);

        return path;
    }

    private List<TileNode> GetAdjacentNodes(TileNode n)
    {
        List<TileNode> temp = new();

        int row = n.Position.y;
        int col = n.Position.x;

        if (row + 1 < GridRowMax)
        {
            temp.Add(new TileNode(new Vector3Int(col, row + 1, 0), tilemap));
        }
        if (row - 1 >= GridRowMin)
        {
            temp.Add(new TileNode(new Vector3Int(col, row - 1, 0), tilemap));
        }
        if (col - 1 >= GridColMin)
        {
            temp.Add(new TileNode(new Vector3Int(col - 1, row, 0), tilemap));
        }
        if (col + 1 < GridColMax)
        {
            temp.Add(new TileNode(new Vector3Int(col + 1, row, 0), tilemap));
        }

        return temp;
    }
}