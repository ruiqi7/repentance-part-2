using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : InteractableInterface
{
    // Code credited https://youtu.be/oCv14L3Ew4w?si=yCkoPQE278BF7nSt
    public bool isOpen = false;
    public Animator door;

    public override void interact(){
        if(!isOpen){
            open();
            isOpen = true;
        } else { 
            close();
            isOpen = false;
        }
    }
    public void open(){
        door.SetBool("open", true);
        door.SetBool("closed", false);
    }
    public void close(){
        door.SetBool("closed", true);
        door.SetBool("open", false);
    }
}
