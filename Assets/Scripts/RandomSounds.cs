using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private int minTime, maxTime;
    private int targetTime = 0;
    private int startTime = 0;
    private bool handling = false;
    void Start()
    {
        targetTime = NewTargetTime();
    }

    private int NewTargetTime() {
        return Random.Range(minTime, maxTime);
    }
    void Update()
    {
        if(Time.time - startTime >= targetTime) {
            if(!handling) {
                StartCoroutine(HandleAudio());
            }
            targetTime = NewTargetTime();
        }
    }

    private IEnumerator HandleAudio() {
        handling = true;
        audioSource.Play();
        yield return new WaitForSeconds(15);
        handling = false;
    }
}
