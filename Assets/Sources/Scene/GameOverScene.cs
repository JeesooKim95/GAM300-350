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


public class GameOverScene : MonoBehaviour
{
    AudioManager audioManager;
    private string nextSceneName;
    private GameObject player;
    private GameObject UI;
    public void Start()
    {
        player = GameObject.Find("Player");
        UI = GameObject.Find("UI");
        PlayerProgress progress = player.GetComponent<PlayerProgress>();
        if (progress != null)
        {
            if (progress.GetPlayerProgress(PlayerProgress.Progress.FOREST) == false)
            {
                nextSceneName = "TutorialToForest";
            }
            else if (progress.GetPlayerProgress(PlayerProgress.Progress.DESERT) == false)
            {
                nextSceneName = "ForestToDesert";
            }
            else if (progress.GetPlayerProgress(PlayerProgress.Progress.SNOW) == false)
            {
                nextSceneName = "DesertToSnow";
            }
            else
            {
                nextSceneName = "SnowToBoss";
            }
        }
        player.SetActive(false);
        UI.SetActive(false);
        //Destroy(GameObject.Find("Player"));
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void Restart()
    {
        PlayButtonClickSound();
        player.SetActive(true);
        UI.SetActive(true);

        /////////////////////////////////////////////
        /// Initialize player-related stuffs 
        /////////////////////////////////////////////

        player.GetComponent<PlayerStatus>().SetHealth(50);
        player.GetComponent<PlayerStatus>().ResetShield(20);
        player.GetComponent<Skills>().SetDefaultStatus(false);
        FindObjectOfType<PlayerHandsManager>().ClearHands();
        player.GetComponent<PlayerMovement>().Knockback(Vector3.zero, 0.0f); // initialize knockback

        /////////////////////////////////////////////
        /// Initialize player-related stuffs 
        /////////////////////////////////////////////

        SceneManager.LoadScene(nextSceneName);
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
