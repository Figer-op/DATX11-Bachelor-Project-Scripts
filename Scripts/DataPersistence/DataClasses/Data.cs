using UnityEngine;

[System.Serializable]
public abstract class Data
{
    // The fields need to be SerializeField or public.
    // Initially not using [field: SerializeField] to make save data more readable.
    [SerializeField]
    private long lastUpdated;
    public long LastUpdated { get { return lastUpdated; } set { lastUpdated = value; } }
}
