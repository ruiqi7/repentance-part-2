using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverInteract : InteractableInterface
{
    public Transform Object;
    public override void interact(){
        if(this.particle){
            ParticleSystem part = Instantiate(this.particle, Object.transform);
            part.Play();
        }
    }
}
