using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindControl : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public SkinnedMeshRenderer[] renderer;
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
        //Debug.Log((initialOffset - houseDistance) / 6);        
        
       
            
    }
}
