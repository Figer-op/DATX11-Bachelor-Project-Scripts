using UnityEngine;
using System;
using UnityEngine.UI;

public class SoundVolumeSlider : VolumeSlider
{
    protected override void VolumeChangeHandler()
    {
        AudioVolumeManager.Instance.HandleSoundVolumeSliderChanged(volumeSlider.value / 100);
    }

    protected override void LoadVolume()
    {
        volumeSlider.value = AudioVolumeManager.Instance.SoundVolume*100;
    }
}
