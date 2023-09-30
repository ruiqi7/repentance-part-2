using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleHandler : ItemHandlerInterface
{
    [SerializeField] private int interactDistance = 2;
    [SerializeField] private float speedIncrement = 0.05f;
    
    private GameObject player;
    private PlayerController playerController;

    public override bool HandleBehavior()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactDistance)) {
            if (hit.collider.CompareTag("Tombstone")) {
                PlaceCandle(hit.collider.gameObject);
                ShowMonologue(true);
                return true;
            }
        }
        ShowMonologue(false);
        return false;
    }

    private void PlaceCandle(GameObject tombstone)
    {
        Vector3 tombstonePos = tombstone.transform.position;
        Vector3 tombstoneDir = tombstone.transform.up;
        Vector3 posFront = tombstonePos + tombstoneDir * 0.6f;
        posFront.y = 0.0f;
        Vector3 posBack = tombstonePos - tombstoneDir * 0.6f;
        posBack.y = 0.0f;

        GameObject candle = null;
        if (isObjectFrontFacing(posFront, posBack))
        {
            candle = Instantiate(gameObject, posFront, Quaternion.identity);
        }
        else
        {
            candle = Instantiate(gameObject, posBack, Quaternion.identity);
        }

        tombstone.tag = "Untagged"; // cannot place another candle / flower on this tombstone
        candle.tag = "Untagged"; // cannot pick up the candle once used

        playerController = player.GetComponent<PlayerController>();
        playerController.ChangeSpeed(speedIncrement);
    }

    private bool isObjectFrontFacing(Vector3 posFront, Vector3 posBack)
    {
        player = GameObject.FindWithTag("Player");
        Vector3 playerPos = player.transform.position;
        return Vector3.Distance(playerPos, posFront) < Vector3.Distance(playerPos, posBack);
    }
}
