/*  Class:               GAM350
 *  Team name:      Speaking Potato
 *  Date:                01/13/2022
 *  Contributor:       Su Kim
 *  Description:       Specialized Mainmenu for M5 build
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class M5_MainMenu : MonoBehaviour
{
    private AudioManager audioManager;

    // Haewon - Leaving confirmation window
    public GameObject confirmWindow;
    private bool isQuittingGame = false;

    void Awake()
    {
        //Destroy(GameObject.Find("Player"));
        //Destroy(GameObject.Find("UI"));
        Destroy(GameObject.Find("SceneObjectManager_Inherited"));
        Cursor.visible = true;
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void Forest()
    {
        LoadScene("M5_Forest");
    }

    public void Snow()
    {
        LoadScene("M5_Snowfield");
    }
    public void Desert()
    {
        LoadScene("M5_Desert");
    }

    public void Boss()
    {
        LoadScene("M5_Boss");
    }

    public void Test()
    {
        LoadScene("MainMenu_M3");
    }

    public void Tutorial()
    {
        LoadScene("M5_ShootingTutorial");
    }

    public void Credit()
    {
        LoadScene("Credits");
    }

    // Haewon, add destructive window into main menu
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

        if (isQuittingGame == true)
        {
            QuitGame();
        }
    }

    public void ConfirmNo()
    {
        confirmWindow.SetActive(false);
        isQuittingGame = false;
    }

    // Add for prototype - Su

    public void M4()
    {
        LoadScene("MainMenu_M4");
    }

    public void Prototype()
    {
        LoadScene("PrototypeMenu_M4");
    }
        
    // Sinil - function which load scene
    private void LoadScene(string s)
    {
        audioManager.Play("ButtonClick");
        SceneManager.LoadScene(s);

        //Suhwan working (if there is some error, plz tell me)
        if (s != "MainMenu_M3" && s != "MainMenu_M4" && s != "PrototypeMenu_M4"
            && s != "M5_MainMenu" && s != "Credits")
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

    // Sinil - for sake of playing sound
    private void PlayButtonClickSound()
    {
        audioManager.Play("ButtonClick");
    }
}
