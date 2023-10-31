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
    [SerializeField] private AudioSource lightOn;
    [SerializeField] private AudioSource lightOff;
    public bool off = false;
    private bool audioPlayed = false;
    private Light flashlight;
    void Start()
    {
        flashlight = GetComponentInChildren<Light>();
        flashlight.enabled = false;
        batteryBar.maxValue = maxBattery;
        batteryBar.value = maxBattery;
        batteryBar.minValue = 0f;
        if(SceneManager.GetActiveScene().name == "MazeGeneration") {
            barFill.color = new Color(11f/255f, 84f/255f, 0f/255f);
        }
    }

    // Update is called once per frame
    void Update(){
         if (Input.GetKeyDown(KeyCode.F) && batteryBar.value > 0 && !off)
        {
            flashlight.enabled = !flashlight.enabled;
            lightOn.Play();
            audioPlayed = false;
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
                batteryBar.value -= 0.008f;
            }
        }
        transform.position = Camera.position;
        transform.rotation= Camera.rotation;

        if(batteryBar.value <= 0) {
            flashlight.enabled = false;
            if(!audioPlayed){
                lightOff.Play();
                audioPlayed = true;
            }
        }

        if(batteryBar.value < (0.5*maxBattery) && SceneManager.GetActiveScene().name == "MazeGeneration") {
            barFill.color = new Color(121f/255f,71f/255f,0f/255f);
        }
        if(batteryBar.value < (0.25*maxBattery) && SceneManager.GetActiveScene().name == "MazeGeneration") {
            barFill.color = new Color(82f/255f, 10f/255f, 0f/255f);
        }
    }
}