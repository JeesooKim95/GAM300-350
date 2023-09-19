/*
    Team    : Speaking Potato
    Author  : Sangmin Kim
    Date    : 10/5/2021
    Desc    : Script for spade 10 boss.
*/


using System.Collections.Generic;
using UnityEngine;

public class Spade10Enemy : SpadeEnemy
{
    public float attackTimer = 1f;
    private float ogAttackTimer;
    public GameObject rangedWeapon;
    private GameObject player;
    public GameObject meleeAttackObject;
    public GameObject knockBackVfx;
    public M5_ForestWave lastWave;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool isAttackAble = true;
    protected override void Start()
    {
        base.Start();

        if (audioManager == null)
            audioManager = FindObjectOfType<AudioManager>();

        meshRenderer.material.color = Color.white;
        player = GameObject.FindGameObjectWithTag("Player");

        ogAttackTimer = attackTimer;

        GameObject temp = GameObject.Find("LastSpawner");

        if (temp)
            lastWave = temp.GetComponent<M5_ForestWave>();

        //if (lastWave)
        //{
        //    List<GameObject> enemies = lastWave.Wave3;

        //    foreach (GameObject enemy in enemies)
        //    {
        //        if (enemy.GetComponent<Spade10Enemy>() == null)
        //        {
        //            //spawnedEnemies.Add(enemy);
        //        }
        //    }
        //}

        GetComponent<Status>().SetProtection(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (isAttackAble)
        {
            Spade10AttackPattern();
            isAttackAble = false;
        }
        else
        {
            if (attackTimer < 0f)
            {
                isAttackAble = true;
                attackTimer = ogAttackTimer;
            }
            else
                attackTimer -= Time.deltaTime;
        }

        if(GameObject.Find("CloverEnemy_Barrier(Clone)") == null)
            GetComponent<Status>().SetProtection(false);

        //if (SpawnedEnemiesValidCheck())
        //    GetComponent<Status>().SetProtection(false);
        //else
        //    GetComponent<Status>().SetProtection(true);
    }
    //private bool SpawnedEnemiesValidCheck()
    //{
    //    foreach(GameObject enemy in lastWave.Wave3)
    //        if(enemy != null && enemy.GetComponent<Spade10Enemy>() == null)
    //            return false;

    //    return true;
    //}
    private void Spade10AttackPattern()
    {
        float distanceBtwPlayer = Vector3.Distance(gameObject.transform.position, player.transform.position);

        if (distanceBtwPlayer > 20f)
            SpawnRangedAttack();
        else if (distanceBtwPlayer < 10f)
        {
            if (knockBackVfx)
            {
                audioManager.Play("CastingExplosion");
                GameObject vfx = GameObject.Instantiate(knockBackVfx, transform.position, Quaternion.identity);
            }
        }
        else
            Summon();
    }

    private void SpawnRangedAttack()
    {
        if (rangedWeapon != null)
        {
            RangedPattern();
        }
    }
    private void RangedPattern()
    {
        PlayCastingSound();

        List<Vector3> spawnPoses = new List<Vector3>();
        spawnPoses.Add(new Vector3(transform.position.x, transform.position.y + 8f, transform.position.z + 2f));
        spawnPoses.Add(new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z));
        spawnPoses.Add(new Vector3(transform.position.x, transform.position.y + 8f, transform.position.z - 2f));

        float randomTimer = Random.Range(1f, 2f);
        float timeStep = 0.5f;

        for (int i = 0; i < 3; ++i)
        {
            float newTimer = randomTimer + (timeStep * i);
            GameObject spawnedAttack = GameObject.Instantiate(rangedWeapon, spawnPoses[i], Quaternion.identity);
            spawnedAttack.GetComponent<Spade10EnemyTree>().waitTimer = newTimer;
        }

    }
    public override void Summon()
    {
        if (enemy1 == null)
        {
            Vector3 pos1 = transform.position + Quaternion.Euler(0.0f, 15f, 0f) * transform.forward * 10.0f;
            enemy1 = Instantiate(enemyToSummon, pos1, Quaternion.identity);
            enemy1.GetComponent<EnemyBase>().SetStopTimer(1.0f);
            enemy1.GetComponent<EnemyBase>().isSummoned = true;
            enemy1.GetComponent<EnemyBase>().SummonStart();
        }
        if (enemy2 == null)
        {
            Vector3 pos2 = transform.position + Quaternion.Euler(0.0f, -15f, 0f) * transform.forward * 10.0f;
            enemy2 = Instantiate(enemyToSummon, pos2, Quaternion.identity);
            enemy2.GetComponent<EnemyBase>().SetStopTimer(1.0f);
            enemy2.GetComponent<EnemyBase>().isSummoned = true;
            enemy2.GetComponent<EnemyBase>().SummonStart();
        }
        NotifyPlayer(10.0f);

        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
        audioManager.PlaySpatial("EnemySummon", gameObject.transform.position);
    }

    public void PlayCastingSound()
    {
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
        audioManager.Play("CastingBalls"/*, gameObject.transform.position*/);
    }
}
