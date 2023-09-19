/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 03/17/2022
    Desc    : class for elite-spade jack's range(creating sandstorm pattern). 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpadeJackEnemyRangeAttack : State
{
    private float motionTimer;
    private Transform playerTransform;

    // Start is called before the first frame update
    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        base.Initialize(enemyRef, anim);
        enemy.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        if (anim != null)
        {
            anim.OnWalk(false);
            ((SpadeJackAnim)anim).OnRangeAttack();
        }

        if (enemy.agent != null)
            enemy.agent.speed = 0.0f;

        motionTimer = 3.0f;
        playerTransform = GameObject.FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        motionTimer -= Time.deltaTime;
        Vector3 forward = (playerTransform.position - enemy.transform.position);
        forward.y = 0.0f;
        enemy.transform.forward = forward;
    }

    public override void Exit()
    {

    }


    public bool IsAttackDone()
    {
        return motionTimer <= 0.0f;
    }
}
