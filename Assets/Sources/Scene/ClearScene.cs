/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                9/25/2021
 *  Contributor:       Su Kim
 *  Description:       Basic MainMenu system
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ClearScene : MonoBehaviour
{
    AudioManager audioManager;

    public void Start()
    {
        Destroy(GameObject.Find("SceneObjectManager_Inherited"));
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        audioManager = FindObjectOfType<AudioManager>();
    }
    public void ToMainMenu()
    {
        PlayButtonClickSound();
        SceneManager.LoadScene("M5_MainMenu");
    }

    public void QuitGame()
    {
        PlayButtonClickSound();
        Debug.Log("Quit");
        Application.Quit();
    }


    // Sinil - for sake of playing sound
    private void PlayButtonClickSound()
    {
        audioManager.Play("ButtonClick");
    }
}
