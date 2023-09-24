using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemHandlerInterface : MonoBehaviour
{
    [SerializeField] private string[] linesWhenItemUsed;
    [SerializeField] private string[] linesWhenItemNotUsed;
    [SerializeField] private Color dialogueColor = Color.white;
    [SerializeField] private float dialogueSpeed = 0.05f;
    
    private GameObject dialogueBox;
    private DialogueController dialogueController;

    public bool InitHandler(GameObject box, DialogueController controller)
    {
        dialogueBox = box;
        dialogueController = controller;
        dialogueController.textSpeed = dialogueSpeed;
        return HandleBehavior();
    }

    public void ShowMonologue(bool itemUsed)
    {
        // reset dialogue
        dialogueBox.SetActive(false);
        
        // start dialogue
        if (itemUsed)
        {
            dialogueController.lines = linesWhenItemUsed;
        }
        else
        {
            dialogueController.lines = linesWhenItemNotUsed;
        }
        dialogueController.textColor = dialogueColor;
        dialogueBox.SetActive(true);
        dialogueController.StartDialogue();
    }

    public bool CheckSpace(Vector3 center, Vector3 size, Quaternion rotation)
    {
        return !Physics.CheckBox(center, size / 2, rotation);
    }

    public abstract bool HandleBehavior();
}
