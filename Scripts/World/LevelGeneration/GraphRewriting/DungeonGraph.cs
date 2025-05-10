
using System.Collections.Generic;
using UnityEngine;

public class DungeonGraph
{
    public List<DungeonNode> Nodes { get; private set; } = new ();
    public List<DungeonEdge> Edges { get; private set; } = new ();

    public DungeonGraph(List<DungeonNode> nodes, List<DungeonEdge> edges) {
        Nodes = nodes;
        Edges = edges;
    }

    public DungeonGraph(DungeonGraph graph)
    {
        // Create a mapping from original nodes to their copies
        var nodeMapping = new Dictionary<DungeonNode, DungeonNode>();

        // Copy all nodes and store their mapping
        foreach (var node in graph.Nodes)
        {
            var copiedNode = new DungeonNode(node);
            Nodes.Add(copiedNode);
            nodeMapping[node] = copiedNode;
        }

        // Copy all edges and update references to the copied nodes
        foreach (var edge in graph.Edges)
        {
            var copiedEdge = new DungeonEdge(
                nodeMapping[edge.From], // Use the copied "From" node
                nodeMapping[edge.To]   // Use the copied "To" node
            );
            Edges.Add(copiedEdge);
        }
    }

    public DungeonGraph() { }

    public void AddNode(DungeonNode node)
    {
        Nodes.Add(node);
    }

    public void AddEdge(DungeonEdge edge)
    {
        Edges.Add(edge);
    }

    public void AddOrReplaceNode(DungeonNode node)
    {
        List<DungeonNode> nodesToRemove = new List<DungeonNode>();
        foreach (var existingNode in Nodes)
        {
            if (existingNode.Position == node.Position)
            {
                foreach (var edge in Edges)
                {
                    edge.From = edge.From == existingNode ? node : edge.From;
                    edge.To = edge.To == existingNode ? node : edge.To;
                }
                nodesToRemove.Add(existingNode);
            }
        }
        foreach (var existingNode in nodesToRemove)
        {
            Nodes.Remove(existingNode);
        }
        AddNode(node);
    }

    public void RemoveNode(DungeonNode node)
    {
        Nodes.Remove(node);
        foreach (var edge in Edges)
        {
            if (edge.From == node || edge.To == node)
            {
                Edges.Remove(edge);
            }
        }
    }

    public void RemoveEdge(DungeonEdge edge)
    {
        Edges.Remove(edge);
    }

    public override string ToString()
    {
        string result = "DungeonGraph:\n";
        foreach (var node in Nodes)
        {
            result += $"Node: {node.Position}, Type: {node.Cell}\n";
        }
        foreach (var edge in Edges)
        {
            result += $"Edge: {edge.From.Position} -> {edge.To.Position}\n";
        }
        return result;
    }
}