using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> pages;
    [SerializeField] private bool allowPause = false;
    [SerializeField] private int pausePageIndex = 0;
    [SerializeField] private int gameOverPageIndex = 0;
    [SerializeField] private int gameWonPageIndex = 0;
    [SerializeField] private GameObject audioManager;

    private bool isPaused = false;
    private float timePassed = 0;
    private CameraController cameraController;

    public float getTimePassed() {
        return timePassed;
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
        if (!isPaused && currentScene.name == "Maze-enemies")
        {
            timePassed += Time.deltaTime;
        }
        if (currentScene.name == "Maze-enemies" && timePassed >= 300 && !isPaused)
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

    private void PauseGame()
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
        ChangePage(gameOverPageIndex);
        PauseGame();
        allowPause = false;
        audioManager.GetComponent<AudioManager>().GameOverMusic();
    }

    private void GameWon()
    {
        ChangePage(gameWonPageIndex);
        PauseGame();
        allowPause = false;
        audioManager.GetComponent<AudioManager>().GameWonMusic();
    }
}
