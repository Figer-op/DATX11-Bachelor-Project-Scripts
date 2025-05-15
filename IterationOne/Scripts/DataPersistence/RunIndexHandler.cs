using UnityEngine;

public class RunIndexHandler : MonoBehaviour, IDataPersistence<RunIndexData>
{
    public static RunIndexHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Found more than one Run Index Handler in scene, destroying newest.");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public int RunIndex { get; private set; }

    public void IncrementRunIndex()
    {
        RunIndex++;
        // Have to save right after incrementing
        // so that it is not overridden when loading new scene.
        RunIndexDataPersistenceManager.Instance.SaveTheData();
    }

    public void LoadData(RunIndexData data)
    {
        RunIndex = data.RunIndex;
    }

    public void SaveData(RunIndexData data)
    {
        data.RunIndex = RunIndex;
    }
}
