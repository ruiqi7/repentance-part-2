using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using TMPro;
using System.Linq;

public class NPCInteractTimer : InteractableInterface
{
    [SerializeField] public string[] beggining;
    [SerializeField] public string[] halfWay;
    [SerializeField] public string[] ending;
    [SerializeField] public GameObject dialogueBox;
    [SerializeField] public DialogueController dialogueController;
    [SerializeField] public Color dialogueColor = Color.white;
    [SerializeField] public GameObject UIManager;
    private UIManager UIManagerScript;
    private bool started = false;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject particleSystem1;
    [SerializeField] private GameObject particleSystem2;

    private bool isTalking = false;

    void Start() {
        UIManagerScript = UIManager.GetComponent<UIManager>();
    }

    public override void interact(){
        if(!isTalking){
            if(dialogueBox.activeSelf){
                dialogueController.SkipLine();
                dialogueController.lines = null;
            }
            started = true;
            interactText = "";
            if(this.particle[0]){
                for(int i = 0;i < this.particle.Count();i ++){
                    this.particle[i].Play();
                }
                if(clip){
                    AudioSource.PlayClipAtPoint(clip, this.transform.position);
                }
            }
            speak();
        }
    }
    public void speak(){
        isTalking = !isTalking;
        if(UIManagerScript.getTimePassed() < 150) {
            dialogueController.lines = beggining;
        } else if(UIManagerScript.getTimePassed() >= 150 && UIManagerScript.getTimePassed() < 240) {
            dialogueController.lines = halfWay;
        } else {
            dialogueController.lines = ending;
        }
        dialogueController.textColor = dialogueColor;
        dialogueBox.SetActive(true);
        dialogueController.StartDialogue();

    }

    
    public void Update(){
        if(Vector3.Distance(player.transform.position, transform.position) < 10) {
            particleSystem1.SetActive(false);
            particleSystem2.SetActive(true);
        } else if(Vector3.Distance(player.transform.position, transform.position) < 200){ 
            particleSystem1.SetActive(true);
            particleSystem2.SetActive(false);
        } else {
             particleSystem1.SetActive(false);
            particleSystem2.SetActive(false);
        }
        if(dialogueController.isActiveAndEnabled == false){
            dialogueController.lines = null;
            dialogueBox.SetActive(false);
            isTalking = !isTalking;
            interactText = "Interact [E]";
            if(started) {
                started = false;
            }
        }
    }
}