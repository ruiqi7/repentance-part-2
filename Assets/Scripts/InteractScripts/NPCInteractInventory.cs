using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool started = false;

    private bool isTalking = false;

    public override void interact(){
        if(!isTalking){
            started = true;
            speak();
        }
    }
    public void speak(){
        isTalking = !isTalking;
        if(inventory.GetComponent<InventoryController>().CheckInventory(item)) {
            dialogueController.lines = linesWithItem;
        } else {
            dialogueController.lines = linesWithoutItem;
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
            if(started) {
                int index = inventory.GetComponent<InventoryController>().GetItemIndex(item);
                inventory.GetComponent<InventoryController>().RemoveFromInventory(index);
                started = false;
            }
        }
    }
}