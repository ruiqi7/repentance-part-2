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
    private float startTime;
    private Vector3 targetPosition;
    [SerializeField] private AudioSource audioSource;
    private bool handling = false;
    private bool flickering = false;
    void Start()
    {
        transform.LookAt(target.transform.position);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector3.Distance(target.transform.position, transform.position) < distance) {
            if(!handling) {
                StartCoroutine(HandleAudio());
            }
            if(!flickering && Vector3.Distance(target.transform.position, transform.position) < 20) {
                StartCoroutine(ShaderFlicker());
            }
            Vector3 newPos = Vector3.MoveTowards(transform.position, target.transform.position, speed*2);
            transform.position = new Vector3(newPos.x,transform.position.y, newPos.z);
            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        } else if(Time.time - startTime >= 10) {
            transform.position = GetRandomTarget();
            startTime = Time.time;
            targetPosition = GetRandomTarget();
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
        }
    }

     private Vector3 GetRandomTarget() {
        return new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ,maxZ));
    }

    private IEnumerator HandleAudio() {
        handling = true;
        audioSource.Play();
        yield return new WaitForSeconds(15);
        handling = false;
    }
    IEnumerator ShaderFlicker() {
        flickering = true;
        var temp = camera.GetComponent<Camera>().GetComponent<PostProcess>().material;
        for(int i = 0; i < 2; i ++) {
            if(temp.GetFloat("_Active") == 1f) {
                temp.SetFloat("_Active", 0);
            } else {
                temp.SetFloat("_Active", 1f);
            }
            float wait = Random.Range(0.1f, .2f);
            yield return new WaitForSeconds(wait);
        }
        yield return new WaitForSeconds(1);
        flickering = false;
    }
}