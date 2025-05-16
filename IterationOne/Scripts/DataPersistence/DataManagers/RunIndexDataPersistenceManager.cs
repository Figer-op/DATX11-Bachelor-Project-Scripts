public class RunIndexDataPersistenceManager : DataPersistenceManager<RunIndexData> 
{
    private void Start()
    {
        if (!HasData())
        {
            CreateNewData();
        }
    }
}
