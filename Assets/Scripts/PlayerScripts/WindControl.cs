using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindControl : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public Material[] materials;
    [SerializeField] private GameObject player;
    [SerializeField] public float windIncrease; 

    private float initialOffset = 60;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float houseDistance = Vector3.Distance(player.transform.position, transform.position); 
        float lmaoNumber = (initialOffset - houseDistance) / 6;
        //Debug.Log(lmaoNumber);        
        for(int i = 0; i < materials.Length; i++) {
            materials[i].SetFloat("_WaveAmp", (lmaoNumber*0.02f));
            materials[i].SetVector("_WindSpeed", new Vector4(5+lmaoNumber,1,1,1));
        }
       
            
    }
}
