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
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject particleSystem1;
    [SerializeField] private GameObject particleSystem2;
    [SerializeField] GameObject npc;
    private bool hadItem = false;
    private bool isTalking = false;
    private bool started = false;
    private bool dissolve = false;
    private SkinnedMeshRenderer renderer;

    public override void interact(){
        if(!isTalking){
            started = true;
            interactText = "";
            speak();
        }
    }

    void Start() {
        renderer = GetComponentsInChildren<SkinnedMeshRenderer>()[0];
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
            interactText = "I N T E R A C T [E]";
            if(started && hadItem) {
                int index = inventory.GetComponent<InventoryController>().GetItemIndex(item);
                inventory.GetComponent<InventoryController>().RemoveFromInventory(index);
                batteryBar.value += 25;
                started = false;
                dissolve = true;
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