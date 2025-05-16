using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public abstract class VolumeSlider : MonoBehaviour
{
    [SerializeField]
    protected Slider volumeSlider;
    [SerializeField]
    private TMP_Text sliderValueText;

    private void Awake()
    {
        volumeSlider.onValueChanged.AddListener(delegate { VolumeChangeHandler(); });
    }

    private void Start()
    {
        LoadVolume();
    }

    protected abstract void VolumeChangeHandler();
    protected abstract void LoadVolume();
    public void ChangeSliderValueText()
    {
        sliderValueText.text = volumeSlider.value.ToString();
    }
}
