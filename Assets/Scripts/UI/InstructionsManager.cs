using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstructionsManager : MonoBehaviour
{
    private List<GameObject> popups;
    private int currPopupIndex = 0;
    private CanvasGroup currCanvas;
    private float displayTime = 2.0f;
    private float startValue;
    private float endValue;
    private float elapsedTime;
    private float fadeDuration = 0.8f;

    void Start()
    {
        popups = new List<GameObject>();
        foreach (Transform popup in GetComponentsInChildren<Transform>())
        {
            popups.Add(popup.gameObject);
        }
        ShowInstruction();
    }
    
    void Update()
    {
        if (currCanvas == null)
        {
            return;
        }
        
        if (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            currCanvas.alpha = Mathf.Lerp(startValue, endValue, elapsedTime / fadeDuration);
        }
    }

    private void ShowInstruction()
    {
        if (currCanvas != null) currCanvas.alpha = 0.0f;
        currCanvas = popups[currPopupIndex].GetComponent<CanvasGroup>();
        currPopupIndex += 1;
        FadeIn();
    }

    private void FadeIn()
    {
        startValue = 0.0f;
        endValue = 1.0f;
        elapsedTime = 0.0f;
        Invoke("FadeOut", fadeDuration + displayTime);
    }

    private void FadeOut()
    {
        startValue = 1.0f;
        endValue = 0.0f;
        elapsedTime = 0.0f;
        if (currPopupIndex < popups.Count)
        {
            Invoke("ShowInstruction", fadeDuration);
        }
    }
}
