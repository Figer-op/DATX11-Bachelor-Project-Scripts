using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderMusicChooser : MonoBehaviour
{
    [SerializeField]
    private MusicName musicToPlay;

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
        MusicPlayer.Instance.PlayMusic(musicToPlay);
    }
}
