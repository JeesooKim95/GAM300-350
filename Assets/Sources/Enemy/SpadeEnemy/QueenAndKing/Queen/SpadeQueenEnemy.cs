/*
    Team    : Speaking Potato
    Author  : Su Kim
    Date    : 03/31/2022
    Desc    : class for elite-spade queen(in snow)
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpadeQueenEnemy : EnemyBase
{
    public GameObject spadeKing;
    public GameObject enemyToSummon;
    public GameObject summonEffect;

    private ClubEnemyPatrol roamingState = new ClubEnemyPatrol();
    private SpadeQueenEscape runningState = new SpadeQueenEscape();
    private SpadeQueenEnemySummon summonState = new SpadeQueenEnemySummon();
    private ClubEnemyAttack meleeAttackState = new ClubEnemyAttack();
    private SpadeQueenEnemyRangeAttack rangeAttackState = new SpadeQueenEnemyRangeAttack();

    public GameObject blizzard;
    public GameObject meleeAttackObject;
    public float meleeAttackRange = 4.0f;
    public float rangeAttackRange = 25.0f;


    private int totalAttackCount = 0;
    private int meleeAttacCount = 0;

    private float meleeTryTimer = 2.0f;

    public AudioManager audioManager;


    protected GameObject enemy1 = null;
    protected GameObject enemy2 = null;
    protected GameObject enemy3 = null;
    protected GameObject enemy4 = null;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        InitializeWithState(roamingState);
        audioManager = FindObjectOfType<AudioManager>();
        status = GetComponent<Status>();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (currentState == meleeAttackState)
        {
            if (meleeAttackState.IsAttackDone() == true)
            {
                SetNextState(runningState);
                meleeAttacCount++;
                totalAttackCount++;
                meleeTryTimer = 2.0f;
            }
        }
        else if (currentState == rangeAttackState)
        {
            if (rangeAttackState.IsAttackDone() == true)
            {
                SetNextState(runningState);
                meleeAttacCount = 0;
                totalAttackCount++;
                meleeTryTimer = 2.0f;
            }
        }
        else if (currentState == summonState)
        {
            if (summonState.IsDone() == true)
            {
                SetNextState(runningState);
                totalAttackCount++;
            }
        }
        else if (currentState == runningState)
        {
            if (runningState.IsDone() == false)
                return;

            if (totalAttackCount % 4 == 0)
            {
                SetNextState(summonState);
                return;
            }

            float distance = Vector3.Distance(this.transform.position,
                FindObjectOfType<PlayerMovement>().transform.position);

            if (meleeAttacCount == 0)
            {
                meleeTryTimer -= Time.deltaTime;
                if (distance < meleeAttackRange)
                {
                    SetNextState(meleeAttackState);
                }
            }
            else if (meleeAttacCount == 2)
            {
                if (distance < rangeAttackRange)
                {
                    audioManager.Play("QueenBlizzard");
                    SetNextState(rangeAttackState);
                }
            }
            else
            {
                meleeTryTimer -= Time.deltaTime;
                if (distance < meleeAttackRange)
                {
                    SetNextState(meleeAttackState);
                }
                else if (distance > 10.0f && distance < rangeAttackRange)
                {
                    audioManager.Play("QueenBlizzard");
                    SetNextState(rangeAttackState);
                }
            }

            if (meleeTryTimer < 0.0f & currentState == runningState)
            {
                audioManager.Play("QueenBlizzard");
                SetNextState(rangeAttackState);
            }


        }
        else if (currentState == roamingState)
        {
            SetNextState(runningState);
        }
    }

    public override void Attack()
    {
        audioManager.PlaySpatial("EnemyAttack", gameObject.transform.position);

        Vector3 initPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        Instantiate(meleeAttackObject, initPos + (transform.forward * 1.5f), Quaternion.identity);
    }



    public virtual void Summon()
    {
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
        if (enemy3 == null)
        {
            Vector3 pos1 = transform.position + Quaternion.Euler(0.0f, 30, 0f) * transform.forward * 5.0f;
            enemy1 = Instantiate(enemyToSummon, pos1, Quaternion.identity);
            enemy1.GetComponent<EnemyBase>().SetStopTimer(1.0f);
            enemy1.GetComponent<EnemyBase>().isSummoned = true;
            enemy1.GetComponent<EnemyBase>().SummonStart();
        }
        if (enemy3 == null)
        {
            Vector3 pos1 = transform.position + Quaternion.Euler(0.0f, -30f, 0f) * transform.forward * 5.0f;
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

    private void Aggro()
    {
        audioManager.PlaySpatial("EnemyAggro", gameObject.transform.position);
        status.BeginCombat();
    }

    public void RangeAttack()
    {

        Vector3 playerPos = FindObjectOfType<PlayerMovement>().transform.position;


        GameObject attackObject = Instantiate(blizzard, playerPos, Quaternion.identity);

    }
}
