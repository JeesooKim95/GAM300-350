using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
//using UnityEditor.SceneTemplate;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SpadeKingChase : State
{
    private GameObject player;
    private float speed = 8.0f;
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