using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Source: https://www.youtube.com/watch?v=yWCHaTwVblk&t=3s

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    void Start()
    {
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 1.0f);
        }
        LoadVolume();
        AdjustVolume();
    }

    public void AdjustVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SaveVolume();
    }

    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
    }

    private void SaveVolume()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }
}
