// Unity does not support having parametrized classes as components
// so have to make a subclass for each data type to be saved.
public class SettingsDataPersistenceManager : DataPersistenceManager<SettingsData> 
{
    private void Start()
    {
        if (!HasData())
        {
            CreateNewData();
        }
    }
}
