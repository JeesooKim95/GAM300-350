/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 10/29/2021
    Desc    : class for enemy type - healing(heart).
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpadeEnemy : EnemyBase
{
    public GameObject enemyToSummon;
    public GameObject summonEffect;
    public float playerDetectRange = 10.0f;

    EnemyMelee1Roaming roamingState = new EnemyMelee1Roaming();
    SpadeEnemyWatching watchingState = new SpadeEnemyWatching();
    SpadeEnemySummon summonState = new SpadeEnemySummon();


    // Sinil - For sake of playing summon sound
    public AudioManager audioManager;

    protected override void Start()
    {
        base.Start();
        InitializeWithState(roamingState);
        audioManager = FindObjectOfType<AudioManager>();
        meshRenderer.material.color = new Color(51.0f / 255, 105.0f / 255, 198.0f / 255);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Vector3 playerPos = GameObject.Find("Player").transform.position;

        if (currentState == roamingState)
        {
            if (Vector3.Distance(transform.position, playerPos) <= playerDetectRange)
            {
                Vector3 forward = Vector3.forward;
                forward = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * forward;
                if (Vector3.Dot(playerPos - transform.position, forward) > 0.2f)
                {
                    Aggro();
                    transform.LookAt(new Vector3(playerPos.x, transform.position.y, playerPos.z));
                    SetNextState(summonState);
                }
            }
        }
        else if (currentState == watchingState)
        {
            if (watchingState.IsDone() == true)
            {
                SetNextState(summonState);
            }
        }
        else if (currentState == summonState)
        {
            if (summonState.IsDone())
            {
                SetNextState(watchingState);
            }
        }
    }

    // 11/23/2021 Haewon, limit enemy number
    protected GameObject enemy1 = null;
    protected GameObject enemy2 = null;

    public virtual void Summon()
    {
        Debug.Log("summon called");
        if (enemy1 == null)
        {
            Vector3 pos1 = transform.position + Quaternion.Euler(0.0f, 15f, 0f) * transform.forward * 3.0f;
            enemy1 = Instantiate(enemyToSummon, pos1, Quaternion.identity);
            enemy1.GetComponent<EnemyBase>().SetStopTimer(1.0f);
            enemy1.GetComponent<EnemyBase>().isSummoned = true;
            enemy1.GetComponent<EnemyBase>().SummonStart();
            
        }
        NotifyPlayer(10.0f);

        if(audioManager == null)
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

    public override void OnNotifyPlayer()
    {
        if (currentState == roamingState)
        { 
            Aggro();
            SetNextState(summonState);
        }
    }
}
