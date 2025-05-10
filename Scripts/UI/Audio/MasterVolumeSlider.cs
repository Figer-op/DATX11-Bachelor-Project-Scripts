using UnityEngine;
using System;
using UnityEngine.UI;

public class MasterVolumeSlider : VolumeSlider
{
    protected override void VolumeChangeHandler()
    {
        AudioVolumeManager.Instance.HandleMasterVolumeSliderChanged(volumeSlider.value / 100);
    }

    protected override void LoadVolume()
    {
        volumeSlider.value = AudioVolumeManager.Instance.MasterVolume*100;
    }
}
