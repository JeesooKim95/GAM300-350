/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 10/05/2021
    Desc    : Base class for FSM enemy classes.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee2Attack : State
{
    private float rotationTimer = 1.0f;
    private float attackTimer = 1.5f;
    private float speed = 12f;
    private GameObject player;
    private Vector3 direction;

    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        base.Initialize(enemyRef, anim);
        rotationTimer = 1.0f;
        attackTimer = 2f;
        player = GameObject.Find("Player");
        enemy.velocity = Vector3.zero;
        anim.OnWalk(false);
        anim.OnAttack();
    }

    public override void FixedUpdate()
    {
        if (rotationTimer > 0.0f)
        {
            rotationTimer -= Time.deltaTime;
            direction = player.transform.position - enemy.transform.position;
            direction.y = 0.0f;
            enemy.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            attackTimer -= Time.deltaTime;
            enemy.velocity = direction.normalized * speed;
        }
    }

    public override void Exit()
    {

    }

    public bool IsAttackDone()
    {
        return attackTimer <= 0.0f;
    }
}
