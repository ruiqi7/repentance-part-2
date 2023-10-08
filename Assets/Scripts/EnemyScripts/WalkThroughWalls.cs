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
    private Animator animator;
    private float startTime;
    private Vector3 targetPosition;
    [SerializeField] private AudioSource audioSource;
    private bool handling = false;
    void Start()
    {
        transform.LookAt(target.transform.position);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //float normalisedSpeed = speed * (Time.time/300);
        if(Vector3.Distance(target.transform.position, transform.position) < distance) {
            if(!handling) {
                StartCoroutine(HandleAudio());
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
}