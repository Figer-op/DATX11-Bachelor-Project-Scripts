using UnityEngine;
using System;
using UnityEngine.UI;

public class MusicVolumeSlider : VolumeSlider
{
    protected override void VolumeChangeHandler()
    {
        AudioVolumeManager.Instance.HandleMusicVolumeSliderChanged(volumeSlider.value / 100);
    }

    protected override void LoadVolume()
    {
        volumeSlider.value = AudioVolumeManager.Instance.MusicVolume*100;
    }
}
