using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPCInteract : InteractableInterface
{
    [SerializeField] public string[] lines;
    [SerializeField] public GameObject dialogueBox;
    [SerializeField] public DialogueController dialogueController;
    [SerializeField] public Color dialogueColor = Color.white;
    [SerializeField] public Light flashlight;
    private bool started = false;

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
        dialogueController.lines = lines;
        dialogueController.textColor = dialogueColor;
        dialogueBox.SetActive(true);
        dialogueController.StartDialogue();
    }

    IEnumerator TurnOff() {
        flashlight.enabled = false;
        yield return new WaitForSeconds(10);
        flashlight.enabled = true;
    }
    
    public void Update(){
        if(dialogueController.isActiveAndEnabled == false){
            dialogueController.lines = null;
            dialogueBox.SetActive(false);
            isTalking = !isTalking;
            interactText = "I N T E R A C T [E]";
            if(started) {
                StartCoroutine(TurnOff());
                started = false;
            }
        }
    }
}
