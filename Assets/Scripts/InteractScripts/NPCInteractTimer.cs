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
    private bool started = false;
    private bool dissolve = false;
    [SerializeField] GameObject npc;

    private SkinnedMeshRenderer renderer;

    void Start() {
        renderer = GetComponentsInChildren<SkinnedMeshRenderer>()[0];
    }

    private bool isTalking = false;
    public override void interact(){
        if(!isTalking){
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
            interactText = "Interact [E]";
            if(started) {
                dissolve = true;
                started = false;
            }
        }

        if(dissolve)  {
            bool disable = true;
            for(int i = 0; i < renderer.materials.Length; i++) {
                if(renderer.materials[i].GetFloat("_Amount") <= 1) {
                    renderer.materials[i].SetFloat("_Amount",  renderer.materials[i].GetFloat("_Amount") + 0.001f);
                }
                if(renderer.materials[i].GetFloat("_BurnSize") <= 1) {
                    renderer.materials[i].SetFloat("_BurnSize",  renderer.materials[i].GetFloat("_BurnSize") + 0.001f);
                }
            }
            for(int i = 0; i < renderer.materials.Length; i++) {
                if(renderer.materials[i].GetFloat("_Amount") < 1) {
                    disable = false;
                }
            }
            if(disable) {
                npc.SetActive(false);
            }
        }
    }
}