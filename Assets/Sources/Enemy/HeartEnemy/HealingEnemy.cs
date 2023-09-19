/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 10/18/2021
    Desc    : class for enemy type - healing(heart).
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingEnemy : EnemyBase
{
    EnemyMelee1Roaming roamingState = new EnemyMelee1Roaming();
    EnemyHealerWatching watchingState = new EnemyHealerWatching();
    EnemyHealerHealing healingState = new EnemyHealerHealing();

    private float playerDetectRange = 15.0f;

    protected override void Start()
    {
        base.Start();
        InitializeWithState(roamingState);
        meshRenderer.material.color = Color.green;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (currentState == roamingState)
        {
            Vector3 playerPos = GameObject.Find("Player").transform.position;
            if (Vector3.Distance(transform.position, playerPos) <= playerDetectRange)
            {
                SetNextState(watchingState);
            }
        }
        if (currentState == watchingState)
        {
            if (watchingState.shouldHeal == true)
            {
                // Sinil - For sake of playing appropriate sound
                FindObjectOfType<AudioManager>().PlaySpatial("EnemyHealing", gameObject.transform.position);
                SetNextState(healingState);
            }
        }
        else if (currentState == healingState)
        {
            if (healingState.IsDone())
            {
                SetNextState(watchingState);
                GetComponentInChildren<HealEffectManager>().Stop();
            }
        }
    }

    public override void OnNotifyPlayer()
    {
        if (currentState == roamingState)
        {
            SetNextState(watchingState);
            status.BeginCombat();
        }
    }
}
