using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyController : MonoBehaviour
{
    private Color color;

    void Start()
    {
        if (!PlayerPrefs.HasKey("difficulty"))
        {
            PlayerPrefs.SetInt("difficulty", 0);
        }
        LoadDifficulty();
    }

    public void AdjustDifficulty()
    {
        int index = transform.GetSiblingIndex();
        SaveDifficulty(index);
        LoadDifficulty();
    }

    private void LoadDifficulty()
    {
        int difficulty = PlayerPrefs.GetInt("difficulty");
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            Transform sibling = transform.parent.GetChild(i);
            Image siblingImage = sibling.gameObject.GetComponent<Image>();
            Color siblingColor = siblingImage.color;
            if (i <= difficulty)
            {
                siblingColor.a = 1.0f;
            }
            else
            {
                siblingColor.a = 0.5f;
            }
            siblingImage.color = siblingColor;
        }
    }

    private void SaveDifficulty(int difficulty)
    {
        PlayerPrefs.SetInt("difficulty", difficulty);
    }
}
