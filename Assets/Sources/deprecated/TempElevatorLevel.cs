/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 11/16/2021
    Desc    : temporary script for elevator level - to control light and spawn delay, etc together
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempElevatorLevel : LevelManager
{
    [Header("custom level only")]
    public Transform center; // to spawn purpose
    private float eventTimer = 0.0f;
    private bool isLevelBegin = false;

    [Header("Light control : first light should be spot light in the center")]
    public float lightsOffTimer = 1f;
    public Light[] lights;
    private bool isLightsOn = true;
    private bool isSpotLightOn = false;

    private float distanceToSpawnRange = -1.0f;

    // Sinil - For sake of playing elevator sounds
    AudioManager audioManager;

    private void Start()
    {
        // Set player's initial position
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerMovement>().InitPosition(playerSpawnPosition.position);

        // Set player's respawn position
        PlayerRespawn respawn = player.GetComponent<PlayerRespawn>();
        if (respawn)
            respawn.SetSavePoint(portal.transform.position);

        // Set next scene
        portal.SetNextSceneName(nextSceneName);

        spawnInfo.Init(0);
        waitForSpawn = true;
        // Spawn enemy
        //waitForSpawn = true;
        //spawnTimer = 0;

        foreach (Transform door in doors)
        {
            indicator = door.GetComponent<ElevatorIndicator>();
            if (indicator)
            {
                indicator.Init();
                indicator.SetText("Lobby");
            }
        }

        if (spawnInfo.isClearedAllStages)
            OnActivatePortal();

        if (spawnInfo.spawnRange != null)
        {
            distanceToSpawnRange = Vector3.Distance(spawnInfo.spawnRange.position, center.position);
        }

        audioManager = FindObjectOfType<AudioManager>();
    }


    private void FixedUpdate()
    {
        if (isLevelBegin == false)
        {
            // after entracne door close
            if (GameObject.Find("Door_Entrance").GetComponent<EntranceDoorContoller>().IsEntranceClosed())
            {
                isLevelBegin = true;
                TurnSpotLightOn();
            }
        }

        if (waitForSpawn == true && isLevelBegin == true)
        {
            if (isSpotLightOn == true)
            {
                GameObject player = GameObject.Find("Player");
                if (Vector3.Dot(player.transform.forward, Vector3.Normalize(center.position - player.transform.position)) > 0.8f
                    && Vector3.Distance(GameObject.Find("Player").transform.position, center.position) < 12.0f)
                {
                    PlayElevatorDDing();
                    TurnAllLightsOff();
                    SpawnEnemyOnce();

                    foreach (Transform door in doors)
                    {
                        int floor = spawnInfo.currentStage + 1;
                        door.GetComponent<ElevatorIndicator>().SetText(floor.ToString());
                    }
                }
            }
            else
            {
                eventTimer -= Time.deltaTime;
                if (eventTimer < 0.0f)
                {
                    TurnAllLightsOn();
                    waitForSpawn = false;
                }
            }
        }
    }

    private void SpawnEnemyOnce()
    {
        GameObject prefab = null;
        GameObject player = GameObject.Find("Player");
        float spawnY = spawnInfo.spawnRange.position.y;
        Vector3 centerToSpawn = Vector3.Normalize(center.position - player.transform.position);
        Vector3 newPosition = center.position + centerToSpawn * distanceToSpawnRange;
        newPosition.y = spawnY;
        spawnInfo.spawnRange.position = newPosition;
        spawnInfo.spawnRange.transform.Rotate(Vector3.up, player.transform.rotation.eulerAngles.y + 180.0f);

        //Debug.Log("spawnInfo position : " + spawnInfo.spawnRange.position);
        while (spawnInfo.GetSpwanEnemyPrefab(ref prefab))
        {
            GameObject enemy = Instantiate(prefab, spawnInfo.GetRandomPos(), Quaternion.identity);
            enemy.GetComponent<EnemyBase>().SetStopTimer(2.0f);
        }
    }

    public override void IncreaseKilledEnemyNum()
    {
        spawnInfo.IncreaseKilledEnemyNumInCurrentStage();

        // check if the player killed all enemies in the current stage
        if (portalActivated == false && spawnInfo.IsCurrentStageCleared())
        {
            spawnInfo.NextStage();

            if (spawnInfo.isClearedAllStages == true)
            {
                foreach (Transform door in doors)
                {
                    door.GetComponent<ElevatorIndicator>().SetText("Boss");
                }
                OnActivatePortal();
                PlayElevatorOpened();
            }
            else
            {
                waitForSpawn = true;
                TurnSpotLightOn();
            }
        }
    }

    private void TurnAllLightsOff()
    {
        //Debug.Log("light off called");
        eventTimer = lightsOffTimer;
        isLightsOn = false;
        foreach (Light l in lights)
        {
            l.color = Color.black;
        }
        //lights[0].color = Color.white; // main light
        isSpotLightOn = false;
    }

    private void TurnAllLightsOn()
    {
        //eventTimer = lightsOffTimer;
        isLightsOn = true;
        foreach (Light l in lights)
        {
            l.color = Color.white * 100 / 255;
        }
        lights[0].color = Color.black; // main light
    }

    private void TurnSpotLightOn()
    {
        foreach (Light l in lights)
        {
            l.color = Color.black;
        }
        lights[0].color = Color.white * 100 / 255; // main light
        isSpotLightOn = true;
    }

    private void PlayElevatorDDing()
    {
        audioManager.Play("ElevatorDing");
    }

    private void PlayElevatorOpened()
    {
        audioManager.Play("ElevatorOpened");
    }
}
