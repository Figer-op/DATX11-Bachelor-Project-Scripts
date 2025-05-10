using UnityEngine;

// contains rules and applies them to a graph
public class GraphRewriter
{
    public DungeonGraph Graph { get; private set; }
    public DungeonGraphGrammar Grammar { get; set; }

    public GraphRewriter(DungeonGraph graph, DungeonGraphGrammar grammar)
    {
        Graph = graph;
        Grammar = grammar;
    }

    public void RewriteGraph(int size, int matches)
    {

        int tries = 0;
        while (Graph.Nodes.Count < size && matches > 0)
        {
            tries++;
            var rule = Grammar.GetRandomRule();
            if (rule.ApplyRule(Graph))
            {
                matches--;
            }
            if (tries > 1000)
            {
                throw new System.Exception("Too many tries to rewrite, Stopping.");
            }
        }
    }
}