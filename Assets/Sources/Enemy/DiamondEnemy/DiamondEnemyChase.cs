/*
    Team    : Speaking Potato
    Author  : Sean Kim
    Date    : 10/19/2021
    Desc    : Diamond enemy chase state.
*/
using UnityEngine;

public class DiamondEnemyChase : State
{
    private GameObject player;
    //private float speed = 2.0f;

    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        name = "Chasing";
        base.Initialize(enemyRef, anim);
        player = GameObject.Find("Player");
        anim.OnWalk(true);

        if (enemy.agent != null)
        { 
            enemy.agent.enabled = true;
            enemy.agent.speed = 6.0f;
        }
    }

    public override void FixedUpdate()
    {
        // if blocked with raycast ignore(TODO)
        enemy.agent.SetDestination(player.transform.position);
    }

    public override void Exit()
    {

    }
}
