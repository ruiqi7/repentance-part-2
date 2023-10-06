using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class NPCInteractTimer : InteractableInterface
{
    [SerializeField] public string[] beggining;
    [SerializeField] public string[] halfWay;
    [SerializeField] public string[] ending;
    [SerializeField] public GameObject dialogueBox;
    [SerializeField] public DialogueController dialogueController;
    [SerializeField] public Color dialogueColor = Color.white;
    [SerializeField] public GameObject UIManager;
    private bool isTalking = false;

    public override void interact(){
        if(!isTalking){
            speak();
        }
    }
    public void speak(){
        isTalking = !isTalking;
        if(UIManager.GetComponent<UIManager>().getTimePassed() < 150) {
            dialogueController.lines = beggining;
        } else if(UIManager.GetComponent<UIManager>().getTimePassed() >= 150 && UIManager.GetComponent<UIManager>().getTimePassed() < 240) {
            dialogueController.lines = halfWay;
        } else {
            dialogueController.lines = ending;
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
        }
    }
}