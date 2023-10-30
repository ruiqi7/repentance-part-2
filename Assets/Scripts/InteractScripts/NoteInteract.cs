using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class NoteInteract : InteractableInterface
{
    [SerializeField] private GameObject noteCanvas;
    [SerializeField] private TMP_Text noteTextArea;
    [SerializeField] [TextArea] private string noteText;
    [SerializeField] CharacterController player;
    [SerializeField] CameraController cameraController;
    private Material renderer;
    public bool isOpen = false; 
    public bool dissolving = false;
    private bool dissolved = false;

    public override void interact(){
        if(!isOpen){ShowNote();}    
    }
    public void ShowNote(){
        if(clip){
            AudioSource.PlayClipAtPoint(clip, this.transform.position);
        }
        noteTextArea.text = noteText;
        noteCanvas.SetActive(true);
        renderer = noteCanvas.GetComponent<Image>().material;
        player.enabled = false;
        renderer.SetFloat("_Amount", 0.0f);
        renderer.SetFloat("_BurnSize", 0.0f);
        cameraController.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isOpen = true;
    }
    public void CloseNote(){
        noteTextArea.text = string.Empty;
        noteCanvas.SetActive(false);
        player.enabled = true;
        cameraController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isOpen = false;
        if(SceneManager.GetActiveScene().name == "IntroScene"){
            SceneManager.LoadScene("Cutscene");
        }
    }
    private void Update(){
        if(dissolving) {
            if(GameObject.Find("Exit")) {
                GameObject.Find("Exit").SetActive(false);
            }
            if(GameObject.Find("PaperOnTable")) {
                GameObject.Find("PaperOnTable").tag = "Untagged";
                GameObject.Find("Paper").tag = "Untagged";
                GameObject.Find("PaperOnTable").SetActive(false);
                if( GameObject.Find("DialogueBox")){
                    GameObject.Find("DialogueBox").SetActive(false);
                }
            }
            noteTextArea.text = string.Empty;
            if(renderer.GetFloat("_Amount") <= 1) {
                renderer.SetFloat("_Amount",  renderer.GetFloat("_Amount") + 0.005f);
            }
            if(renderer.GetFloat("_BurnSize") <= 1) {
                renderer.SetFloat("_BurnSize",  renderer.GetFloat("_BurnSize") + 0.005f);
            }
            if(renderer.GetFloat("_Amount") >= 1) {
                dissolved = true;
            }
        }

        if(dissolved) {
            CloseNote();
        }
    }

    public void setDissolving(bool val) {
        Debug.Log("set dissolve");
        dissolving = val;
    }
}
