using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed;
    [SerializeField] private float minX, maxX;
    [SerializeField] private float minZ, maxZ;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource growl;
    private int noticed = 0;
    [SerializeField] private Camera camera;
    private Material cameraMat;
    private bool handling = false;
    private Rigidbody rb;
    private Vector3 targetPosition;
    private Animator animator;
    private bool isRepelled = false;
    private BoxCollider bc;
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

    private Vector3 GetRandomTarget() {
        Vector3 newPos = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ,maxZ));
        transform.LookAt(new Vector3(newPos.x, 0, newPos.z));
        return newPos;
    }

    public void SetFinalPos(Vector3 pos) {
        finalPos = pos;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver) {
            animator.enabled = false;
            bc.enabled = false;
            transform.position = finalPos;
        } else if(!isRepelled) {
            RaycastHit hit;
            Vector3 direction = target.transform.position - transform.position;
            Debug.DrawRay(transform.position, direction, Color.yellow);
            if(Physics.Raycast(transform.position, direction, out hit)) {
                if(hit.collider.tag == "Generated") {
                    moveRandom();
                    noticed = 0;
            } else if(hit.collider.tag == "Player") {
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
                    rb.MovePosition(new Vector3(newPos.x, transform.position.y, newPos.z));
                    transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
                } else {
                    moveRandom();
                    noticed = 0;
            }
            } 
        }
    }

    IEnumerator ShaderFlicker() {
        flickering = true;
        for(int i = 0; i < 2; i ++) {
            if(cameraMat.GetFloat("_Active") == 1f) {
                cameraMat.SetFloat("_Active", 0);
            } else {
                cameraMat.SetFloat("_Active", 1f);
            }
            float wait = Random.Range(0.2f, 0.4f);
            yield return new WaitForSeconds(wait);
        }
        flickering = false;
    }

    private void moveRandom() {
        //float normalisedSpeed = speed * (Time.time/300);
        if(Vector3.Distance(targetPosition, transform.position) <= 1) {
            targetPosition = GetRandomTarget();
        }
        RaycastHit hit;
        Vector3 direction = targetPosition - transform.position;
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y+1, transform.position.z), direction, Color.green);
        if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y+1, transform.position.z), direction, out hit, 4.0f)) {
            if(hit.collider.tag == "Generated") {
                targetPosition = GetRandomTarget();
            }
        }
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
        rb.MovePosition(new Vector3(newPos.x, transform.position.y, newPos.z));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "RepelArea")
        {
            isRepelled = true;
            rb.AddForce(collision.contacts[0].normal * 300.0f);
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

    private void NoticeAudio(){
        growl.Play();
    }
}
