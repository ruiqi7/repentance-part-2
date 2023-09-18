using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollHandler : ItemHandlerInterface
{
    public override void HandleBehavior()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 playerPos = player.transform.position;
        Vector3 playerDir = player.transform.forward;
        Vector3 pos = playerPos + playerDir;
        pos.y = 0.4f;
        Quaternion playerRot = player.transform.rotation;
        Instantiate(gameObject, pos, playerRot * Quaternion.Euler(-90.0f, 180.0f, 0.0f)); // need to check if there is space to spawn
    }
}
