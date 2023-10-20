using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterExitManager : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Close);
    }
    
    private void Close()
    {
        GameObject[] interactables = GameObject.FindGameObjectsWithTag("Interactable");
        foreach (GameObject interactable in interactables)
        {
            if (interactable.name.Contains("Letter"))
            {
                NoteInteract noteInteract = interactable.GetComponent<NoteInteract>();
                if (noteInteract.isOpen)
                {
                    noteInteract.CloseNote(); 
                    break;
                }
            }
        }
    }

    // private void FirstLetter() {
    //     GameObject temp = GameObject.Find("LetterImage");
    //     NoteInteract noteInteract = temp.GetComponent<NoteInteract>();
    //     noteInteract.setDissolving(true);
    //     GetComponent<Button>().Set
    // }
}
