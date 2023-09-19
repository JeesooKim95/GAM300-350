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


public class MainMenu : MonoBehaviour
{
    void Awake()
    {
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("UI"));
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitGame()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        Debug.Log("Quit");
        Application.Quit();
    }
}
