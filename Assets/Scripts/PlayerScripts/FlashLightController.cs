using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class FlashLightController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Camera;
    [SerializeField] private Slider batteryBar;
    [SerializeField] private float maxBattery;
    [SerializeField] private Image barFill;
    private Light flashlight;
    void Start()
    {
        flashlight = GetComponentInChildren<Light>();
        flashlight.enabled = false;
        batteryBar.maxValue = maxBattery;
        batteryBar.value = maxBattery;
        batteryBar.minValue = 0f;
        barFill.color = Color.green;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(flashlight.enabled) {
            batteryBar.value -= 0.0075f;
        }
        transform.position = Camera.position;
        transform.rotation= Camera.rotation;
        if (Input.GetKeyDown(KeyCode.F) && batteryBar.value > 0)
        {
            flashlight.enabled = !flashlight.enabled;
        }
        if(batteryBar.value <= 0) {
            flashlight.enabled = false;
        }

        if(batteryBar.value < (0.5*maxBattery)) {
            barFill.color = new Color(211f,84f,0f);
        }
        if(batteryBar.value < (0.25*maxBattery)) {
            barFill.color = Color.red;
        }
    }
}
