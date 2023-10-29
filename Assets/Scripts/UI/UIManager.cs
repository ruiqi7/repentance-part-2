using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> pages;
    [SerializeField] private bool allowPause = false;
    [SerializeField] private int pausePageIndex = 0;
    [SerializeField] private int gameOverPageIndex = 0;
    [SerializeField] private int gameWonPageIndex = 0;
    [SerializeField] private GameObject audioManager;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] float timeRemaining = 300;
    [SerializeField] TMP_Text text;
    private bool running = true;

    private bool isPaused = false;
    private CameraController cameraController;

    public float getTimePassed() {
        return 300 - timeRemaining;
    }

    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        TogglePause();
        TogglePause();
    }

    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (Input.GetKeyDown("escape"))
        {
            TogglePause();
        }
        if (!isPaused && currentScene.name == "MazeGeneration")
        {
            if(running)
            {
                if(timeRemaining > 0) 
                {
                    timeRemaining -= Time.deltaTime;
                    if(timeRemaining < 0) 
                    {
                        timeRemaining = 0;
                        running = false;
                    }
                }   
                if(timeRemaining > 295 || (timeRemaining > 145 && timeRemaining <= 151) || (timeRemaining > 27 && timeRemaining <= 31) ) {
                    float minutes = Mathf.FloorToInt(timeRemaining / 60);
                    float seconds = Mathf.FloorToInt(timeRemaining % 60);
                    text.text = string.Format("{0:0}:{1:00}", minutes, seconds);
                } else {
                    text.text = "";
                }
            }
        }
        if (currentScene.name == "MazeGeneration" && timeRemaining <= 0 && !isPaused)
        {
            GameWon();
        } 
    }

    private void TogglePause()
    {
        if (allowPause)
        {
            if (isPaused)
            {
                pages[pausePageIndex].gameObject.SetActive(false);
                ResumeGame();
            }
            else
            {
                ChangePage(pausePageIndex);
                PauseGame();
            }
        }      
    }

    private void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraController.enabled = true;
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cameraController.enabled = false;
        Time.timeScale = 0.0f;
        isPaused = true;
    }

    public void ChangePage(int pageIndex)
    {
        foreach (GameObject page in pages)
        {
            page.gameObject.SetActive(false);
        }
        pages[pageIndex].gameObject.SetActive(true);
    }

    public void GameOver()
    {
        dialogueBox.SetActive(false);
        ChangePage(gameOverPageIndex);
        //PauseGame();
        allowPause = false;
        audioManager.GetComponent<AudioManager>().GameOverMusic();
    }

    private void GameWon()
    {
        dialogueBox.SetActive(false);
        ChangePage(gameWonPageIndex);
        PauseGame();
        allowPause = false;
        audioManager.GetComponent<AudioManager>().GameWonMusic();
    }
}
