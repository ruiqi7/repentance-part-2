using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleHandler : ItemHandlerInterface
{
    [SerializeField] private int interactDistance = 2;
    
    private GameObject player;

    public override bool HandleBehavior()
    {
        player = GameObject.FindWithTag("Player");
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
        
        Vector3 playerPos = player.transform.position;
        Vector3 playerDir = player.transform.forward;
        Vector3 pos = playerPos + playerDir;
        pos.y = 0.2f;
        Vector3 size = GetComponent<MeshRenderer>().bounds.size;
        if (CheckSpace(pos, size, Quaternion.identity))
        {
            pos.y = 0.0f;
            Instantiate(gameObject, pos, Quaternion.identity);
            return true;
        }
        else
        {
            ShowMonologue(ItemStatus.NOTUSED, -1);
            return false;
        }
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

        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.SpeedBoost();
        
        return true;
    }
}
