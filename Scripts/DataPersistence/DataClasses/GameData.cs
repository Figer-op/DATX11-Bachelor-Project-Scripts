using System;
using UnityEngine;

[System.Serializable]
public class GameData : Data
{
    [SerializeField]
    private SerializableDictionary<DungeonType, DataSpecificForDungeon> dungeonData;

    public SerializableDictionary<DungeonType, DataSpecificForDungeon> DungeonData { get => dungeonData; set => dungeonData = value; }

    [SerializeField]
    private int timesGenerated = 0;

    public int TimesGenerated { get => timesGenerated; set => timesGenerated = value; }

    public GameData()
    {
        dungeonData = new();
        foreach (DungeonType dungeonType in Enum.GetValues(typeof(DungeonType)))
        {
            dungeonData[dungeonType] = new DataSpecificForDungeon();
        }
    }
}
