using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentIterationManager : MonoBehaviour
{
    public static CurrentIterationManager Instance { get; private set; }
    public int CurrentIteration { get; private set; }

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
        CurrentIteration++;
        if (scene.name != "PCGBasicRoom")
        {
            CurrentIteration = 0;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Found more than one Current Iteration Manager in scene, destroying newest.");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
}