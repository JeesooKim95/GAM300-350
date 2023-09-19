/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                10/17/2021
 *  Contributor:       Su Kim
 *  Description:      Set the Chasing AI for Club enemy
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClubEnemyChase : State
{
    private GameObject player;
    private float speed = 5.0f;
    private float timer = 2.0f;

    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        name = "Chasing";
        base.Initialize(enemyRef, anim);
        player = GameObject.Find("Player");
        anim.OnWalk(true);
        //Debug.Log("onwalk true");
        if (enemy.agent != null)
        {
            enemy.agent.enabled = true;
            enemy.agent.speed = 9.0f;
        }
        timer = 2.0f;
    }

    public override void FixedUpdate()
    {
        //Vector3 direction = player.transform.position - enemy.transform.position;
        //direction.y = 0.0f;
        //enemy.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        //enemy.velocity = speed * direction.normalized;

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
