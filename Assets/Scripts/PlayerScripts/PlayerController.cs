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

    [SerializeField] private AudioSource walk;
    [SerializeField] private AudioSource run;
    [SerializeField] private AudioSource walkHouse;
    [SerializeField] private AudioSource runHouse;

    private bool isWalking = false;
    private bool isRunning = false;

    private InHouse inHouseScript;

    public GameObject floor;
    private bool gameOver = false;
    private Vector3 enemyPosition;

    void Start()
    {
        uiManagerScript = uiManager.GetComponent<UIManager>();
        staminaBar.value = maxStamina;
        inHouseScript = floor.GetComponent<InHouse>();
        GameObject.Find("Timer").SetActive(true);
        if(camera.GetComponent<Camera>().GetComponent<PostProcess>()) {
            var temp = camera.GetComponent<Camera>().GetComponent<PostProcess>().material;
            temp.SetFloat("_Active", 0f);
        }
        if(GameObject.Find("PaperOnTable")) {
            GameObject.Find("PaperOnTable").SetActive(true);
        }
    }

    void FixedUpdate()
    {
        // Keyboard Inputs
        if(gameOver) {
            GameObject.Find("FlashLight").GetComponent<Light>().enabled = true;
            var targetDir = enemyPosition - transform.position;
            if(Vector3.Angle(transform.forward, targetDir) < 10) {
                uiManagerScript.PauseGame();
            } 
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDir), Time.deltaTime * 50f);
            var temp = camera.GetComponent<Camera>().GetComponent<PostProcess>().material;
            temp.SetFloat("_Active", 1f);
            isWalking = false;
            isRunning = false;
        } else {
            float xMove = Input.GetAxisRaw("Horizontal");
            float zMove = Input.GetAxisRaw("Vertical");
            Vector3 move = transform.right * xMove + transform.forward * zMove;
            player.Move(move * speed * Time.deltaTime); 
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
            {   
                speed = sprintSpeed;
                isRunning = true;
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
                    isRunning = false;
                isWalking = true;
            }
            } else {
                speed = baseSpeed;
                isRunning = false;
            staminaBar.value += 0.002f;
            }
            if(!isRunning && (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D))){
            isWalking = true;
        } else {
            isWalking = false;
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
        HandleRunSound();
        HandleWalkSound();
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "EnemyChild") {
            gameOver = true;
            enemyPosition = collision.gameObject.transform.position;
            GameObject.Find("Enemy3fixed").GetComponent<ChaseCamera>().SetFinalPos(enemyPosition);
            enemyPosition = new Vector3(enemyPosition.x, 0.5f, enemyPosition.z);
            GameObject.Find("Enemy3fixed").GetComponent<ChaseCamera>().SetGameOver(true);
            GetComponent<CapsuleCollider>().enabled = false;
            camera.GetComponent<CameraController>().SetGameOver(true);
            uiManagerScript.GameOver();
            GameObject.Find("Timer").SetActive(false);
        }
        if (collision.gameObject.tag == "Enemy2") {
            gameOver = true;
            enemyPosition = collision.gameObject.transform.position;
            GameObject.Find("Enemy2fixed").GetComponent<ChaseCamera>().SetFinalPos(enemyPosition);
            enemyPosition = new Vector3(enemyPosition.x, 1f, enemyPosition.z);
            GameObject.Find("Enemy2fixed").GetComponent<ChaseCamera>().SetGameOver(true);
            GetComponent<CapsuleCollider>().enabled = false;
            camera.GetComponent<CameraController>().SetGameOver(true);
            uiManagerScript.GameOver();
            GameObject.Find("Timer").SetActive(false);
        }
    }

    void OnTriggerEnter(Collider enemy) {
        if (enemy.gameObject.tag == "Enemy1") {
            gameOver = true;
            enemyPosition = enemy.gameObject.transform.position;
            GameObject.Find("Enemy1").GetComponent<WalkThroughWalls>().SetFinalPos(enemyPosition);
            enemyPosition = new Vector3(enemyPosition.x, 0.7f, enemyPosition.z);
            GameObject.Find("Enemy1").GetComponent<WalkThroughWalls>().SetGameOver(true);
            GetComponent<CapsuleCollider>().enabled = false;
            camera.GetComponent<CameraController>().SetGameOver(true);
            uiManagerScript.GameOver();
            GameObject.Find("Timer").SetActive(false);
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

    public void SetSoundFalse(){
        isWalking=false;
        isRunning=false;
        walk.Stop();
        run.Stop();
        walkHouse.Stop();
        runHouse.Stop();
    }

    private void HandleWalkSound()
    {
        if(isWalking && !walk.isPlaying){
            walk.Play();
        } else if (!isWalking || inHouseScript.inHouse){
            walk.Stop();
        }
        if(isWalking && !walkHouse.isPlaying && inHouseScript.inHouse){
            walkHouse.Play();
        } else if (!isWalking || !inHouseScript.inHouse){
            walkHouse.Stop();
        }
    }

    private void HandleRunSound()
    {
        if(isRunning && !inHouseScript.inHouse && !run.isPlaying){
            run.Play();
        } else if (!isRunning || inHouseScript.inHouse){
            run.Stop();
        }
        if(isRunning && !runHouse.isPlaying && inHouseScript.inHouse){
            runHouse.Play();
        } else if (!isRunning || !inHouseScript.inHouse){
            runHouse.Stop();
        }
    }
}
