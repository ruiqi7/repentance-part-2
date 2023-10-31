using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltHandler : ItemHandlerInterface
{
    [SerializeField] private float dissolveSpeed;
    
    public override bool HandleBehavior()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 playerPos = player.transform.position;
        Vector3 playerDir = player.transform.forward;
        Vector3 pos = playerPos + playerDir * 1.2f;
        pos.y = 0.0f;
        Vector3 size = GetComponent<MeshRenderer>().bounds.size;
        Vector3 scaledSize = Vector3.Scale(size, new Vector3(10, 0, 10));
        Quaternion rot = player.transform.rotation;
        if (CheckSpace(pos, scaledSize, rot))
        {
            GameObject salt = Instantiate(gameObject, pos, rot);
            if(clip){
                AudioSource.PlayClipAtPoint(clip, pos);
            }
            salt.tag = "Untagged"; // cannot pick up the salt once used
            GameObject repelArea = salt.transform.GetChild(1).gameObject;
            repelArea.SetActive(true);
            ShowMonologue(ItemStatus.USED, -1);
            return true;
        }
        else
        {
            ShowMonologue(ItemStatus.NOTUSED, -1);
            return false;
        }
    }

    void Update()
    {
        if (gameObject.tag == "Interactable")
        {
            return;
        }

        if (transform.localScale.x < 0.01f)
        {
            Destroy(gameObject);
        }
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, dissolveSpeed * Time.deltaTime);
    }
}
