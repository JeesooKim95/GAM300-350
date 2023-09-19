/*
    Team    : Speaking Potato
    Author  : Sinil.kang
    Date    : 01/26/2022
    Desc    : Script for storing player's progress.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    private bool isForestDone = false;
    private bool isDesertDone = false;
    private bool isSnowDone = false;

    public enum Progress
    {
        FOREST,
        DESERT,
        SNOW
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerProgress(Progress p)
    {
        switch(p)
        {
            case Progress.FOREST:
                isForestDone = true;
                break;
            case Progress.DESERT:
                isDesertDone = true;
                break;
            case Progress.SNOW:
                isSnowDone = true;
                break;
            default:
                break;
        }

        return;
    }

    public bool GetPlayerProgress(Progress p)
    {
        switch (p)
        {
            case Progress.FOREST:
                return isForestDone;
            case Progress.DESERT:
                return isDesertDone;
            case Progress.SNOW:
                return isSnowDone;
            default:
                return false;
        }
    }
}
