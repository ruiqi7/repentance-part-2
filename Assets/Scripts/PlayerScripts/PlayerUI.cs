using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI promptText;
    [SerializeField] public TMP_FontAsset font;
    void Start()
    {
        
    }
    
    public void updateText(string promptMsg){
        promptText.text = promptMsg; 
    }
    public void updateFont(TMP_FontAsset font){
        promptText.font = font;
    }
}
