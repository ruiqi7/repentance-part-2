using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float sprintSpeed = 10.0f;
    public float baseSpeed = 5.0f;
    
    public CharacterController player;
    public LayerMask groundMask;

    private bool isGrounded;
    Vector3 velocity;

    [SerializeField] private GameObject uiManager;
    [SerializeField] private Camera camera;
    private UIManager uiManagerScript;

    [SerializeField] private Slider staminaBar;
    [SerializeField] private float maxStamina;

    private bool conserveStamina = false;
    private bool speedBoost = false;

    void Start()
    {
        uiManagerScript = uiManager.GetComponent<UIManager>();
        staminaBar.value = maxStamina;
        var temp = camera.GetComponent<Camera>().GetComponent<PostProcess>().material;
        temp.SetFloat("_Active", 0f);
    }

    void FixedUpdate()
    {
        // Keyboard Input
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");
        Vector3 move = transform.right * xMove + transform.forward * zMove;
        player.Move(move * speed * Time.deltaTime); 
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {   
            speed = sprintSpeed;
            if (!conserveStamina)
            {
                string difficulty = PlayerPrefs.GetString("difficulty");
                if (difficulty == "Easy")
                {
                    staminaBar.value -= 0.005f;
                }
                else
                {
                    staminaBar.value -= 0.01f;
                }
            }
            if(staminaBar.value <= 0){
                speed = baseSpeed;
            }
        } else {
            speed = baseSpeed;
            staminaBar.value += 0.002f;
        }
        
        isGrounded = Physics.Raycast(player.transform.position, Vector3.down, (player.height / 2) + 0.1f, groundMask);
        if (isGrounded && velocity.y < 0) {
            velocity.y = 0f;
        }
        if (!isGrounded) {
            velocity.y += -9.8f;
            player.Move(velocity * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Enemy") {
            Vector3 enemyPosition = collision.gameObject.transform.position;
            transform.LookAt(new Vector3(enemyPosition.x, enemyPosition.y + 1.5f, enemyPosition.z));
            var temp = camera.GetComponent<Camera>().GetComponent<PostProcess>().material;
            temp.SetFloat("_Active", 1f);
            uiManagerScript.GameOver();
        }
    }

    public void SpeedBoost()
    {
        if (!speedBoost)
        {
            speedBoost = true;
            baseSpeed += 0.05f;
            sprintSpeed += 0.05f;
            Invoke("SpeedBoost", 20.0f);
        }
        else
        {
            speedBoost = false;
            baseSpeed -= 0.05f;
            sprintSpeed -= 0.05f;
        }
    }

    public void ConserveStamina()
    {
        if (!conserveStamina)
        {
            conserveStamina = true;
            Invoke("ConserveStamina", 20.0f);
        }
        else
        {
            conserveStamina = false;
        }
    }
}
