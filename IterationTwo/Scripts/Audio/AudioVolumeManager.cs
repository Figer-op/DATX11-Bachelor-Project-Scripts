using System;
using UnityEngine;

public class AudioVolumeManager : MonoBehaviour, IDataPersistence<SettingsData>
{
    public static AudioVolumeManager Instance { get; private set; }

    public float MasterVolume { get; private set; } = 1;
    public float MusicVolume { get; private set; } = 0.5f;
    public float InGameMusicVolume => MusicVolume * MasterVolume;
    public float SoundVolume { get; private set; } = 0.5f;
    public float InGameSoundVolume => SoundVolume * MasterVolume;

    public static event Action OnMasterVolumeChanged;
    public static event Action OnMusicVolumeChanged;
    public static event Action OnSoundVolumeChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Found more than one Audio Volume Manager in scene, destroying newest.");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void HandleMasterVolumeSliderChanged(float newVolume)
    {
        if (!IsValidVolume(newVolume))
        {
            Debug.LogWarning($"New Master Volume: {newVolume}, has to be in [0, 1]!");
            return;
        }

        MasterVolume = newVolume;
        OnMasterVolumeChanged?.Invoke();
        SettingsDataPersistenceManager.Instance.SaveTheData();
    }

    public void HandleMusicVolumeSliderChanged(float newVolume)
    {
        if (!IsValidVolume(newVolume))
        {
            Debug.LogWarning($"New Music Volume: {newVolume}, has to be in [0, 1]!");
            return;
        }

        MusicVolume = newVolume;
        OnMusicVolumeChanged?.Invoke();
        SettingsDataPersistenceManager.Instance.SaveTheData();
    }

    public void HandleSoundVolumeSliderChanged(float newVolume)
    {
        if (!IsValidVolume(newVolume))
        {
            Debug.LogWarning($"New Sound Volume: {newVolume}, has to be in [0, 1]!");
            return;
        }

        SoundVolume = newVolume;
        OnSoundVolumeChanged?.Invoke();
        SettingsDataPersistenceManager.Instance.SaveTheData();
    }

    private static bool IsValidVolume(float newVolume)
    {
        return 0 <= newVolume && newVolume <= 1;
    }

    public void LoadData(SettingsData data)
    {
        MasterVolume = data.MasterVolume;
        MusicVolume = data.MusicVolume;
        SoundVolume = data.SoundVolume;
    }

    public void SaveData(SettingsData data)
    {
        data.MasterVolume = MasterVolume;
        data.MusicVolume = MusicVolume;
        data.SoundVolume = SoundVolume;
    }
}
