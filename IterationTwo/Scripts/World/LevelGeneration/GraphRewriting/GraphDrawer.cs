#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GraphDrawer : MonoBehaviour
{
    public DungeonGraph graph;
    public Color anyColor = Color.red;
    public Color emptyColor = Color.white;
    public Color roomColor = Color.green;
    public Color corridorColor = Color.black;
    public Color startColor = Color.yellow;
    public Color endColor = Color.cyan;
    public Color edgeColor = Color.blue;
    public float nodeSize = 0.5f;

    public int maxTriesToApplyRule = 10;

    public DungeonGraphGrammarRule rule;

    public RuleDefinition ruleDefinition;
    public DungeonGraphGrammar ExitGrammar;
    public DungeonGraphGrammar BranchGrammar;
    public DungeonGraphGrammar ReplaceGrammar;

    public int distanceToExit = 10;
    public int numberOfRooms = 20;
    public int numberOfSpecialRooms = 10;

    private void Start ()
    {
        // Initialize the graph with some nodes and edges for demonstration purposes
        InitializeGraph();
    }

    [ContextMenu("Reset Graph")]
    private void InitializeGraph()
    {
        graph = new DungeonGraph(new List<DungeonNode>(), new List<DungeonEdge>());
        rule = null;
        // Example nodes
        var nodeA = new DungeonNode(new DungeonEntranceCell(), new Vector2(0, 0));
        var nodeB = new DungeonNode(new DungeonExitCell(), new Vector2(0, 1));

        // Example edges
        var edgeAB = new DungeonEdge(nodeA, nodeB);

        // Add nodes and edges to the graph
        graph.AddNode(nodeA);
        graph.AddNode(nodeB);
        graph.AddEdge(edgeAB);

        // var RnodeA = new DungeonNode(new DungeonCell(DungeonCell.CellType.Any), new Vector2(0, 0));
        // var RnodeB = new DungeonNode(new DungeonCell(DungeonCell.CellType.Any), new Vector2(0, 1));
        // var RnodeC = new DungeonNode(new DungeonCell(DungeonCell.CellType.Any), new Vector2(1, 1));
        // var RnodeD = new DungeonNode(new DungeonCell(DungeonCell.CellType.Any), new Vector2(1, 2));

        // var RnodeCEmpty = new DungeonNode(new DungeonCell(DungeonCell.CellType.Empty), new Vector2(1, 1));
        // var RnodeDEmpty = new DungeonNode(new DungeonCell(DungeonCell.CellType.Empty), new Vector2(1, 2));

        // var RedgeAB = new DungeonEdge(RnodeA, RnodeB);

        // DungeonGraph L = new DungeonGraph(new HashSet<DungeonNode> { RnodeA, RnodeB, RnodeCEmpty, RnodeDEmpty }, 
        //                                     new HashSet<DungeonEdge> { RedgeAB });
        // DungeonGraph R = new DungeonGraph(new HashSet<DungeonNode> { RnodeA, RnodeB, RnodeC, RnodeD }, 
        //                                     new HashSet<DungeonEdge> { RedgeAB, 
        //                                     new DungeonEdge(RnodeB, RnodeC), 
        //                                     new DungeonEdge(RnodeC, RnodeD) });
        //rule = new DungeonGraphGrammarRule(L, R);
    }

    // private void OnValidate()
    // {
    //     InitializeGraph();
    // }

    [ContextMenu("Apply Rule")]
    private void ApplyRuleFromInspector()
    {
        rule = ruleDefinition.GetRule();
        rule.ApplyRule(graph);
    }

    [ContextMenu("Apply Graph Grammar")]
    private void ApplyGraphGrammarFromInspector()
    {
        for (int i = 0; i < maxTriesToApplyRule; i++)
        {
            rule = ExitGrammar.GetRandomRule();
            if (rule.ApplyRule(graph)) break;
        }
    }

    [ContextMenu("Rewrite Graph")]
    private void RewriteGraphFromInspector()
    {
        try
        {
            var rewriter = new GraphRewriter(graph, ExitGrammar);
            Debug.Log("Applying Exit Grammar");
            rewriter.RewriteGraph(distanceToExit, 100);
            rewriter.Grammar = BranchGrammar;
            Debug.Log("Applying Branch Grammar");
            rewriter.RewriteGraph(numberOfRooms, 100);
            rewriter.Grammar = ReplaceGrammar;
            Debug.Log("Applying Replace Grammar");
            rewriter.RewriteGraph(numberOfRooms*2, numberOfSpecialRooms);
            graph = rewriter.Graph;
        } catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            InitializeGraph();
        }
    }

    private void OnDrawGizmos()
    {
        if (graph == null) return;

        DrawNodes(graph);
        DrawEdges(graph);
        if (rule == null) return;
        DrawNodes(rule.L, new Vector2(2, 0));
        DrawEdges(rule.L, new Vector2(2, 0));
        DrawNodes(rule.R, new Vector2(4, 0));
        DrawEdges(rule.R, new Vector2(4, 0));
    }

    private void DrawNodes(DungeonGraph drawnGraph, Vector2 offset = default)
    {
        foreach (var node in drawnGraph.Nodes)
        {
            if (node.Cell is DungeonEntranceCell)
            {
                Handles.color = startColor;
            }
            else if (node.Cell is DungeonExitCell)
            {
                Handles.color = endColor;
            }
            else if (node.Cell is DungeonCorridorCell)
            {
                Handles.color = corridorColor;
            }
            else if (node.Cell is DungeonWaterCell)
            {
                Handles.color = Color.gray;
            }
            else if (node.Cell is DungeonIslandCell)
            {
                Handles.color = Color.blue;
            }
            else if (node.Cell is DungeonXCell)
            {
                Handles.color = Color.red;
            }
            else if (node.Cell is DungeonMazeCell)
            {
                Handles.color = Color.magenta;
            }
            else if (node.Cell is DungeonTrapCell)
            {
                Handles.color = Color.yellow;
            }
            else if (node.Cell is DungeonLootCell)
            {
                Handles.color = Color.cyan;
            }
            else if (node.Cell is DungeonWallCell)
            {
                Handles.color = Color.black;
            }
            else if (node.Cell is EmptyCell)
            {
                Handles.color = emptyColor;
            }
            else if (node.Cell is DungeonCell)
            {
                Handles.color = roomColor;
            }
            else
            {
                Handles.color = Color.white;
            }
            Handles.DrawSolidDisc(node.Position + offset, Vector3.forward, nodeSize);
            Handles.Label(node.Position + offset, node.Cell.GetType().Name);
        }
    }

    private void DrawEdges(DungeonGraph drawnGraph, Vector2 offset = default)
    {
        Handles.color = edgeColor;
        foreach (var edge in drawnGraph.Edges)
        {
            Handles.DrawLine(edge.From.Position + offset, edge.To.Position + offset);
        }
    }
}
#endif