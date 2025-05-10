using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuleDefinition", menuName = "LevelGeneration/RuleDefinition")]
public class RuleDefinition : ScriptableObject
{
    public enum CellTypes
    {
        Empty,
        Any,
        Room,
        Water,
        Island,
        Loot,
        Maze,
        Trap,
        Wall,
        X,
        Corridor,
        Entrance,
        Exit,
        Boss
    }

    public static DungeonCell CreateCellInstance(CellTypes cellType)
    {
        switch (cellType)
        {
            case CellTypes.Empty:
                return new EmptyCell();
            case CellTypes.Any:
                return new DungeonCell();
            case CellTypes.Room:
                return new DungeonCell();
            case CellTypes.Water:
                return new DungeonWaterCell();
            case CellTypes.Island:
                return new DungeonIslandCell();
            case CellTypes.Loot:
                return new DungeonLootCell();
            case CellTypes.Maze:
                return new DungeonMazeCell();
            case CellTypes.Trap:
                return new DungeonTrapCell();
            case CellTypes.Wall:
                return new DungeonWallCell();
            case CellTypes.X:
                return new DungeonXCell();
            case CellTypes.Corridor:
                return new DungeonCorridorCell();
            case CellTypes.Entrance:
                return new DungeonEntranceCell();
            case CellTypes.Exit:
                return new DungeonExitCell();
            case CellTypes.Boss:
                return new DungeonBossCell();
            default:
                Debug.LogError($"Unknown CellType: {cellType}");
                return null;
        }
    }

    [System.Serializable]
    public struct RuleEdgeData
    {
        public Vector2Int from;
        public Vector2Int to;

        public RuleEdgeData(Vector2Int from, Vector2Int to)
        {
            this.from = from;
            this.to = to;
        }
    }

    [System.Serializable]
    public struct RuleNodeData
    {
        public Vector2Int position;
        public CellTypes type;

        public RuleNodeData(Vector2Int position, CellTypes type)
        {
            this.position = position;
            this.type = type;
        }
    }

    [System.Serializable]
    public struct GraphMatchRuleData
    {
        public List<RuleNodeData> nodes;
        public List<RuleEdgeData> edges;
    }

    [Serializable]
    public struct ReferenceMapping
    {
        public Vector2Int key;
        public Vector2Int value;

        public ReferenceMapping(Vector2Int key, Vector2Int value)
        {
            this.key = key;
            this.value = value;
        }
    }

    public GraphMatchRuleData L;
    public GraphMatchRuleData R;

    public List<ReferenceMapping> ReferenceMap = new ();

    private DungeonGraph ToGraph(GraphMatchRuleData ruleGraph)
    {
        DungeonGraph graph = new DungeonGraph();
        // Create a mapping from original nodes to their copies
        var nodeMapping = new Dictionary<Vector2Int, DungeonNode>();

        // Copy all nodes and store their mapping
        foreach (var node in ruleGraph.nodes)
        {
            var cellInstance = CreateCellInstance(node.type);

            if (cellInstance != null)
            {
                var copiedNode = new DungeonNode(cellInstance, node.position);
                graph.AddNode(copiedNode);
                nodeMapping[node.position] = copiedNode;
            }
            else
            {
                Debug.LogError($"Failed to create instance for type {node.type}");
            }
        }

        // Copy all edges and update references to the copied nodes
        foreach (var edge in ruleGraph.edges)
        {
            var copiedEdge = new DungeonEdge(
                nodeMapping[edge.from], // Use the copied "From" node
                nodeMapping[edge.to]   // Use the copied "To" node
            );
            graph.AddEdge(copiedEdge);
        }
        return graph;
    }

    public DungeonGraph GetLGraph()
    {
        return ToGraph(L);
    }

    public DungeonGraph GetRGraph()
    {
        return ToGraph(R);
    }

    public Dictionary<Vector2, Vector2> GetReferenceMap()
    {
        Dictionary<Vector2, Vector2> referenceMap = new ();
        foreach (var mapping in ReferenceMap)
        {
            referenceMap[mapping.key] = mapping.value;
        }
        return referenceMap;
    }

    public DungeonGraphGrammarRule GetRule()
    {
        return new DungeonGraphGrammarRule(GetLGraph(), GetRGraph(), GetReferenceMap());
    }
}