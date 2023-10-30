using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerHandler : ItemHandlerInterface
{
    [SerializeField] private int interactDistance = 2;
    
    public override bool HandleBehavior()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactDistance)) {
            if (hit.collider.transform.parent != null && hit.collider.transform.parent.name == "Graves") {
                bool placed = PlaceFlower(hit.collider.gameObject);
                if (placed)
                {
                    ShowMonologue(ItemStatus.USED, -1);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        ShowMonologue(ItemStatus.NOTUSED, -1);
        return false;
    }

    private bool PlaceFlower(GameObject grave)
    {
        GraveHandler graveHandler = grave.GetComponent<GraveHandler>();
        if (graveHandler.occupied)
        {
            return false;
        }
        
        Vector3 gravePos = grave.transform.position;
        Vector3 graveDir = grave.transform.up;
        Vector3 pos = gravePos + graveDir * 0.6f;
        pos.y = -0.05f;
        
        Vector3 graveRot = grave.transform.localEulerAngles;
        GameObject flower = Instantiate(gameObject, pos, Quaternion.Euler(0.0f, graveRot.y - 90.0f, 0.0f));
        if(clip){
            AudioSource.PlayClipAtPoint(clip, pos);
        }
        flower.tag = "Untagged"; // cannot pick up the flower once used
        grave.GetComponent<GraveHandler>().occupied = true;

        GameObject player = GameObject.FindWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.ConserveStamina();

        return true;
    }
}
