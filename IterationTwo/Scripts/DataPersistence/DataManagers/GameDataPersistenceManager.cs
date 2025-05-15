public class GameDataPersistenceManager : DataPersistenceManager<GameData> 
{
    private void Start()
    {
        // Have to load the data again because the suffix path needs to be updated
        // after all data has been loaded initially.
        string runIndexSuffix = RunIndexHandler.Instance.RunIndex.ToString();
        UpdateDataHandlerPath(runIndexSuffix);
        LoadTheData();
    }

    public override void CreateNewData()
    {
        RunIndexHandler.Instance.IncrementRunIndex();
        string runIndexSuffix = RunIndexHandler.Instance.RunIndex.ToString();
        UpdateDataHandlerPath(runIndexSuffix);
        base.CreateNewData();
    }
}
