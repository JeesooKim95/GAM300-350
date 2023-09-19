/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 10/05/2021
    Desc    : Base class for FSM enemy classes.
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee1Chasing : State
{
    private GameObject player;
    private float speed = 3.0f;

    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        name = "Chasing";
        base.Initialize(enemyRef, anim);
        player = GameObject.Find("Player");
        anim.OnWalk(true);
    }

    public override void FixedUpdate() 
    {
        // if blocked with raycast ignore(TODO)
        Vector3 direction = player.transform.position - enemy.transform.position;
        direction.y = 0.0f;
        float maxRotationAngle = 120.0f * Time.deltaTime;
        if (Vector3.Dot(enemy.velocity, direction.normalized) > maxRotationAngle)
        {
            //enemy.velocity = speed * direction.normalized;
            if (Vector3.Cross(enemy.velocity, direction.normalized).y < 0.0f)
            {
                enemy.transform.Rotate(Vector3.up, -maxRotationAngle);
                enemy.velocity = Quaternion.Euler(0, -maxRotationAngle, 0) * enemy.velocity;
            }
            else
            {
                enemy.transform.Rotate(Vector3.up, maxRotationAngle);
                enemy.velocity = Quaternion.Euler(0, maxRotationAngle, 0) * enemy.velocity;
            }
        }
        else
        {
            enemy.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            enemy.velocity = speed * direction.normalized;
        }
        
    }

    public override void Exit() 
    {

    }
}
