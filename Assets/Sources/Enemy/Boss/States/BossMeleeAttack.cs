/*
    Team    : Speaking Potato
    Author  : Sinil Kang
    Date    : 10/17/2021
    Desc    : Boss's base melee Attack code.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeAttack : State
{
    private float preDelayTimer = 0.5f;
    private float attackTimer = 0.3f;
    private float postDelayTimer = 0.5f;

    private float localTimer;

    private bool hasAttackAreaSpawned = false;
    private bool hasPlayerDamaged = false;

    private bool isStateDone = false;
    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        name = "MeleeAttack";
        base.Initialize(enemyRef, anim);
        
        localTimer = 0f;
        hasAttackAreaSpawned = false;
        hasPlayerDamaged = false;
        isStateDone = false;

    }

    public override void FixedUpdate()
    {
        localTimer += Time.deltaTime;

        // Pre delay
        if (localTimer < preDelayTimer)
        {
            PlayPreDelayAnimation(localTimer);
        }
        // Attack
        else if (localTimer < preDelayTimer + attackTimer)
        {
            Attack(localTimer);
        }
        // Post delay
        else if (localTimer < preDelayTimer + attackTimer + postDelayTimer)
        {
            PlayPostDelayAnimation(localTimer);
        }
        else
        {
            isStateDone = true;
        }
    }
    public override void Exit()
    {
    }

    private void PlayPreDelayAnimation(float dt)
    {

    }


    private void Attack(float dt)
    {
        // Spawn red transparent cube to make visual feedback
        if(!hasAttackAreaSpawned)
        {
            // Spawn Attack Area
            hasAttackAreaSpawned = true;
            enemy.Attack();
        }
        // deal damage.
        if(!hasPlayerDamaged)
        {
            hasPlayerDamaged = true;
        }
        //
    }

    private void PlayPostDelayAnimation(float dt)
    {
    }

    public bool IsAttackDone()
    {
        return isStateDone;
    }
}
