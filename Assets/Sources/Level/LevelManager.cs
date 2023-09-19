/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 09/19/2021
    Desc    : Script for level manager. Curruntly managing enemy spawn / clear check / portal creation. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Jina Hyun: 10/24: add classs and functions for stage & spawn enemy per stage
*/
[System.Serializable]
public class SpawnInfo
{
    public int currentStage = 0;
    public List<StageInfo> stages = new List<StageInfo>();
    public Transform spawnRange = null;

    

    private Vector3 min = new Vector3(-10, 0, -10);
    private Vector3 max = new Vector3(10, 0, 10);
    [HideInInspector]
    public bool isClearedAllStages = false;

    

    // Initialize enemy spawn range based on Game Object (spwanRange)
    public void Init(int enemy_num_already_in_the_scene = 0)
    {
        if(spawnRange != null)
        {
            Vector3 pos = spawnRange.position;
            Vector3 half_scale = spawnRange.localScale / 2.0f;
            min = new Vector3(pos.x - half_scale.x, pos.y, pos.z - half_scale.z);
            max = new Vector3(pos.x + half_scale.x, pos.y, pos.z + half_scale.z);
        }
        isClearedAllStages = false;

        if (stages.Count == 0)
        {
            currentStage = 0;
            stages.Add(new StageInfo());
            stages[0].InitWithEnemyNumber(enemy_num_already_in_the_scene);
           
            if(enemy_num_already_in_the_scene == 0)
                isClearedAllStages = true;
        }
        else
        {
            stages[currentStage].Init(enemy_num_already_in_the_scene);
        }

    }

    public void NextStage()
    {
        if (currentStage + 1 == stages.Count)
            isClearedAllStages = true;
        else
            stages[++currentStage].Init(0);
    }

    public bool GetSpwanEnemyPrefab(ref GameObject prefab)
    {
        return stages[currentStage].GetSpawnEnemyPrefab(ref prefab);
    }

    public Vector3 GetRandomPos()
    {
        Vector3 pos = new Vector3(Random.Range(min.x, max.x), min.y, Random.Range(min.y, max.y));
        Vector3 adjustment = pos - new Vector3((min.x + max.x) / 2, min.y, (min.y + max.y) / 2);
        adjustment = spawnRange.rotation * adjustment;
        return spawnRange.position + adjustment;
    }

    public bool IsCurrentStageCleared()
    {
        return stages[currentStage].IsCleared();
    }

    public void IncreaseKilledEnemyNumInCurrentStage()
    {
        stages[currentStage].IncreaseKilledEnemyNum();
    }
}


[System.Serializable]
public class StageInfo
{
    [System.Serializable]
    public class EnemyNumberPair
    {
        public GameObject prefab = null;
        public int count = 0;
    }

    // pair<Enemy prefab, How many enemies of this type are spawned>
    public List<EnemyNumberPair> enemyList = new List<EnemyNumberPair>();
    private int total = 0;
    private int killed = 0;
    private bool clear = false;
    private int current = 0; // used to spawn enemy

    public void Init(int enemy_num_already_in_the_scene)
    {
        total = enemy_num_already_in_the_scene;
        foreach (EnemyNumberPair enemy in enemyList) 
            total += enemy.count;
        killed = 0;
        clear = false;
    }

    public void InitWithEnemyNumber(int enemy_num_already_in_the_scene)
    {
        total = enemy_num_already_in_the_scene;
        //Debug.Log("stage initialized with total : " + total);
        killed = 0;
        clear = enemy_num_already_in_the_scene == 0 ? true : false;
    }
    
    public void IncreaseKilledEnemyNum()
    {
        killed += 1;
        if (total <= killed)
            clear = true;
    }

    public bool IsCleared()
    {
        return clear;
    }

    public bool GetSpawnEnemyPrefab(ref GameObject prefab)
    {
        int index = current++;
        foreach (EnemyNumberPair enemy in enemyList)
        {
            if (index < enemy.count)
            {
                prefab = enemy.prefab;
                return true;
            }
            else
                index -= enemy.count;
        }
        return false;
    }
}

// Haewon 11/16 temporarily changed protection level to protected
public class LevelManager : MonoBehaviour
{
    public int numberOfEnemyAlreadyInTheScene = 0;
    [Header("Enemy Spawn")]
    protected bool waitForSpawn = false;
    protected float spawnTimer = 0;
    public float spawnInterval = 3;
    public SpawnInfo spawnInfo = new SpawnInfo();
    
    [Header("Transform")]
    public Transform playerSpawnPosition = null;
    public List<Transform> doors = new List<Transform>();
    protected ElevatorIndicator indicator = null;
    [SerializeField]
    protected Portal portal;

    [Header("Next Scene")]
    [SerializeField]
    protected string nextSceneName;

    //12.1.2021 Su working here
    [SerializeField]
    protected string nextSceneWord;

    protected bool portalActivated = false;

    private void Start()
    {
        // Set player's initial position

        // commented out by Haewon, 02/03. 
        GameObject player = GameObject.Find("Player");
        //player.GetComponent<PlayerMovement>().InitPosition(playerSpawnPosition.position);

        // Set player's respawn position
        PlayerRespawn respawn = player.GetComponent<PlayerRespawn>();
        if (respawn)
            if(portal)
                respawn.SetSavePoint(portal.transform.position);

        // Set next scene
        if(portal)
            portal.SetNextSceneName(nextSceneName);

        // Spawn enemy
        spawnInfo.Init(numberOfEnemyAlreadyInTheScene);
        waitForSpawn = true;
        spawnTimer = 0;

        foreach(Transform door in doors)
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
    }


    private void FixedUpdate()
    {
        if (waitForSpawn == true)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnEnemyOnce();
                waitForSpawn = false;

                foreach (Transform door in doors)
                {
                    int floor = spawnInfo.currentStage + 1;
                    ElevatorIndicator indicator = door.GetComponent<ElevatorIndicator>();
                    if (indicator)
                    {
                        indicator.SetText(floor.ToString());
                    }
                }
            }
        }
    }

    protected void OnActivatePortal()
    {
        // Open the door if it exists
        if(doors.Count > 0)
        {
            DoorAnimController controller = doors[0].GetComponent<DoorAnimController>();
            if(controller != null)
            {
                controller.OnActiveDoor();
            }
        }

        // Activate the portal
        if (portal)
        { 
            portal.Activate();
            portalActivated = true;
        }
    }
    
    private void SpawnEnemyOnce()
    {
        GameObject prefab = null;
        while(spawnInfo.GetSpwanEnemyPrefab(ref prefab))
        { 
            Instantiate(prefab, spawnInfo.GetRandomPos(), Quaternion.AngleAxis(Random.Range(0, 3.14f), Vector3.up));
        }
    }

    public virtual void IncreaseKilledEnemyNum()
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
                    door.GetComponent<ElevatorIndicator>().SetText(nextSceneWord);
                }
                OnActivatePortal();
            }
            else
            {
                waitForSpawn = true;
                spawnTimer = 0;
            }
        }
    }
}
