/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                10/17/2021
 *  Contributor:       Su Kim
 *  Description:      Set the Attack AI for Club Enemy
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClubEnemyAttack : State
{
    private float attackTimer = 1.5f;

    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        base.Initialize(enemyRef, anim);
        attackTimer = 1.0f;
        //enemy.OnAttack();
        enemy.velocity = new Vector3(0.0f, 0.0f, 0.0f);

        anim.OnWalk(false);
        anim.OnAttack();

        if (enemy.agent != null)
            enemy.agent.speed = 0.0f;
    }

    public override void FixedUpdate()
    {
        attackTimer -= Time.deltaTime;
    }

    public override void Exit()
    {
    }

    public bool IsAttackDone()
    {
        return attackTimer <= 0.0f;
    }
}