/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 10/10/2021
    Desc    : class for enemy type - melee with bash.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy2 : EnemyBase
{
    EnemyMelee1Roaming roamingState = new EnemyMelee1Roaming();
    EnemyMelee2Attack attackState = new EnemyMelee2Attack();

    // temp
    [SerializeField] private GameObject attackObject;
    public float playerDetectRange = 3.5f;

    protected override void Start()
    {
        base.Start();
        InitializeWithState(roamingState);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // state check
        if (currentState == roamingState)
        {
            // later might change using collider
            if (Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) <= playerDetectRange)
            {
                SetNextState(attackState);
                GetComponent<Renderer>().material.color = Color.red;
                NotifyPlayer(30.0f);
            }
        }
        else if (currentState == attackState)
        {
            if (attackState.IsAttackDone())
            {
                SetNextState(roamingState);
                GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }
}
