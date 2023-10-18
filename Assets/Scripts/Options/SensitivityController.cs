using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityController : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;

    void Start()
    {
        if (!PlayerPrefs.HasKey("sensitivity"))
        {
            PlayerPrefs.SetInt("sensitivity", 8);
        }
        LoadSensitivity();
    }

    public void AdjustSensitivity()
    {
        SaveSensitivity();
    }

    private void LoadSensitivity()
    {
        sensitivitySlider.value = PlayerPrefs.GetInt("sensitivity");
    }

    private void SaveSensitivity()
    {
        PlayerPrefs.SetInt("sensitivity", (int) sensitivitySlider.value);
    }
}
