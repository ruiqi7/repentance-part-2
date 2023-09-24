using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltHandler : ItemHandlerInterface
{
    public override bool HandleBehavior()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 playerPos = player.transform.position;
        Vector3 playerDir = player.transform.forward;
        Vector3 pos = playerPos + playerDir * 1.2f;
        pos.y = 0.05f;
        Vector3 size = GetComponent<MeshRenderer>().bounds.size;
        Vector3 scaledSize = Vector3.Scale(size, new Vector3(10, 0, 10));
        Quaternion rot = player.transform.rotation;
        if (CheckSpace(pos, scaledSize, rot))
        {
            Instantiate(gameObject, pos, rot);
            ShowMonologue(true);
            return true;
        }
        else
        {
            ShowMonologue(false);
            return false;
        }
    }
}
