/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                10/3/2021
 *  Contributor:       Su Kim
 *  Description:       Main logic for Pause Menu
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused;

    // Sinil - for sake of playing audio
    private AudioManager audioManager;

    public GameObject pausedMenuUI;

    // Haewon - Leaving confirmation window
    public GameObject confirmWindow;
    private bool isLeavingToMenu = false;
    private bool isQuittingGame = false;

    void Start()
    {
        GameIsPaused = false;
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PlayButtonClickSound();
        pausedMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pausedMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMenu()
    {
        if (isLeavingToMenu == false)
        {
            isLeavingToMenu = true;
            confirmWindow.SetActive(true);
        }
        else
        {
            isLeavingToMenu = false;
            PlayButtonClickSound();
            Time.timeScale = 1f;
            GameIsPaused = false;
            pausedMenuUI.SetActive(false);
            SceneManager.LoadScene("M5_MainMenu");
        }
    }

    public void QuitGame()
    {
        if (isQuittingGame == false)
        {
            isQuittingGame = true;
            confirmWindow.SetActive(true);
        }
        else 
        {
            isQuittingGame = false;
            PlayButtonClickSound();
            Application.Quit();
        }
    }

    // Haewon - confirmation window
    public void ConfirmYes()
    {
        confirmWindow.SetActive(false);
        
        if (isLeavingToMenu == true)
        {
            LoadMenu();
        }
        else if (isQuittingGame == true)
        {
            QuitGame();
        }
    }

    public void ConfirmNo()
    {
        confirmWindow.SetActive(false);
        isQuittingGame = false;
        isLeavingToMenu = false;
    }

    // Sinil - for sake of playing sound
    private void PlayButtonClickSound()
    {
        audioManager.Play("ButtonClick");
    }
}
