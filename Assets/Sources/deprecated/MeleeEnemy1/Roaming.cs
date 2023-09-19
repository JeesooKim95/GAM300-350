/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 10/05/2021
    Desc    : Base class for FSM enemy classes.
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee1Roaming : State
{
    private Vector3 roamingAreaCenter;
    private float roamingAreaRadius =  3f;
    private float speed = 2.0f; 
    private float rotationTimer = 0.2f;
    private float rotationAngle = 0.0f;


    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim) 
    {
        name = "Romaing";
        base.Initialize(enemyRef, anim);
        roamingAreaCenter = new Vector3(enemyRef.transform.position.x, 0.0f, enemyRef.transform.position.z);
        SetNewVelocity();
        enemy.SetStopTimer(0.5f);
        rotationTimer = 0.0f;
        enemy.transform.rotation = Quaternion.LookRotation(enemy.velocity, Vector3.up);
        anim.OnWalk(true);
    }

    public override void FixedUpdate()
    {
        rotationTimer -= Time.deltaTime;
        if (rotationTimer > 0.0f)
        {
            Rotate();
        }
        else if (rotationTimer <= -0.1f && Vector3.Distance(new Vector3(enemy.transform.position.x, 0.0f, enemy.transform.position.z), roamingAreaCenter) >= roamingAreaRadius)
        {
            float randomTime = Random.Range(0.5f, 0.7f);
            enemy.SetStopTimer(randomTime);
            rotationTimer = randomTime;
            SetNewVelocity();
            Rotate();
        }
    }

    public override void Exit() 
    {

    }

    private void SetNewVelocity()
    {
        float randomAngle = Random.Range(0.0f, 2 * Mathf.PI);
        Vector3 nextSpot = roamingAreaCenter + roamingAreaRadius * new Vector3(Mathf.Cos(randomAngle), 0.0f, Mathf.Sin(randomAngle));
        Vector3 newVelocity = nextSpot - enemy.transform.position;
        newVelocity.y = 0.0f;
        rotationAngle = Vector3.Angle(enemy.velocity.normalized, newVelocity.normalized) / rotationTimer;
        if (Vector3.Cross(enemy.velocity, newVelocity).y < 0.0f)
        {
            rotationAngle = -rotationAngle;
        }
        enemy.velocity = speed * newVelocity.normalized;
    }

    private void Rotate()
    {
        enemy.transform.Rotate(Vector3.up, rotationAngle * Time.deltaTime);
    }

    public override void OnDrawGizmos()
    {
        Gizmos.DrawLine(enemy.transform.position, enemy.transform.position + enemy.velocity);
    }
}
