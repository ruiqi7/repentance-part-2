using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Camera;
    private Light flashlight;
    void Start()
    {
        flashlight = GetComponentInChildren<Light>();
        flashlight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.position;
        transform.rotation= Camera.rotation;
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}
