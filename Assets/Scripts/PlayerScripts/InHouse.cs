using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHouse : MonoBehaviour
{
    public bool inHouse = false;

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            inHouse=true;
        }
    }

    void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            inHouse=false;
        }
    }
}
