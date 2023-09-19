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
        Vector3 displacement = new Vector3(0.15f, -0.15f, -0.7f);
        Vector3 pos = tombstonePos + displacement;
        Instantiate(gameObject, pos, Quaternion.Euler(0.0f, -90.0f, 0.0f));
    }
}
