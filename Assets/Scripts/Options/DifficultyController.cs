using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyController : MonoBehaviour
{
    void Start()
    {
        if (!PlayerPrefs.HasKey("difficulty"))
        {
            PlayerPrefs.SetString("difficulty", "Easy");
        }
        LoadDifficulty();
    }

    public void AdjustDifficulty()
    {
        string difficulty = gameObject.name.Substring(0, 4);
        SaveDifficulty(difficulty);
        LoadDifficulty();
    }

    private void LoadDifficulty()
    {
        TextMeshProUGUI selectedDifficultyText;
        TextMeshProUGUI unselectedDifficultyText;
        
        string difficulty = PlayerPrefs.GetString("difficulty");
        if (difficulty == "Easy")
        {
            selectedDifficultyText = transform.parent.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            unselectedDifficultyText = transform.parent.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        else
        {
            selectedDifficultyText = transform.parent.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            unselectedDifficultyText = transform.parent.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        selectedDifficultyText.color = new Color(255, 255, 255, 200);
        unselectedDifficultyText.color = new Color(255, 255, 255, 0.5f);
    }

    private void SaveDifficulty(string difficulty)
    {
        PlayerPrefs.SetString("difficulty", difficulty);
    }
}
