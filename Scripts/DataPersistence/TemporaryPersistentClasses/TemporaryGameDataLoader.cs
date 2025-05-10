using UnityEngine;

public class TemporaryGameDataLoader : MonoBehaviour, IDataPersistence<GameData>
{
    public void LoadData(GameData data)
    {
        Debug.Log("Loaded the data!");
        Debug.Log(data.LastUpdated);
    }

    public void SaveData(GameData data)
    {
        Debug.Log("Would save the data now!");
    }
}
