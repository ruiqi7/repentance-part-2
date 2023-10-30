using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollHandler : ItemHandlerInterface
{
    public override bool HandleBehavior()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 playerPos = player.transform.position;
        Vector3 playerDir = player.transform.forward;
        Vector3 pos = playerPos + playerDir;
        pos.y = 0.25f;
        Vector3 size = GetComponent<MeshRenderer>().bounds.size;
        Quaternion playerRot = player.transform.rotation;
        Quaternion rot = playerRot * Quaternion.Euler(-90.0f, 180.0f, 0.0f);
        if (CheckSpace(pos, size, rot))
        {
            if(clip){
                AudioSource.PlayClipAtPoint(clip, pos);
            }
            Instantiate(gameObject, pos, rot);
            ShowMonologue(ItemStatus.USED, -1);
            return true;
        }
        else
        {
            ShowMonologue(ItemStatus.NOTUSED, -1);
            return false;
        }
    }
}
