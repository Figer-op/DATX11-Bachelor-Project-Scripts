using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PCGMusicChooser : MonoBehaviour, IDataPersistence<GameData>
{
    private int timesGeneratedCount;

    private readonly IReadOnlyList<MusicName> levelSongs = new List<MusicName>()
    {
        MusicName.DungeonLevelOne,
        MusicName.DungeonLevelTwo,
        MusicName.DungeonLevelThree
    };

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (timesGeneratedCount >= levelSongs.Count)
        {
            Debug.LogError("Generated count greater or equal to the music count!");
        }
        MusicPlayer.Instance.PlayMusic(levelSongs[timesGeneratedCount]);

    }

    public void LoadData(GameData data)
    {
        timesGeneratedCount = data.TimesGenerated;
    }

    public void SaveData(GameData data)
    {
        // Don't do anything.
    }
}
