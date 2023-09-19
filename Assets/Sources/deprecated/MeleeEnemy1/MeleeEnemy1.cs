/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 10/05/2021
    Desc    : class for enemy type - melee.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy1 : EnemyBase
{
    EnemyMelee1Roaming roamingState = new EnemyMelee1Roaming();
    EnemyMelee1Chasing chasingState = new EnemyMelee1Chasing();
    EnemyMelee1Attack attackState = new EnemyMelee1Attack();

    // temp
    [SerializeField] private GameObject attackObject;

    public float playerDetectRange = 15.0f;
    public float playerMissRange = 30.0f;
    public float attackRange = 2.0f;


    protected override void Start() 
    {
        base.Start();
        InitializeWithState(roamingState);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Vector3 playerPos = GameObject.Find("Player").transform.position;
        // state check
        if (currentState == roamingState)
        {
            // later might change using collider
            if (Vector3.Distance(transform.position, playerPos) <= playerDetectRange)
            {
                Vector3 forward = Vector3.forward;
                forward = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * forward;
                if (Vector3.Dot(playerPos - transform.position, forward) > 0.2f)
                { 
                    SetNextState(chasingState);
                    GetComponent<Renderer>().material.color = Color.blue;
                    NotifyPlayer(30.0f);
                }
            }
        }
        else if (currentState == chasingState)
        {
            float distance = Vector3.Distance(transform.position, GameObject.Find("Player").transform.position);
            if (distance <= attackRange)
            {
                SetNextState(attackState);
                GetComponent<Renderer>().material.color = Color.red;
            }
            else if (distance > playerMissRange)
            {
                SetNextState(roamingState);
                GetComponent<Renderer>().material.color = Color.white;
            }
        }
        else if (currentState == attackState)
        { 
            if (attackState.IsAttackDone())
            {
                float distance = Vector3.Distance(transform.position, GameObject.Find("Player").transform.position);
                if (distance <= attackRange)
                {
                    attackState.Initialize(gameObject, anim);
                }
                else
                {
                    SetNextState(roamingState);
                    GetComponent<Renderer>().material.color = Color.white;
                } 
            }
        }
    }

    public override void Attack()
    {
        Instantiate(attackObject, transform.position + 0.5f * velocity, Quaternion.identity);
    }
}
