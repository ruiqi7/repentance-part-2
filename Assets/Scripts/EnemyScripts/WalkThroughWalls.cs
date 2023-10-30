using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkThroughWalls : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    [SerializeField] private float minX, maxX;
    [SerializeField] private float minZ, maxZ;
    [SerializeField] private Camera camera;
    private Animator animator;
    private Material cameraMat;
    private BoxCollider bc;
    private float startTime;
    private Vector3 targetPosition;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource growl;
    private int noticed = 0;
    private bool handling = false;
    private Rigidbody rb;
    private bool isRepelled = false;
    private bool flickering = false;
    public bool gameOver = false;
    private Vector3 finalPos;
    void Start()
    {
        transform.LookAt(target.transform.position);
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        bc = GetComponent<BoxCollider>();
        cameraMat = camera.GetComponent<PostProcess>().material;
    }


    public void SetGameOver(bool var) {
        gameOver = var;
    }

    public void SetFinalPos(Vector3 pos) {
        finalPos = pos;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(gameOver) {
            animator.enabled = false;
            bc.enabled = false;
            transform.position = finalPos;
        } else if(!isRepelled) {
            if(Vector3.Distance(target.transform.position, transform.position) < distance) {
                if(!handling) {
                    StartCoroutine(HandleAudio());
                }
                if(!flickering && Vector3.Distance(target.transform.position, transform.position) < 20) {
                    StartCoroutine(ShaderFlicker());
                }
                if(noticed == 0){
                    NoticeAudio();
                    noticed = 1;
                }
                Vector3 newPos = Vector3.MoveTowards(transform.position, target.transform.position, speed*2);
                transform.position = new Vector3(newPos.x,transform.position.y, newPos.z);
                transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
            } else if(Time.time - startTime >= 10) {
                transform.position = GetRandomTarget();
                startTime = Time.time;
                targetPosition = GetRandomTarget();
                noticed = 0;
            } else {
                Vector3 newPos;
                string difficulty = PlayerPrefs.GetString("difficulty");
                if (difficulty == "Easy")
                {
                    newPos = Vector3.MoveTowards(transform.position, targetPosition, speed);
                }
                else
                {
                    newPos = Vector3.MoveTowards(transform.position, targetPosition, speed*1.5f);
                }
                transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
                transform.LookAt(targetPosition);
                noticed = 0;
            }
        }
    }

     private Vector3 GetRandomTarget() {
        return new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ,maxZ));
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "RepelArea")
        {
            isRepelled = true;
            Vector3 collisionPoint = collider.ClosestPoint(transform.position).normalized;
            rb.AddForce(collisionPoint * 300.0f);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "RepelArea")
        {
            Invoke("StopRepulsion", 0.3f);
        }
    }

    private void StopRepulsion()
    {
        rb.velocity = Vector3.zero;
        isRepelled = false;
    }

    private IEnumerator HandleAudio() {
        handling = true;
        audioSource.Play();
        yield return new WaitForSeconds(15);
        handling = false;
    }
    IEnumerator ShaderFlicker() {
        flickering = true;
        for(int i = 0; i < 2; i ++) {
            if(cameraMat.GetFloat("_Active") == 1f) {
                cameraMat.SetFloat("_Active", 0);
            } else {
                cameraMat.SetFloat("_Active", 1f);
            }
            float wait = Random.Range(0.1f, .2f);
            yield return new WaitForSeconds(wait);
        }
        yield return new WaitForSeconds(1);
        flickering = false;
    }

    private void NoticeAudio(){
        growl.Play();
    }
}