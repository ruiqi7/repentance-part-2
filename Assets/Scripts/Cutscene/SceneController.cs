using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject text1;
    [SerializeField] private GameObject text2;
    [SerializeField] private GameObject text3;
    [SerializeField] private GameObject text4;
    private bool text1Done = false;
    private bool text2Done = false;
    private bool text3Done = false;
    private bool text4Done = false;
    private bool handling = false;

    // Update is called once per frame
    void Update()
    {
        if(!handling) {
            if(!text1Done) {
                StartCoroutine(HandleText(text1, 2));
                text1Done = true;
            } else if(!text2Done) {
                StartCoroutine(HandleText(text2, 2));
                text2Done = true;
            }else if(!text3Done) {
                StartCoroutine(HandleText(text3, 2));
                text3Done = true;
            }else if(!text4Done) {
                StartCoroutine(HandleText(text4, 2));
                text4Done = true;
            }
        }
        if(text1Done && text2Done && text3Done && text4Done) {
             SceneManager.LoadScene("MazeGeneration");
        }
    }

    private IEnumerator HandleText(GameObject text, int time) {
        handling = true;
        text.SetActive(true);
        yield return new WaitForSeconds(time);
        text.SetActive(false);
        handling = false;
    }
}
