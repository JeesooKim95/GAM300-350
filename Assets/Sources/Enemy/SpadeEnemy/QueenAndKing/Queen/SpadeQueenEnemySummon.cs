using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpadeQueenEnemySummon : State
{
    [SerializeField] 
    protected float motionTimer = 1.0f;

    public GameObject enemyToSummon;

    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        base.Initialize(enemyRef, anim);
        anim.OnWalk(false);
        ((SpadeQueenAnim)anim).OnSummon();
        enemy.velocity = Vector3.zero;
        motionTimer = 1.0f;

        Vector3 direction = GameObject.Find("Player").transform.position - enemy.transform.position;
        direction.y = 0.0f;
        enemy.transform.rotation = Quaternion.LookRotation(direction);

        enemyRef.GetComponent<SpadeQueenEnemy>().Summon();

        if (enemy.agent != null)
            enemy.agent.speed = 0.0f;
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
