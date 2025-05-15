using UnityEngine;

[System.Serializable]
public class RunIndexData : Data
{
    [SerializeField]
    private int runIndex;
    public int RunIndex { get { return runIndex; } set { runIndex = value; } }

    public RunIndexData()
    {
        runIndex = 0;
    }
}
