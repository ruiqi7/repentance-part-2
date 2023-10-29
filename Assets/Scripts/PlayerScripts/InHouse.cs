using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHouse : MonoBehaviour
{
    public bool inHouse = false;
    private GameObject wind;

    // Update is called once per frame
    void Start(){
        wind = GameObject.Find("Wind_Loop");
    }
    void Update()
        {
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            inHouse=true;
            wind.GetComponentInChildren<AudioSource>().volume = 0.1f;

        }
    }

    void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            inHouse=false;
            wind.GetComponentInChildren<AudioSource>().volume = 1f;
        }
    }
}
