using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleHandler : ItemHandlerInterface
{
    [SerializeField] private int interactDistance = 2;
    
    public override bool HandleBehavior()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactDistance)) {
            if (hit.collider.transform.parent != null && hit.collider.transform.parent.name == "Graves") {
                bool placed = PlaceCandle(hit.collider.gameObject);
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

    private bool PlaceCandle(GameObject grave)
    {
        GraveHandler graveHandler = grave.GetComponent<GraveHandler>();
        if (graveHandler.occupied)
        {
            return false;
        }

        Vector3 gravePos = grave.transform.position;
        Vector3 graveDir = grave.transform.up;
        Vector3 pos = gravePos + graveDir * 0.6f;
        pos.y = 0.0f;
        GameObject candle = Instantiate(gameObject, pos, Quaternion.identity);
        candle.tag = "Untagged"; // cannot pick up the candle once used
        graveHandler.occupied = true;

        GameObject player = GameObject.FindWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.SpeedBoost();
        
        return true;
    }
}
