using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHouse : MonoBehaviour
{
    public bool inHouse = false;
    private AudioSource windAudio;

    // Update is called once per frame
    void Start(){
        if(GameObject.Find("Wind_Loop")) {
            windAudio = GameObject.Find("Wind_Loop").GetComponentInChildren<AudioSource>();
        }
    }
    void Update()
        {
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            inHouse=true;
            StopAllCoroutines();
            StartCoroutine(FadeOut());

        }
    }

    void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            inHouse=false;
            //windAudio.volume = 1f;
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }
    }
    IEnumerator FadeOut() 
    {
        
        float timeElapsed = 0;
        while (windAudio.volume > 0.1) 
        {
            windAudio.volume = Mathf.Lerp(windAudio.volume, 0.1f, timeElapsed);
            timeElapsed += Time.deltaTime * 10;
            //Debug.Log(timeElapsed);
            //Debug.Log(windAudio.volume);
            yield return new WaitForSeconds(0.07f);
        }
        StopAllCoroutines();
    }

    IEnumerator FadeIn() 
    {
        float timeElapsed = 0;

        while (windAudio.volume < 1) 
        {
            windAudio.volume = Mathf.Lerp(windAudio.volume, 1, timeElapsed);
            timeElapsed += Time.deltaTime * 10;
            //Debug.Log(timeElapsed);
            yield return new WaitForSeconds(0.05f);
        }
        StopAllCoroutines();
    }
}
