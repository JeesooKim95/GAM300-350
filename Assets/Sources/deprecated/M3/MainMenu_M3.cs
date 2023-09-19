/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                10/24/2021
 *  Contributor:       Haewon Shon
 *  Description:       Specialized Mainmenu for M3 build
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu_M3 : MonoBehaviour
{
    private AudioManager audioManager;
    void Awake()
    {
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("UI"));
        Cursor.visible = true;
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void Shooting()
    {
        LoadScene("Shooting_M3");
    }

    public void Enemy()
    {
        LoadScene("Enemy_M3");    }

    public void Boss()
    {

        LoadScene("Boss_M3");
    }
    public void Upgrade()
    {

        LoadScene("Upgrade_M3");
    }

    public void Player()
    {
    
        LoadScene("Player_M3");
    }

    public void M4Menu()
    {
        LoadScene("MainMenu_M4");
    }

    public void QuitGame()
    {
        audioManager.Play("ButtonClick");
        Debug.Log("Quit");
        Application.Quit();
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

        // Suhwan working (if there is some error, please tell me)
        if (s != "MainMenu_M4")
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
