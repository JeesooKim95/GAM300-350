/*
    Team    : Speaking Potato
    Author  : Sinil.kang
    Date    : 01/26/2022
    Desc    : Script for managing portals in lobby.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPortalManager : MonoBehaviour
{
    PlayerProgress progress;
    public Portal forestPortal;
    public Portal desertPortal;
    public Portal snowPortal;


    private bool isBossTime = false;


    // Start is called before the first frame update
    void Start()
    {
        isBossTime = false;

        progress = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerProgress>();
        bool isForestDone = progress.GetPlayerProgress(PlayerProgress.Progress.FOREST);
        bool isDesertDone = progress.GetPlayerProgress(PlayerProgress.Progress.DESERT);
        bool isSnowDone = progress.GetPlayerProgress(PlayerProgress.Progress.SNOW);


        if (isForestDone == false)
        {
            forestPortal.Activate();
        }
        else
        {

        }

        if (isDesertDone == false)
        {
            desertPortal.Activate();
        }
        else
        {

        }

        if (isSnowDone == false)
        {
            snowPortal.Activate();
        }
        else
        {

        }

        if(isForestDone && isDesertDone && isSnowDone)
        {
            isBossTime = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isBossTime == true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (5f * Time.deltaTime), transform.position.z);

            if(transform.position.y <= -50f)
            {
                isBossTime = false;
            }
        }
    }
}
