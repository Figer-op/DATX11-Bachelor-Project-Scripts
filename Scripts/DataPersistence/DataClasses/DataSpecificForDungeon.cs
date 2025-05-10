using UnityEngine.AddressableAssets;
using UnityEngine;

[System.Serializable]
public class DataSpecificForDungeon
{
    [SerializeField]
    private float currentHealth = 100f;
    public float CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }

    [SerializeField]
    private AssetReference weaponBase = new("Assets/Main/Prefabs/Entities/Items/Weapons/PlayerWoodenSword.prefab");
    public AssetReference WeaponBase { get { return weaponBase; } set { weaponBase = value; } }

    [SerializeField]
    private bool hasVisited = false;
    public bool HasVisited { get => hasVisited; set => hasVisited = value; }

    [SerializeField]
    private int amountOfDeaths = 0;
    public int AmountOfDeaths { get => amountOfDeaths; set => amountOfDeaths = value; }

    [SerializeField]
    private float timestamp = 0f;
    public float Timestamp { get => timestamp; set => timestamp = value; }

    [SerializeField]
    private SerializableDictionary<string, WeaponEquipTimes> equippedWeapons = new();
    public SerializableDictionary<string, WeaponEquipTimes> EquippedWeapons { get => equippedWeapons; set => equippedWeapons = value; }

    [SerializeField]
    private int enemyDeathCount = 0;
    public int EnemyDeathCount { get => enemyDeathCount; set => enemyDeathCount = value; }

    [SerializeField]
    private int enemySpawnCount = 0;
    public int EnemySpawnCount { get => enemySpawnCount; set => enemySpawnCount = value; }

    [SerializeField]
    private int chestSpawnCount = 0;
    public int ChestSpawnCount { get => chestSpawnCount; set => chestSpawnCount = value; }

    [SerializeField]
    private int healthPotionCount = 0;
    public int HealthPotionCount { get => healthPotionCount; set => healthPotionCount = value; }
}
