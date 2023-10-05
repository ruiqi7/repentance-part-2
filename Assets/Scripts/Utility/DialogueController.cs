using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueController : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI textBox;
    [SerializeField] public string[] lines;
    [SerializeField] public float textSpeed = 0.1f;
    [SerializeField] public Color textColor = Color.white;
    private int lineIndex;
    
    public void Start(){
        if(SceneManager.GetActiveScene().name == "IntroScene"){
            textBox.text = String.Empty;
            StartCoroutine(IntroDialogue());
        }
    }
    public void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            if (textBox.text == lines[lineIndex]){
                NextLine();
            }
            else{
                StopAllCoroutines();
                textBox.text = lines[lineIndex];
                StartCoroutine(WaitLine());
            }
        }
    }


    public void StartDialogue(){
        textBox.text = String.Empty;
        textBox.color = textColor;
        lineIndex = 0;
        StartCoroutine(TypeLine());
        
    }
    public void NextLine(){
        if(lineIndex+1 < lines.Length){
            textBox.text = String.Empty;
            lineIndex += 1;
            StartCoroutine(TypeLine());
        } else{
            gameObject.SetActive(false);
        }
    }
    IEnumerator TypeLine(){
        foreach(char c in lines[lineIndex].ToCharArray()){
            textBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        yield return new WaitForSeconds(2);
        NextLine();
    }
    IEnumerator IntroDialogue(){
        yield return new WaitForSeconds(5);
        StartDialogue();
    }
    IEnumerator WaitLine(){
        yield return new WaitForSeconds(2);
        NextLine();
    }
}
