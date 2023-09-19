/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                11/4/2021
 *  Contributor:       Haewon Shon
 *  Description:       Specialized Mainmenu for M4 build
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_M4 : MonoBehaviour
{
    private AudioManager audioManager;
    void Awake()
    {
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("UI"));
        Cursor.visible = true;
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void Forest()
    {
        LoadScene("Forest_M4");
    }

    public void Elevator()
    {
        LoadScene("Elevator_M4");
    }

    public void Boss()
    {
        LoadScene("Boss_M4");
    }

    public void Test()
    {
        LoadScene("MainMenu_M3");
    }

    public void QuitGame()
    {
        audioManager.Play("ButtonClick");
        Debug.Log("Quit");
        Application.Quit();
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

    // Sinil - Play music sound
    public void PlayGameThemeMusic()
    {
        audioManager.PlayBackgroundMusic(AudioManager.BGMType.LegacyGameBGM);
    }

    // Sinil - function which load scene
    private void LoadScene(string s)
    {
        audioManager.Play("ButtonClick");
        PlayGameThemeMusic();
        SceneManager.LoadScene(s);

        //Suhwan working (if there is some error, plz tell me)
        if (s != "MainMenu_M3" && s != "MainMenu_M4" && s != "PrototypeMenu_M4")
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }
}
