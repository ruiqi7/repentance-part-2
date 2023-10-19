using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemHandlerInterface : MonoBehaviour
{
    [SerializeField] private string[] linesWhenItemCollected;
    [SerializeField] private string[] linesWhenItemUsed;
    [SerializeField] private string[] linesWhenItemNotUsed;
    [SerializeField] private Color dialogueColor = Color.white;
    [SerializeField] private float dialogueSpeed = 0.1f;
    
    private GameObject dialogueBox;
    private DialogueController dialogueController;

    public enum ItemStatus
    {
        COLLECTED,
        USED,
        NOTUSED
    }

    public void InitHandler(GameObject box, DialogueController controller)
    {
        dialogueBox = box;
        dialogueController = controller;
        dialogueController.textSpeed = dialogueSpeed;
    }

    public void ShowMonologue(ItemStatus status, int numToPress)
    {
        // reset dialogue
        dialogueBox.SetActive(false);
        
        // start dialogue
        switch (status)
        {
            case ItemStatus.COLLECTED:
                string pressText = "[ Press " + numToPress + " to use. ]";
                dialogueController.lines = new string[] { linesWhenItemCollected[0] + "<br>" + pressText };
                break;
            case ItemStatus.USED:
                dialogueController.lines = linesWhenItemUsed;
                break;
            case ItemStatus.NOTUSED:
                dialogueController.lines = linesWhenItemNotUsed;
                break;
            default:
                dialogueController.lines = new string[0];
                break;
        }
        dialogueController.textColor = dialogueColor;
        dialogueBox.SetActive(true);
        if (status == ItemStatus.COLLECTED)
        {
            dialogueController.FlashDialogue();
        }
        else
        {
            dialogueController.StartDialogue();
        }
    }

    public bool CheckSpace(Vector3 center, Vector3 size, Quaternion rotation)
    {
        return !Physics.CheckBox(center, size / 2, rotation);
    }

    public abstract bool HandleBehavior();
}
