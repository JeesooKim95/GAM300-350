/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 10/05/2021
    Desc    : Base class for FSM enemy classes.
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee1Attack : State
{
    private float attackTimer = 1.0f;

    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        base.Initialize(enemyRef, anim);
        attackTimer = 1.0f;
        enemy.Attack();
        enemy.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        anim.OnWalk(false);
        anim.OnAttack(); 
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
