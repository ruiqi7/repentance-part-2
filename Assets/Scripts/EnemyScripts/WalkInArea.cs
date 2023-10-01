using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkInArea : MonoBehaviour
{
    [SerializeField] private float distractDuration;
    [SerializeField] private float moveRadius;
    [SerializeField] private float speed;
    
    private ChaseCamera chaseCamera;
    private Rigidbody rb;
    private Vector3 targetPosition;
    private float minX, maxX, minZ, maxZ;
    
    void Start()
    {
        chaseCamera = GetComponent<ChaseCamera>();
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "DistractArea")
        {
            Invoke("EndDistraction", distractDuration);
            chaseCamera.enabled = false;
            Vector3 dollPosition = collider.gameObject.transform.parent.position;
            minX = dollPosition.x - moveRadius;
            maxX = dollPosition.x + moveRadius;
            minZ = dollPosition.z - moveRadius;
            maxZ = dollPosition.z + moveRadius;
            targetPosition = dollPosition;
            MoveRandom();
        }
    }

    void Update()
    {
        if (chaseCamera.enabled)
        {
            return;
        }
        
        RaycastHit hit;
        Vector3 direction = targetPosition - transform.position;
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            MoveRandom();
        } 
    }

    private void MoveRandom()
    {
        if (Vector3.Distance(targetPosition, transform.position) <= 1)
        {
            targetPosition = GetRandomTarget();
        }

        RaycastHit hit;
        Vector3 direction = targetPosition - transform.position;
        if (Physics.Raycast(transform.position, direction, out hit, 1.0f))
        {
            targetPosition = GetRandomTarget();
        }
        Vector3 newPos = Vector3.MoveTowards(transform.position, targetPosition, speed);
        rb.MovePosition(new Vector3(newPos.x, transform.position.y, newPos.z));
    }

    private Vector3 GetRandomTarget()
    {
        Vector3 newPos = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
        transform.LookAt(new Vector3(newPos.x, 0, newPos.z));
        return newPos;
    }

    private void EndDistraction()
    {
        chaseCamera.enabled = true;
    }
}
