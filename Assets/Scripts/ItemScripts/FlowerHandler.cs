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
            if (hit.collider.CompareTag("Tombstone")) {
                PlaceFlower(hit.collider.gameObject);
                return true;
            }
        }
        // show some UI text
        return false;
    }

    private void PlaceFlower(GameObject tombstone)
    {
        Vector3 tombstonePos = tombstone.transform.position;
        Vector3 tombstoneDir = tombstone.transform.up;
        Vector3 posFront = tombstonePos + tombstoneDir * 0.6f;
        posFront.y = -0.15f;
        Vector3 posBack = tombstonePos - tombstoneDir * 0.6f;
        posBack.y = -0.15f;

        GameObject flower = null;
        Vector3 tombstoneRot = tombstone.transform.localEulerAngles;
        if (isObjectFrontFacing(posFront, posBack))
        {
            flower = Instantiate(gameObject, posFront, Quaternion.Euler(0.0f, tombstoneRot.y - 90.0f, 0.0f));
        }
        else
        {
            flower = Instantiate(gameObject, posBack, Quaternion.Euler(0.0f, tombstoneRot.y + 90.0f, 0.0f));
        }
        flower.tag = "Untagged";
    }

    private bool isObjectFrontFacing(Vector3 posFront, Vector3 posBack)
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 playerPos = player.transform.position;
        return Vector3.Distance(playerPos, posFront) < Vector3.Distance(playerPos, posBack);
    }
}
