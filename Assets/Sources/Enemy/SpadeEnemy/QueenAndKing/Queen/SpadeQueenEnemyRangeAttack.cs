using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpadeQueenEnemyRangeAttack : State
{
    private float motionTimer;

    private Transform playerTransform;
    // Start is called before the first frame update

    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        base.Initialize(enemyRef, anim);
        enemy.velocity = new Vector3(0, 0, 0);

        if (anim != null)
        {
            anim.OnWalk(false);
            ((SpadeQueenAnim)anim).OnRangeAttack();
        }

        if (enemy.agent != null)
            enemy.agent.speed = 0.0f;

        motionTimer = 2.0f;
        playerTransform = GameObject.FindObjectOfType<PlayerMovement>().transform;
    }


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
