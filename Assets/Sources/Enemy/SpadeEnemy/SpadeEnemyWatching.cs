/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 10/29/2021
    Desc    : class for healing enemy's healing state
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpadeEnemyWatching : State
{
    private Transform playerTransform = null;
    private float motionTimer = 6.0f;

    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        base.Initialize(enemyRef, anim);
        anim.OnWalk(true);

        // only 1st time
        if (playerTransform == null)
        {
            playerTransform = GameObject.Find("Player").transform;
        }
        enemy.velocity = Vector3.zero;
        motionTimer = 6.0f;

        if (enemy.agent != null)
        {
            enemy.agent.enabled = true;
            enemy.agent.speed = 4.0f;
        }
    }

    public override void FixedUpdate()
    {
        motionTimer -= Time.deltaTime;
        if(enemy.agent != null)
            enemy.agent.SetDestination(playerTransform.position);
    }

    public override void Exit()
    {

    }

    public bool IsDone()
    {
        return motionTimer <= 0.0f;
    }
}
