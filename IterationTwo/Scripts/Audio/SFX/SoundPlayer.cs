using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField]
    private SoundCollectionSO soundCollection;

    [SerializeField]
    private AudioSource audioSource;

    private float lastAudioBalacingValue = 1;

    private void Start()
    {
        AdjustAudioVolume();
    }

    private void OnEnable()
    {
        AudioVolumeManager.OnMasterVolumeChanged += AdjustAudioVolume;
        AudioVolumeManager.OnSoundVolumeChanged += AdjustAudioVolume;
    }

    private void OnDisable()
    {
        AudioVolumeManager.OnMasterVolumeChanged -= AdjustAudioVolume;
        AudioVolumeManager.OnSoundVolumeChanged -= AdjustAudioVolume;
    }

    public void PlaySound(SoundName soundName)
    {
        SoundSO soundToPlay = soundCollection.FindSound(soundName);
        lastAudioBalacingValue = soundToPlay.AudioBalancingValue;
        AdjustAudioVolume();
        audioSource.PlayOneShot(soundToPlay.AudioClip);
    }

    private void AdjustAudioVolume()
    {
        audioSource.volume = lastAudioBalacingValue * AudioVolumeManager.Instance.InGameSoundVolume;
    }
}
