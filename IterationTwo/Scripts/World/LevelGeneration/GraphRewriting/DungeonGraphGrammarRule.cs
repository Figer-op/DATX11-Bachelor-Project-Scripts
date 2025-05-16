using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class DungeonGraphGrammarRule
{
    public DungeonGraph L {get; private set; } = new ();
    public DungeonGraph R {get; private set; } = new ();

    public Dictionary<Vector2, Vector2> ReferenceMap { get; private set; } = new ();
    public DungeonGraphGrammarRule(DungeonGraph l, DungeonGraph r, Dictionary<Vector2, Vector2> referenceMap)
    {
        L = l;
        R = r;
        ReferenceMap = referenceMap;
    }

    private class DungeonGraphTransformation
    {
        public Vector2 PositionOffset { get; set; }
        public float RotationAngle { get; set; }
        public Vector2 MirrorAxis { get; set; }

        public DungeonGraphTransformation(Vector2 positionOffset, float rotationAngle, Vector2 mirrorAxis)
        {
            PositionOffset = positionOffset;
            RotationAngle = rotationAngle;
            MirrorAxis = mirrorAxis;
        }
    }

    public bool ApplyRule(DungeonGraph graph)
    {
        DungeonGraphTransformation transformation = FindMatchingTransformation(graph);
        if (transformation == null)
        {
            return false;
        }
        return ReplaceSubgraph(graph, transformation);
    }

    private DungeonGraphTransformation FindMatchingTransformation(DungeonGraph graph)
    {
        DungeonGraphTransformation transformation = new DungeonGraphTransformation(Vector2.zero, 0f, Vector2.one);
        DungeonNode subStartNode = L.Nodes.FirstOrDefault();
        var nodeList = graph.Nodes.ToList();
        nodeList = nodeList.OrderBy(_ => Random.value).ToList();
        var angles = new[] { 0f, 90f };
        angles = angles.OrderBy(_ => Random.value).ToArray();
        var axes = new[] { Vector2.one, new Vector2(-1, 1), new Vector2(1, -1), new Vector2(-1, -1) };
        axes = axes.OrderBy(_ => Random.value).ToArray();
        foreach (var node in nodeList)
        {
            if (node.Cell.Matches(subStartNode.Cell))
            {
                Vector2 offset = node.Position - subStartNode.Position;
                foreach (var angle in angles)
                {
                    foreach (var axis in axes)
                    {
                        if (CheckEdgeMatch(graph, offset, angle, axis))
                        {
                            return new DungeonGraphTransformation(offset, angle, axis);
                        }
                    }
                }
            }
        }
        return null;
    }

    private bool CheckEdgeMatch(DungeonGraph graph, Vector2 offset, float angle, Vector2 axis)
    {
        var transformedL = new DungeonGraph(L);
        RotateGraph(transformedL, angle);
        MirrorGraph(transformedL, axis);
        TranslateGraph(transformedL, offset);

        var transformedR = new DungeonGraph(R);
        RotateGraph(transformedR, angle);
        MirrorGraph(transformedR, axis);
        TranslateGraph(transformedR, offset);
        foreach (var edge in transformedL.Edges)
        {
            bool found = false;
            foreach (var graphEdge in graph.Edges)
            {
                if (graphEdge.Matches(edge))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                return false;
            }
        }
        foreach (var node in transformedR.Nodes)
        {
            foreach (var graphNode in graph.Nodes)
            {
                Vector2 reverseTransformedPosition = Round(Quaternion.Euler(0, 0, -angle) * (node.Position - offset) * new Vector2(axis.x, axis.y));
                if (node.Position == graphNode.Position && !node.CanReplace(graphNode) && !ReferenceMap.ContainsKey(reverseTransformedPosition))
                {
                    return false;
                }
            }
        }
        foreach (var node in transformedL.Nodes)
        {
            if (node.Cell is EmptyCell) // Check if node.Cell is of type EmptyCell
            {
                foreach (var graphNode in graph.Nodes)
                {
                    if (graphNode.Position == node.Position)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private bool ReplaceSubgraph(DungeonGraph graph, DungeonGraphTransformation transformation)
    {
        var transformedR = new DungeonGraph(R);
        RotateGraph(transformedR, transformation.RotationAngle);
        MirrorGraph(transformedR, transformation.MirrorAxis);
        TranslateGraph(transformedR, transformation.PositionOffset);
        foreach (var node in transformedR.Nodes)
        {
            graph.AddOrReplaceNode(node);
        }
        foreach (var edge in transformedR.Edges)
        {
            graph.AddEdge(edge);
        }

        return true;
    }

    private void TranslateGraph(DungeonGraph graph, Vector2 offset)
    {
        foreach (var node in graph.Nodes)
        {
            node.Position = Round(node.Position + offset);
        }
    }

    private void RotateGraph(DungeonGraph graph, float angle)
    {
        foreach (var node in graph.Nodes)
        {
            node.Position = Round(Quaternion.Euler(0, 0, angle) * node.Position);
        }
    }

    private void MirrorGraph(DungeonGraph graph, Vector2 axis)
    {
        foreach (var node in graph.Nodes)
        {
            node.Position = Round(new Vector2(node.Position.x * axis.x, node.Position.y * axis.y));
        }
    }

    private Vector2 Round(Vector2 vector)
    {
        return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
    }
}