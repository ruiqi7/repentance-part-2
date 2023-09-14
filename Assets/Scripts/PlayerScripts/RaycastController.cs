using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class RaycastController : MonoBehaviour
{
    // Start is called before the first frame update
    public int interactDistance = 2;
    //[SerializeField] public TextMeshProUGUI promptText;
    public PlayerUI playerUI;
    void Start()
    {
        
    }

    // Update is called once per 
    void Update()
    {
        // Creates a Ray from the center of the viewport
        playerUI.updateText(string.Empty);
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * 10);
        RaycastHit hit;
  
        if (Physics.Raycast(ray, out hit, interactDistance)) {
            if (hit.collider.CompareTag("Interactable")) { 
                InteractableInterface interact = hit.collider.GetComponent<InteractableInterface>();
                playerUI.updateText(interact.interactText);
                playerUI.updateFont(interact.font);
                if (Input.GetKeyDown(KeyCode.E)) {
                    interact.interact();
                }
            }
        }
    }
}
