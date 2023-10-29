using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerControler : MonoBehaviour
{
    private bool flickering = false;
    private float wait;
    private float randomIntensity;
    private Light flashlight;

    void Start() {
        flashlight = GetComponentInChildren<Light>();
    }
    void Update()
    {
        if(flashlight.enabled) {
            if(!flickering) {
                StartCoroutine(Flicker());
            }
        }
    }

    IEnumerator Flicker() {
        flickering = true;
        for(int i = 0; i < 2; i ++) {
            if(flashlight.enabled) {
                randomIntensity = Random.Range(1f, 5f);
                flashlight.intensity = randomIntensity;
                wait = Random.Range(0.01f, 0.2f);
                yield return new WaitForSeconds(wait);
            }
        }
        flickering = false;
    }

}
