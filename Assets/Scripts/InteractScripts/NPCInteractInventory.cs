using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Linq;

public class NPCInteractInventory : InteractableInterface
{
    [SerializeField] public string[] linesWithoutItem;
    [SerializeField] public string[] linesWithItem;
    [SerializeField] public GameObject dialogueBox;
    [SerializeField] public DialogueController dialogueController;
    [SerializeField] public Color dialogueColor = Color.white;
    [SerializeField] public Light flashlight;
    [SerializeField] public string item;
    [SerializeField] public GameObject inventory;
    [SerializeField] private Slider batteryBar;
    private bool started = false;
    private bool hadItem = false;
    private bool isTalking = false;

    public override void interact(){
        if(!isTalking){
            started = true;
            interactText = "";
            speak();
        }
    }
    public void speak(){
        isTalking = !isTalking;
        if(inventory.GetComponent<InventoryController>().CheckInventory(item)) {
            dialogueController.lines = linesWithItem;
            hadItem = true;
             if(this.particle[0]){
                for(int i = 0;i < this.particle.Count();i ++){
                    this.particle[i].Play();
                }
                if(clip){
                    AudioSource.PlayClipAtPoint(clip, this.transform.position);
                }
            }
            
        } else {
            dialogueController.lines = linesWithoutItem;
           if(this.particle[0]){
                this.particle[0].Play();
            }
        }
        dialogueController.textColor = dialogueColor;
        dialogueBox.SetActive(true);
        dialogueController.StartDialogue();
    }

    
    public void Update(){
        if(dialogueController.isActiveAndEnabled == false){
            dialogueController.lines = null;
            dialogueBox.SetActive(false);
            isTalking = !isTalking;
            interactText = "I N T E R A C T [E]";
            if(started && hadItem) {
                int index = inventory.GetComponent<InventoryController>().GetItemIndex(item);
                inventory.GetComponent<InventoryController>().RemoveFromInventory(index);
                batteryBar.value += 25;
                started = false;
            }
        }
    }
}