using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }

    [SerializeField]
    private MusicCollectionSO musicCollection;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private MusicName startMusic;

    private float lastAudioBalacingValue = 1;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Found more than one Music Player in scene, destroying newest.");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayMusic(startMusic);
    }

    private void OnEnable()
    {
        AudioVolumeManager.OnMasterVolumeChanged += AdjustAudioVolume;
        AudioVolumeManager.OnMusicVolumeChanged += AdjustAudioVolume;
    }

    private void OnDisable()
    {
        AudioVolumeManager.OnMasterVolumeChanged -= AdjustAudioVolume;
        AudioVolumeManager.OnMusicVolumeChanged -= AdjustAudioVolume;
    }

    public void PlayMusic(MusicName musicName)
    {
        MusicSO musicToPlay = musicCollection.FindSong(musicName);
        lastAudioBalacingValue = musicToPlay.AudioBalancingValue;
        AdjustAudioVolume();
        audioSource.clip = musicToPlay.AudioClip;
        audioSource.Play();
    }

    private void AdjustAudioVolume()
    {
        audioSource.volume = lastAudioBalacingValue * AudioVolumeManager.Instance.InGameMusicVolume;
    }
}
