/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 03/17/2022
    Desc    : class for elite-spade jack(in desert). 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpadeJackEnemy : EnemyBase
{
    public GameObject enemyToSummon;
    public GameObject summonEffect;

    ClubEnemyPatrol roamingState = new ClubEnemyPatrol();
    ClubEnemyChase chasingState = new ClubEnemyChase();
    ClubEnemyAttack meleeAttackState = new ClubEnemyAttack();
    SpadeJackEnemySummon summonState = new SpadeJackEnemySummon();
    SpadeJackEnemyRangeAttack rangeAttackState = new SpadeJackEnemyRangeAttack();

    public GameObject sandstorm;
    public GameObject meleeAttackObject;
    public float meleeAttackRange = 4.0f;
    public float rangeAttackRange = 25.0f;

    int totalAttackCount = 0;
    int meleeAttackCount = 0;

    private float meleeTryTimer = 5.0f;

    // Sinil - For sake of playing summon sound
    public AudioManager audioManager;

    protected override void Start()
    {
        base.Start();
        InitializeWithState(roamingState);
        audioManager = FindObjectOfType<AudioManager>();
    }

    protected override void FixedUpdate()
    {
        

        base.FixedUpdate();

        if (currentState == meleeAttackState)
        {
            if (meleeAttackState.IsAttackDone() == true)
            {
                SetNextState(chasingState);
                meleeAttackCount++;
                totalAttackCount++;
                meleeTryTimer = 5.0f;
            }
        }
        else if (currentState == rangeAttackState)
        {
            if (rangeAttackState.IsAttackDone() == true)
            {
                SetNextState(chasingState);
                meleeAttackCount = 0;
                totalAttackCount++;
                meleeTryTimer = 5.0f;
            }
        }
        else if (currentState == summonState)
        {
            if (summonState.IsDone() == true)
            {
                SetNextState(chasingState);
                totalAttackCount++;
            }
        }
        else if (currentState == chasingState)
        {
            if (chasingState.IsDone() == false) { }
            else 
            {
                if (totalAttackCount % 4 == 0)
                {
                    SetNextState(summonState);
                }
                else
                {
                    float distance = Vector3.Distance(this.transform.position, FindObjectOfType<PlayerMovement>().transform.position);
                    if (meleeAttackCount == 0) // melee only
                    {
                        meleeTryTimer -= Time.deltaTime;
                        if (distance < meleeAttackRange)
                        {
                            SetNextState(meleeAttackState);
                        }
                    }
                    else if (meleeAttackCount == 2) // use other - range 
                    {
                        if (distance < rangeAttackRange)
                        {
                            SetNextState(rangeAttackState);
                        }
                    }
                    else // based on distance? 
                    {
                        meleeTryTimer -= Time.deltaTime;
                        if (distance < meleeAttackRange)
                        {
                            SetNextState(meleeAttackState);
                        }
                        else if (distance > 10.0f && distance < rangeAttackRange)
                        {
                            SetNextState(rangeAttackState);
                        }
                    }

                    if (meleeTryTimer < 0.0f && currentState == chasingState)
                    {
                        SetNextState(rangeAttackState);
                    }
                }
            }
        }
        else if (currentState == roamingState)
        {
            SetNextState(chasingState);
        }

        // if still protected check if it must be unprotected
        if (GetComponent<Status>().IsProtected() == true)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null) return;
            }
            GetComponent<Status>().SetProtection(false);
        }
    }

    public override void Attack()
    {
        audioManager.PlaySpatial("EnemyAttack", gameObject.transform.position);

        Vector3 initPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        Instantiate(meleeAttackObject, initPos + (transform.forward * 1.5f), Quaternion.identity);
    }

    // 11/23/2021 Haewon, limit enemy number
    protected GameObject enemy1 = null;
    protected GameObject enemy2 = null;

    public virtual void Summon()
    {
        Debug.Log("summon called");
        if (enemy1 == null)
        {
            Vector3 pos1 = transform.position + Quaternion.Euler(0.0f, 15f, 0f) * transform.forward * 5.0f;
            enemy1 = Instantiate(enemyToSummon, pos1, Quaternion.identity);
            enemy1.GetComponent<EnemyBase>().SetStopTimer(1.0f);
            enemy1.GetComponent<EnemyBase>().isSummoned = true;
            enemy1.GetComponent<EnemyBase>().SummonStart();
        }
        if (enemy2 == null)
        {
            Vector3 pos1 = transform.position + Quaternion.Euler(0.0f, -15f, 0f) * transform.forward * 5.0f;
            enemy2 = Instantiate(enemyToSummon, pos1, Quaternion.identity);
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

    // Sinil - For sake of playing appropriate sound
    private void Aggro()
    {
        audioManager.PlaySpatial("EnemyAggro", gameObject.transform.position);
        status.BeginCombat();
    }

    public void RangeAttack()
    {
        audioManager.Play("CastingWinds");

        const float angleDiff = 30.0f;
        for (int i = -2; i <=2; ++i)
        {
            Vector3 forward = Quaternion.Euler(0.0f, angleDiff * i, 0f) * transform.forward;
            Vector3 initPos = transform.position + forward * 5.0f;
            initPos.y = -7.0f;

            GameObject attackObject = Instantiate(sandstorm, initPos, Quaternion.identity);
            attackObject.GetComponent<Sandstorm>().velocity = forward * 5.0f;
        }
    }

    // 04/07/2022 Haewon. Set protection for elite boss
    private List<GameObject> enemies;
    public void SetProtect(ref List<GameObject> list)
    {
        foreach (GameObject enemy in list)
        {
            enemies = new List<GameObject>();
            if (enemy.name != "Boss")
                enemies.Add(enemy);
        }

        GetComponent<Status>().SetProtection(true);
    }
}
