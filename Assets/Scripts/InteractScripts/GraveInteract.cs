using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveInteract : InteractableInterface
{
    [SerializeField] public string[] lines;
    [SerializeField] public GameObject dialogueBox;
    [SerializeField] public DialogueController dialogueController;
    [SerializeField] public Color dialogueColor = Color.white;

    private bool isTalking = false;
    public override void interact(){
        if(!isTalking){
            interactText = "";
            speak();
        }
    }
    public void speak(){
        isTalking = !isTalking;
        dialogueController.lines = lines;
        dialogueController.textColor = dialogueColor;
        dialogueBox.SetActive(true);
        dialogueController.StartDialogue();
    }
    public void Update(){
        if(dialogueController.isActiveAndEnabled == false){
            dialogueController.lines = null;
            dialogueBox.SetActive(false);
            isTalking = !isTalking;
            interactText = "";
        }
    }
}
