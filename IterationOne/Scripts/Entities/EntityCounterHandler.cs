using System.Collections.Generic;
using UnityEngine;

public class EntityCounterHandler: MonoBehaviour, IDataPersistence<GameData>
{
    private const FindObjectsSortMode noSort = FindObjectsSortMode.None;

    private PlayerBase playerBase;

    private EnemyBase[] enemiesInScene;
    private int enemyDeathCount;
    private int enemySpawnCount;

    private AbstractChest[] chestsInScene;
    private int chestSpawnCount;

    private HealthPotionInteraction[] healthPotionsInScene;
    private int healthPotionCount;

    private DungeonType currentDungeon;

    private void Awake()
    {
        playerBase = FindAnyObjectByType<PlayerBase>();
        currentDungeon = playerBase.CurrentDungeon;
    }

    private void Start()
    {
        enemiesInScene = FindObjectsByType<EnemyBase>(noSort);
        foreach (var enemyInScene in enemiesInScene)
        {
            enemyInScene.OnEnemyDeath += IncrementDeathCount;
            enemyInScene.OnEnemySpawned += IncrementSpawnCount;
        }

        chestsInScene = FindObjectsByType<AbstractChest>(noSort);
        foreach (var chestInScene in chestsInScene)
        {
            chestInScene.OnChestSpawned += IncrementChestSpawnCount;
        }

        healthPotionsInScene = FindObjectsByType<HealthPotionInteraction>(noSort);
        foreach (var healthPotionInScene in healthPotionsInScene)
        {
            healthPotionInScene.OnHealthPotionSpawned += IncrementHealthPotionCount;
        }
    }

    private void IncrementDeathCount()
    {
        enemyDeathCount++;
    }

    private void IncrementSpawnCount()
    {
        enemySpawnCount++;
    }

    private void IncrementChestSpawnCount()
    {
        chestSpawnCount++;
    }

    private void IncrementHealthPotionCount()
    {
        healthPotionCount++;
    }

    private void OnDestroy()
    {
        foreach (var enemyInScene in enemiesInScene)
        {
            if (enemyInScene == null)
            {
                continue;
            }
            enemyInScene.OnEnemyDeath -= IncrementDeathCount;
            enemyInScene.OnEnemySpawned -= IncrementSpawnCount;
        }

        foreach (var chestInScene in chestsInScene)
        {
            chestInScene.OnChestSpawned -= IncrementChestSpawnCount;
        }

        foreach (var healthPotionInScene in healthPotionsInScene)
        {
            if (healthPotionInScene == null)
            {
                continue;
            }
            healthPotionInScene.OnHealthPotionSpawned -= IncrementHealthPotionCount;
        }
    }

    public void LoadData(GameData data)
    {
        enemyDeathCount = data.DungeonData[currentDungeon].EnemyDeathCount;
        enemySpawnCount = data.DungeonData[currentDungeon].EnemySpawnCount;
        chestSpawnCount = data.DungeonData[currentDungeon].ChestSpawnCount;
        healthPotionCount = data.DungeonData[currentDungeon].HealthPotionCount;
    }

    public void SaveData(GameData data)
    {
        data.DungeonData[currentDungeon].EnemyDeathCount = enemyDeathCount;
        data.DungeonData[currentDungeon].EnemySpawnCount = enemySpawnCount;
        data.DungeonData[currentDungeon].ChestSpawnCount = chestSpawnCount;
        data.DungeonData[currentDungeon].HealthPotionCount = healthPotionCount;
    }
}
