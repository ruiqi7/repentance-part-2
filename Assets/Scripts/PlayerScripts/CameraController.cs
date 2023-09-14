using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(1,20)]public int sens = 8;
    float xRotation = 0f;
    public Transform player;
    private PlayerUI playerUI;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void LateUpdate()
    {
        // Mouse Input
        float mouseX = Input.GetAxisRaw("Mouse X")  * sens;
        float mouseY = Input.GetAxisRaw("Mouse Y")  * sens;

        xRotation -= mouseY;
        xRotation = Math.Clamp(xRotation, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }
}
