using UnityEngine;
using System.Collections;

public class TimeDataHandler : MonoBehaviour, IDataPersistence<GameData>
{
    private float startTime;

    private float timestamp;
    
    private PlayerBase playerBase;

    private DungeonType currentDungeon;

    private void Awake() 
    {
        startTime = Time.time;

        playerBase = FindAnyObjectByType<PlayerBase>();
        currentDungeon = playerBase.CurrentDungeon;
    }

    public void LoadData(GameData data) 
    {
        timestamp = data.DungeonData[currentDungeon].Timestamp;
    }

    public void SaveData(GameData data)
    {
        float timeWhileSaving = Time.time;
        timestamp += timeWhileSaving - startTime;
        data.DungeonData[currentDungeon].Timestamp = timestamp;
        startTime = timeWhileSaving;
    }

    public float GetCurrentTime()
    {
        return timestamp + Time.time - startTime;
    }
}
