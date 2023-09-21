using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemHandlerInterface : MonoBehaviour
{
    [SerializeField] public string[] lines;
    [SerializeField] public Color dialogueColor = Color.white;
    
    private GameObject dialogueBox;
    private DialogueController dialogueController;

    public bool InitHandler(GameObject box, DialogueController controller)
    {
        dialogueBox = box;
        dialogueController = controller;
        return HandleBehavior();
    }

    public void ShowMonologue()
    {
        // reset dialogue
        dialogueBox.SetActive(false);
        
        // start dialogue
        dialogueController.lines = lines;
        dialogueController.textColor = dialogueColor;
        dialogueBox.SetActive(true);
        dialogueController.StartDialogue();
    }

    public abstract bool HandleBehavior();
}
