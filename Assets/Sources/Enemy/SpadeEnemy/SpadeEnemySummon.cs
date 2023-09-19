/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 10/29/2021
    Desc    : class for healing enemy's healing state
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpadeEnemySummon : State
{
    [SerializeField]
    protected float motionTimer = 1.0f;
    public GameObject enemyToSummon;

    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        base.Initialize(enemyRef, anim);
        anim.OnWalk(false);
        anim.OnAttack();
        enemy.velocity = Vector3.zero;
        motionTimer = 1.0f;

        Vector3 direction = GameObject.Find("Player").transform.position - enemy.transform.position;
        direction.y = 0.0f;
        enemy.transform.rotation = Quaternion.LookRotation(direction);

        if (enemy.agent != null)
            enemy.agent.speed = 0.0f;

        enemyRef.GetComponent<SpadeEnemy>().Summon();
    }

    public override void FixedUpdate()
    {
        motionTimer -= Time.deltaTime;
    }

    public override void Exit()
    {

    }

    public bool IsDone()
    {
        return motionTimer <= 0.0f;
    }
}
