/*
    Team    : Speaking Potato
    Author  : Sean Kim
    Date    : 10/17/2021
    Desc    : Diamond enemy attack state
*/
using UnityEngine;

public class DiamondEnemyAttack : State
{
    private float attackTimer = 1.5f;
    public Rigidbody arrowRB;

    public float forwardForce = 32.0f;
    public float upForce = 4.0f;

    public float sightRange;
    public int speed = 2;

    public float attackRange = 4.5f;

    public Transform player;
    bool hasAttacked;

    // Start is called before the first frame update
    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        base.Initialize(enemyRef, anim);
        enemy.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        if (anim != null)
        { 
            anim.OnWalk(false);
            anim.OnAttack();
        }
        

        if(enemy.agent != null)
            enemy.agent.speed = 0.0f;
    }

    // Update is called once per frame
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemy.transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemy.transform.position, sightRange);
    }
}
