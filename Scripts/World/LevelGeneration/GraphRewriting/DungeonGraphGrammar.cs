using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DungeonGraphGrammar", menuName = "LevelGeneration/DungeonGraphGrammar")]
public class DungeonGraphGrammar : ScriptableObject
{
    [System.Serializable]
    public struct WeightedRule
    {
        public RuleDefinition rule;
        public float weight;

        public WeightedRule(RuleDefinition rule, float weight = 1f)
        {
            this.rule = rule;
            this.weight = weight;
        }

        public DungeonGraphGrammarRule GetRule()
        {
            return rule.GetRule();
        }
    }

    [field: SerializeField]
    public List<WeightedRule> Rules { get; private set; } = new ();

    public DungeonGraphGrammarRule GetRandomRule()
    {
        if (Rules.Count == 0)
        {
            Debug.LogError("No rules available in the graph grammar.");
            return null;
        }

        float totalWeight = 0f;
        foreach (var weightedRule in Rules)
        {
            totalWeight += weightedRule.weight;
        }

        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var weightedRule in Rules)
        {
            cumulativeWeight += weightedRule.weight;
            if (randomValue <= cumulativeWeight)
            {
                return weightedRule.GetRule();
            }
        }

        Debug.LogError("Failed to select a rule based on weights.");
        return null;
    }
}