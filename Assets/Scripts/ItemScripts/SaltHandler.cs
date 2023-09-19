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
        pos.y = 0.0f;
        Quaternion playerRot = player.transform.rotation;
        Instantiate(gameObject, pos, playerRot);
        return true;
    }
}
