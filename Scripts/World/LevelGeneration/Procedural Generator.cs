using System.Collections.Generic;
using UnityEngine;
public class ProceduralGenerator : MonoBehaviour
{

    [SerializeField]
    private DungeonGraphGrammar exitGrammar;

    [SerializeField]
    private DungeonGraphGrammar branchGrammar;

    [SerializeField]
    private DungeonGraphGrammar replaceGrammar;
    [SerializeField]
    private int distanceToExit = 10;
    [SerializeField]
    private int numberOfRooms = 20;
    [SerializeField]
    private int numberOfSpecialRooms = 15;

    private void Awake()
    {
        GenerateDungeon(GenerateDungeonGraph());
    }

    private void GenerateDungeon(DungeonGraph dungeonGraph)
    {
        List<DungeonNode> nodes = dungeonGraph.Nodes;
        List<DungeonEdge> edges = dungeonGraph.Edges;

        foreach (DungeonNode node in nodes)
        {
            List<Directions> directions = new();
            foreach (DungeonEdge edge in edges)
            {
                if (edge.From == node)
                {
                    var dir = edge.GetDirection();
                    if (dir.HasValue)
                        directions.Add(dir.Value);
                }
                else if (edge.To == node)
                {
                    var dir = edge.GetReverseDirection();
                    if (dir.HasValue)
                        directions.Add(dir.Value);
                }
            }

            node.Cell.GenerateRoom(node.Position, directions);
        }
    }  

    private DungeonGraph GenerateDungeonGraph()
    {
        DungeonGraph graph = InitializeGraph();
        while (true)
        {
            try
            {
                var rewriter = new GraphRewriter(graph, exitGrammar);
                Debug.Log("Applying Exit Grammar");
                rewriter.RewriteGraph(distanceToExit, 100);
                rewriter.Grammar = branchGrammar;
                Debug.Log("Applying Branch Grammar");
                rewriter.RewriteGraph(numberOfRooms, 100);
                rewriter.Grammar = replaceGrammar;
                Debug.Log("Applying Replace Grammar");
                rewriter.RewriteGraph(numberOfRooms*2, numberOfSpecialRooms);
                graph = rewriter.Graph;
                
                return graph;
            } catch (System.Exception e)
            {
                Debug.Log("Error: " + e.Message);
                Debug.Log("Retrying with a new graph.");
                graph = InitializeGraph(); // Reinitialize the graph
            }
        }
    }

    private DungeonGraph InitializeGraph()
    {
        var graph = new DungeonGraph(new List<DungeonNode>(), new List<DungeonEdge>());
        // Example nodes
        var nodeA = new DungeonNode(new DungeonEntranceCell(), new Vector2(0, 0));
        var nodeB = new DungeonNode(new DungeonExitCell(), new Vector2(0, 1));

        // Example edges
        var edgeAB = new DungeonEdge(nodeA, nodeB);

        // Add nodes and edges to the graph
        graph.AddNode(nodeA);
        graph.AddNode(nodeB);
        graph.AddEdge(edgeAB);

        return graph;
    }

    private DungeonGraph GenerateTestDungeonGraph()
    {
        var nodeA = new DungeonNode(new DungeonEntranceCell(), new Vector2(0, 0));
        var nodeB = new DungeonNode(new DungeonWaterCell(), new Vector2(0, 1));
        var nodeC = new DungeonNode(new DungeonCorridorCell(), new Vector2(1, 0));
        var nodeD = new DungeonNode(new DungeonExitCell(), new Vector2(1, 1));
        var nodeE = new DungeonNode(new DungeonXCell(), new Vector2(1, 2));
        var nodeF = new DungeonNode(new DungeonIslandCell(), new Vector2(2, 1));
        var nodeG = new DungeonNode(new DungeonWeirdXCell(), new Vector2(3, 1));
        var nodeH = new DungeonNode(new DungeonBossCell(), new Vector2(3, 0));
        var nodeJ = new DungeonNode(new DungeonLootCell(), new Vector2(4, 0));
        var nodeK = new DungeonNode(new DungeonCell(), new Vector2(4, 1));
        var nodeL = new DungeonNode(new DungeonMazeCell(), new Vector2(4, 2));
        var nodeM = new DungeonNode(new DungeonTrapCell(), new Vector2(4, -1));
        var nodeN = new DungeonNode(new DungeonWallCell(), new Vector2(4, -2));

        List<DungeonNode> nodes = new()
        {
            nodeA,
            nodeB,
            nodeC,
            nodeD,
            nodeE,
            nodeF,
            nodeG,
            nodeH,
            nodeJ,
            nodeK,
            nodeL,
            nodeM,
            nodeN

        };

        List<DungeonEdge> edges = new()
        {
            new DungeonEdge(nodeA, nodeB),
            new DungeonEdge(nodeA, nodeC),
            new DungeonEdge(nodeC, nodeD),
            new DungeonEdge(nodeB, nodeD),
            new DungeonEdge(nodeD, nodeE),
            new DungeonEdge(nodeD, nodeF),
            new DungeonEdge(nodeF, nodeG),
            new DungeonEdge(nodeG, nodeH),
            new DungeonEdge(nodeH, nodeJ),
            new DungeonEdge(nodeJ, nodeK),
            new DungeonEdge(nodeK, nodeL),
            new DungeonEdge(nodeJ, nodeM),
            new DungeonEdge(nodeM, nodeN),

        };

        return new DungeonGraph(nodes, edges);
    }
}