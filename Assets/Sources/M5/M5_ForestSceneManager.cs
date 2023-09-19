/*
    Sangmin Kim
    22/02/01
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M5_ForestSceneManager : MonoBehaviour
{
    public GameObject spawnWave1;
    public GameObject spawnWave2;
    public GameObject spawnWave3;

    public GameObject obstacle;
    private MeshRenderer firstMesh;
    private MeshRenderer secondMesh;
    private MeshRenderer thirdMesh;
    private Spade10Enemy boss = null;
    private Status bossStatus = null;
    private Vector3 bossDiedPos;

    private Portal portal = null;
    private Coroutine alreadyCelebrating = null;
    public GameObject celebrateEffect;
    public GameObject bossDummy;
    Quaternion bossRot;
    private float celebrateTimer = 1f;
    void Start()
    {
        firstMesh = spawnWave1.GetComponent<MeshRenderer>();
        secondMesh = spawnWave2.GetComponent<MeshRenderer>();
        thirdMesh = spawnWave3.GetComponent<MeshRenderer>();
        portal = FindObjectOfType<Portal>();
    }

    void Update()
    {
        if (firstMesh.enabled == false && secondMesh.enabled == false)
        {
            GameObject.Destroy(obstacle);
            //enabled = false;
        }
        if (boss == null)
        {
            Spade10Enemy spade10 = GameObject.FindObjectOfType<Spade10Enemy>();
            if (spade10 != null)
            {
                //boss is exist
                boss = spade10;
                bossStatus = boss.gameObject.GetComponent<Status>();
                bossRot = boss.transform.rotation;
            }
        }
        else
        {
            if (bossStatus.currentHealth < 0f)
            {
                if (alreadyCelebrating == null)
                {
                    bossDiedPos = boss.gameObject.transform.position;
                    bossDiedPos.y = -1f;
                    alreadyCelebrating = StartCoroutine(CelebratingScene());

                    if(bossDummy)
                    {
                        GameObject bossDummyObj = GameObject.Instantiate(bossDummy, bossDiedPos, bossRot);
                        Destroy(bossDummyObj, 3f);
                    }
                }
            }
        }

        if (alreadyCelebrating != null)
        {
            if (celebrateTimer > 0f)
            {
                celebrateTimer -= Time.deltaTime;
            }
            else
            {
                GameObject cele = GameObject.Instantiate(celebrateEffect, bossDiedPos, Quaternion.identity);
                cele.transform.localScale = new Vector3(5f, 5f, 5f);
                Destroy(cele, 1f);
                celebrateTimer = 1f;
            }
        }
    }
    IEnumerator CelebratingScene()
    {
        yield return new WaitForSeconds(3f);
        ClearScene();
        alreadyCelebrating = null;
    }
    private void ClearScene()
    {
        portal.Activate();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].GetComponent<EnemyBase>().OnDeath();
            }
        }
    }
}
