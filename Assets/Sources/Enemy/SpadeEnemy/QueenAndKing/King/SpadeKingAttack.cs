using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpadeKingAttack : State
{
    private float DelayTimer = 3.0f;

    private float attackTimer = 0.5f;

    private float localTimer;

    private bool attackAreaSpawned = false;

    private bool playerDamaged = false;

    private bool isStateDone = false;
    // Start is called before the first frame update
    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        name = "MeleeAttack";
        base.Initialize(enemyRef, anim);

        localTimer = 0f;
        attackAreaSpawned = false;
        playerDamaged = false;
        isStateDone = false;

    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        localTimer += Time.deltaTime;

        if (localTimer < attackTimer + DelayTimer)
        {
            Attack(localTimer);
        }
        else
        {
            isStateDone = true;
        }
    }

    void Attack(float dt)
    {
        if (attackAreaSpawned == false)
        {
            attackAreaSpawned = true;
            enemy.Attack();
        }

        if (playerDamaged == false)
        {
            playerDamaged = true;
        }
    }

    public bool IsAttackDone()
    {
        return isStateDone;
    }
    public override void Exit()
    {
    }

}