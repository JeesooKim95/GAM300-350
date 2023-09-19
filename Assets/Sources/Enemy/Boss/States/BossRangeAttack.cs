/*
    Team    : Speaking Potato
    Author  : Sean Kim
    Date    : 10/23/2021
    Desc    : Range attack state for boss unit.
*/
using UnityEngine;

public class BossRangeAttack : State
{
    private Rigidbody rb;
    private GameObject player;
    private float attackTimer = 3.0f;
    bool hasAttacked;
    public float forwardForce = 10.0f;
    public float upForce = 0.0f;
    private Vector3 start;
    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        base.Initialize(enemyRef, anim);
        rb = enemy.OnRangeAttack();
        enemy.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        start = enemy.transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    public override void FixedUpdate()
    {
        attackTimer -= Time.deltaTime;

        if (rb != null)
        {
            //rb.transform.LookAt(player.transform.position);
            //rb.AddForce(enemy.transform.forward * forwardForce, ForceMode.Impulse);
            //rb.AddForce(enemy.transform.up * upForce, ForceMode.Impulse);
            //rb.gameObject.GetComponent<Rigidbody>().position = Vector3.Lerp(start, player.transform.position, 0.3f);
            hasAttacked = true;
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
