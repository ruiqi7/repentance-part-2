using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject enemy1;
    [SerializeField] private GameObject enemy2;
    [SerializeField] private GameObject enemy3;
    // Start is called before the first frame update
    void Start()
    {
        enemy1.SetActive(false);
        enemy2.SetActive(false);
        enemy3.SetActive(false);
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer() {
        yield return new WaitForSecondsRealtime(15);
        enemy1.SetActive(true);
        enemy2.SetActive(true);
        enemy3.SetActive(true);
    }

}
