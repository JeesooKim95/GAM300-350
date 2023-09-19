using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpadeQueenEscape : State
{
    // Start is called before the first frame update
    private GameObject player;
    private float speed = 5.0f;
    private float timer = 2.0f;

    public float enemyDistanceToRun = 10.0f;

    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        name = "Chasing";
        base.Initialize(enemyRef, anim);
        player = GameObject.Find("Player");
        anim.OnWalk(true);

        if (enemy.agent != null)
        {
            enemy.agent.enabled = true;
            enemy.agent.speed = 9.0f;
        }

        timer = 2.0f;
    }
    // Update is called once per frame
    public override void FixedUpdate()
    {
        //float distance = Vector3.Distance(enemy.transform.position, player.transform.position);

        //if (distance < enemyDistanceToRun)
        //{
        //    Vector3 dirToPlayer = enemy.transform.position - player.transform.position;
        //    Vector3 newPosition = enemy.transform.position + dirToPlayer;

        //    enemy.agent.SetDestination(newPosition);
        //    timer -= Time.deltaTime;
        //}

        enemy.agent.SetDestination(player.transform.position);
        timer -= Time.deltaTime;
    }

    public override void Exit()
    {

    }

    public bool IsDone()
    {
        return timer < 0.0f;
    }
}
