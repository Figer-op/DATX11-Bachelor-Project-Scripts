using System.Collections.Generic;
using UnityEngine;

public class LootTableChest : AbstractChest
{
    [SerializeField]
    private List<LootTable> lootTable = new List<LootTable>();

    [System.Serializable]
    private struct LootTable
    {
        public GameObject item;
        public float weight;
    }

    public override void ToggleOn()
    {
        base.ToggleOn();

        if (lootTable.Count == 0)
        {
            Debug.LogWarning("Loot table is empty!");
            return;
        }

        // Calculate the total weight
        float totalWeight = 0f;
        foreach (LootTable loot in lootTable)
        {
            totalWeight += loot.weight;
        }

        // Generate a random number between 0 and totalWeight
        float randomValue = Random.Range(0, totalWeight);

        // Determine which item to spawn
        float cumulativeWeight = 0f;
        foreach (LootTable loot in lootTable)
        {
            cumulativeWeight += loot.weight;
            if (randomValue <= cumulativeWeight)
            {
                Instantiate(loot.item, transform.position, Quaternion.identity);
                return;
            }
        }
    }
}
