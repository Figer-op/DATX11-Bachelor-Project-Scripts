using UnityEngine;

public class PCGLevelExitBehaviour : InteractableLevelExit, IDataPersistence<GameData>
{
    private bool hasVisitedAll;
    private int timesGenerated;
    [SerializeField]
    private bool isHandMadeFinalLevel;

    public void LoadData(GameData data)
    {
        hasVisitedAll = data.DungeonData[DungeonType.Handmade].HasVisited && data.DungeonData[DungeonType.PCG].HasVisited;

        timesGenerated = data.TimesGenerated;
    }

    public void SaveData(GameData data)
    {
        data.TimesGenerated = timesGenerated;
    }

    protected override void Start()
    {
        base.Start();

        if (timesGenerated >= 2 && hasVisitedAll)
        {
            levelName = "EndCreditsScene";
        } 
        else if(timesGenerated >= 2)
        {
            levelName = "LevelSelect";
        } 
        else if (isHandMadeFinalLevel) 
        {
            levelName = "LevelSelect";
        }
        if (!isHandMadeFinalLevel)
        {
            timesGenerated++;
        }
        GameDataPersistenceManager.Instance.SaveTheData();
    }
}
