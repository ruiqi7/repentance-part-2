using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class FlashLightController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Camera;
    [SerializeField] private Slider batteryBar;
    [SerializeField] private float maxBattery;
    [SerializeField] private Image barFill;
    public bool off = false;
    private Light flashlight;
    void Start()
    {
        flashlight = GetComponentInChildren<Light>();
        flashlight.enabled = false;
        batteryBar.maxValue = maxBattery;
        batteryBar.value = maxBattery;
        batteryBar.minValue = 0f;
        if(SceneManager.GetActiveScene().name == "MazeGeneration") {
            barFill.color = Color.green;
        }
    }

    // Update is called once per frame
    void Update(){
         if (Input.GetKeyDown(KeyCode.F) && batteryBar.value > 0 && !off)
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
    void FixedUpdate()
    {
        if(flashlight.enabled && SceneManager.GetActiveScene().name == "MazeGeneration") {
            string difficulty = PlayerPrefs.GetString("difficulty");
            if (difficulty == "Easy")
            {
                batteryBar.value -= 0.0075f;
            }
            else
            {
                batteryBar.value -= 0.015f;
            }
        }
        transform.position = Camera.position;
        transform.rotation= Camera.rotation;
        // if (Input.GetKeyDown(KeyCode.F) && batteryBar.value > 0)
        // {
        //     flashlight.enabled = !flashlight.enabled;
        // }
        if(batteryBar.value <= 0) {
            flashlight.enabled = false;
        }

        if(batteryBar.value < (0.5*maxBattery) && SceneManager.GetActiveScene().name == "MazeGeneration") {
            barFill.color = new Color(211f,84f,0f);
        }
        if(batteryBar.value < (0.25*maxBattery) && SceneManager.GetActiveScene().name == "MazeGeneration") {
            barFill.color = Color.red;
        }
    }
}