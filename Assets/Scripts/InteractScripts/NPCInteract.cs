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
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject particleSystem1;
    private bool started = false;
    private bool dissolve = false;
    [SerializeField] GameObject npc;
    private SkinnedMeshRenderer renderer;
    private bool handling = false;

    private bool isTalking = false;

    void Start() {
        renderer = GetComponentsInChildren<SkinnedMeshRenderer>()[0];
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
        dialogueController.lines = lines;
        dialogueController.textColor = dialogueColor;
        dialogueBox.SetActive(true);
        dialogueController.StartDialogue();
    }

    IEnumerator TurnOff() {
        handling = true;
        flashlight.enabled = false;
        flashlight.GetComponent<FlashLightController>().off = true;
        yield return new WaitForSeconds(10);
        flashlight.GetComponent<FlashLightController>().off = false;
        flashlight.enabled = true;
        handling = false;
    }
    
    public void Update(){
        if(Vector3.Distance(player.transform.position, transform.position) < 10) {
            particleSystem1.SetActive(false);
        } else if(Vector3.Distance(player.transform.position, transform.position) < 200){
            particleSystem1.SetActive(true);
        } else {
            particleSystem1.SetActive(false);
        }
        if(dialogueController.isActiveAndEnabled == false){
            dialogueController.lines = null;
            dialogueBox.SetActive(false);
            isTalking = !isTalking;
            interactText = "I N T E R A C T [E]";
            if(started) {
                StartCoroutine(TurnOff());
                dissolve = true;
                started = false;
            }
        }
        if(dissolve)  {
            bool disable = true;
            for(int i = 0; i < renderer.materials.Length; i++) {
                if(renderer.materials[i].GetFloat("_Amount") <= 1) {
                    renderer.materials[i].SetFloat("_Amount",  renderer.materials[i].GetFloat("_Amount") + 0.004f);
                }
                if(renderer.materials[i].GetFloat("_BurnSize") <= 1) {
                    renderer.materials[i].SetFloat("_BurnSize",  renderer.materials[i].GetFloat("_BurnSize") +0.004f);
                }
            }
            for(int i = 0; i < renderer.materials.Length; i++) {
                if(renderer.materials[i].GetFloat("_Amount") < 1) {
                    disable = false;
                }
            }
            if(disable && !handling) {
                npc.SetActive(false);
            }
        }
    }
}
