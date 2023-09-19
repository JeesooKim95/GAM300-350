/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 02/03/2022
    Desc    : For m5_desert map spawn and clear
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M5_DesertSceneManager : MonoBehaviour
{
    [System.Serializable]
    public class StageObstaclePair
    {
        public List<StageInfo> stage;
        public List<Transform> spawnRange;
        public GameObject obstacle;
        public GameObject spawnTrigger;
    }

    private List<GameObject> liveEnemies = new List<GameObject>();
    private Transform currentSpawnRange;
    public List<StageObstaclePair> stages;

    private bool isDummySummoned = false; // to check if boss dummy summoned. 

    private Vector3 min = new Vector3(-10, 0, -10);
    private Vector3 max = new Vector3(10, 0, 10);

    public Portal portal;
    //Sangmin
    public GameObject bossDieDummyObj;
    private GameObject boss;
    private Status bossStatus;

    int currentStage = 0;
    private bool waitForNextStage = false;

    public GameObject spawnParticle;

void Start()
    {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            liveEnemies.Add(enemy);
        }

        if (liveEnemies.Count == 0)
        {
            StartCoroutine(ExecuteAfterTime(0.5f));
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        waitForNextStage = true;
        RunNextStage();
    }

    void Update()
    {
        List<GameObject> enemyToDelete = new List<GameObject>();
        foreach (GameObject enemy in liveEnemies)
        {
            if (enemy == null)
            {
                enemyToDelete.Add(enemy);
            }
        }
        foreach (GameObject enemy in enemyToDelete)
        {
            liveEnemies.Remove(enemy);
        }

        if (liveEnemies.Count == 0)
        {
            if (currentStage < stages.Count)
            {
                if (stages[currentStage].obstacle)
                    stages[currentStage].obstacle.SetActive(false);
                waitForNextStage = true;
            }
            else
            {
                portal.Activate();
            }
        }

        if(boss != null && isDummySummoned == false)
        {
            if (boss.GetComponent<Status>().GetHealth() <= 0)
            {
                Vector3 bossDiePos = boss.transform.position;
                bossDiePos.y = -7.6f;

                isDummySummoned = true;
                GameObject bossDummyObj = GameObject.Instantiate(bossDieDummyObj, bossDiePos, boss.transform.rotation);
                Destroy(bossDummyObj, 3f);
            }            
        }
    }

    public void RunNextStage()
    {
        if (waitForNextStage == false)
        {
            return; 
        }

        FindObjectOfType<AudioManager>().Play("ButtonClick");

        for (int i = 0; i < stages[currentStage].stage.Count; ++i)
        {
            SetSpawnRange(stages[currentStage].spawnRange[i]);
            foreach (StageInfo.EnemyNumberPair pair in stages[currentStage].stage[i].enemyList)
            {
                for (int j = 0; j < pair.count; ++j)
                {
                    Vector3 randomPos = GetRandomPos();
                    GameObject enemy = Instantiate(pair.prefab, randomPos, Quaternion.identity);
                    if (enemy.name == "SpadeJack(Clone)")
                    {
                        enemy.name = "Boss";
                        enemy.GetComponent<SpadeJackEnemy>().SetProtect(ref liveEnemies);

                        //Sangmin
                        boss = enemy;
                        bossStatus = boss.GetComponent<Status>();
                    }
                    liveEnemies.Add(enemy);
                    if(spawnParticle)
                    {
                        GameObject particle = GameObject.Instantiate(spawnParticle, new Vector3(randomPos.x, -7.5f, randomPos.z), Quaternion.identity);
                        Destroy(particle, 1f);
                    }
                }
            }
        }

        if (stages[currentStage].obstacle)
            stages[currentStage].obstacle.SetActive(true);

        currentStage++;
    }

    public Vector3 GetRandomPos()
    {
        Vector3 pos = new Vector3(Random.Range(min.x, max.x), min.y, Random.Range(min.y, max.y));
        Vector3 adjustment = pos - new Vector3((min.x + max.x) / 2, min.y, (min.y + max.y) / 2);
        adjustment = currentSpawnRange.rotation * adjustment;
        return currentSpawnRange.position + adjustment;
    }

    private void SetSpawnRange(Transform newSpawnRange)
    {
        Vector3 pos = newSpawnRange.position;
        Vector3 half_scale = newSpawnRange.localScale / 2.0f;
        min = new Vector3(pos.x - half_scale.x, pos.y, pos.z - half_scale.z);
        max = new Vector3(pos.x + half_scale.x, pos.y, pos.z + half_scale.z);
        currentSpawnRange = newSpawnRange;
    }
}
