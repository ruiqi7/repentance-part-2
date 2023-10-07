using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;



public class NoteInteract : InteractableInterface
{
    [SerializeField] private GameObject noteCanvas;
    [SerializeField] private TMP_Text noteTextArea;
    [SerializeField] [TextArea] private string noteText;
    [SerializeField] CharacterController player;
    [SerializeField] CameraController cameraController;
    public bool isOpen = false;
    public override void interact(){
        if(!isOpen){ShowNote();}    
    }
    public void ShowNote(){
        noteTextArea.text = noteText;
        noteCanvas.SetActive(true);
        player.enabled = false;
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
            SceneManager.LoadScene("MazeGeneration");
        }
    }
    private void Update(){
        if (Input.GetKeyUp(KeyCode.Escape) && isOpen) {
            CloseNote();
        }
    }
}
