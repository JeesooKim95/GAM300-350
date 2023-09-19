/*
    Team    : Speaking Potato
    Author  : Sangmin Kim
    Date    : 10/23/2021
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSceneLevelManager : MonoBehaviour
{
    public GameObject[] enemyObj;
    public GameObject[] spawnPoses;
    void Start()
    {
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.R))
        // {
        //     SpawnEnemies();
        // }
    }
    void SpawnEnemies()
    {
        for(int k = 0; k < 3; ++k)
        {
            for(int i = 0; i < 4; ++i)
            {
                GameObject enemy = Instantiate(enemyObj[i], spawnPoses[i].transform.position, Quaternion.identity);
            }
        }

    }
}
