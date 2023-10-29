using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessController : MonoBehaviour
{
    [SerializeField] private Slider brightnessSlider;

    private Image image; 

    void Start()
    {
        image = GetComponent<Image>();
        if (!PlayerPrefs.HasKey("brightness"))
        {
            PlayerPrefs.SetFloat("brightness", 1.0f);
        }
        LoadBrightness();
        AdjustBrightness(brightnessSlider.value);
    }

    public void AdjustBrightness(float value)
    {
        Color color = image.color;
        color.a = 1.0f - value;
        image.color = color;
        SaveBrightness();
    }

    private void LoadBrightness()
    {
        brightnessSlider.value = PlayerPrefs.GetFloat("brightness");
    }

    private void SaveBrightness()
    {
        PlayerPrefs.SetFloat("brightness", brightnessSlider.value);
    }
}
