/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 09/19/2021
    Desc    : Script for portal(temporary) which allows move to another scene. Attached to portal object
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string nextSceneName;
    private bool isActivated = false;

    

    // Sinil - for sake of playing portal environmental sounds
    private AudioManager audioManager;
    private const float time = 4.0f;
    private float localTimer = time;

    public GameObject portalRenderer;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        localTimer = time;
    }

    private void Update()
    {
        if(isActivated)
        {
            localTimer -= Time.deltaTime;

            if (localTimer < 0f)
            {
                localTimer = time;

                audioManager.PlaySpatial("PortalEnv", gameObject.transform.position);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(isActivated == true)
        {
            Debug.Log("Portal entered");
            if(other.gameObject.tag == "Player")
            {
                Debug.Log(nextSceneName + "is Loading..");
                FindObjectOfType<AudioManager>().Play("Portal");
                SceneManager.LoadScene(nextSceneName);

                UpdatePlayerProgress(other.gameObject);

                if (nextSceneName == "ClearScene")
                {
                    Destroy(other.gameObject);
                }

                // haewon 02/03/2022, disable boss health bar for later level when leave
                GameObject bossHealthBar = GameObject.Find("Boss Health Bar");
                if (bossHealthBar)
                    bossHealthBar.SetActive(false);
            }
        }
    }

    public void SetNextSceneName(string sceneName)
    {
        nextSceneName = sceneName;
    }

    public void Activate()
    {
        isActivated = true;
        gameObject.GetComponent<Renderer>().material.color = Color.blue;

        if(portalRenderer != null)
        {
            portalRenderer.SetActive(true);
        }
    }



    // Sinil.kang
    // for sake of saving player's progress - lobby portal activate
    public void UpdatePlayerProgress(GameObject player)
    {
        if (SceneManager.GetActiveScene().name == "M5_Forest")
        {
            player.GetComponent<PlayerProgress>().SetPlayerProgress(PlayerProgress.Progress.FOREST);
        }
        if (SceneManager.GetActiveScene().name == "M5_Desert")
        {
            player.GetComponent<PlayerProgress>().SetPlayerProgress(PlayerProgress.Progress.DESERT);
        }
        if (SceneManager.GetActiveScene().name == "M5_Snowfield")
        {
            player.GetComponent<PlayerProgress>().SetPlayerProgress(PlayerProgress.Progress.SNOW);
        }
    }
}
