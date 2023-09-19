/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 10/18/2021
    Desc    : class for healing enemy's healing state
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealerWatching : State
{
    private Transform playerTransform = null;
    private float healRange = 15.0f;
    private float healingCooldown = 3.0f;
    public bool shouldHeal = false;
    EnemyAnimBase animBase;
    private const float speed = 6.0f;

    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        base.Initialize(enemyRef, anim);
        animBase = anim;
        anim.OnWalk(true);
        shouldHeal = false;
        healingCooldown = 3.0f;

        // only 1st time
        if (playerTransform == null)
        {
            playerTransform = GameObject.Find("Player").transform;
        }

        if (enemy.agent != null)
        {
            enemy.agent.enabled = true;
            enemy.agent.speed = speed;
        }
    }

    public override void FixedUpdate()
    {
        healingCooldown -= Time.deltaTime;
        enemy.agent.SetDestination(playerTransform.position);
        bool shouldHealNow = IsNearbyEnemyHurt();
        if (shouldHeal != shouldHealNow)
        {
            if (shouldHealNow == true)
            {
                animBase.OnWalk(false);
                enemy.agent.speed = 0.0f;
            }
            else
            {
                animBase.OnWalk(true);
                enemy.agent.speed = speed;
            }
            shouldHeal = shouldHealNow;
        }
        enemy.transform.LookAt(new Vector3(playerTransform.position.x, enemy.transform.position.y, playerTransform.position.z));
    }

    public override void Exit()
    {

    }

    public bool IsDone()
    {
        return healingCooldown <= 0.0f;
    }

    public override void OnDrawGizmos()
    {
    }

    public bool IsNearbyEnemyHurt()
    {
        EnemyBase[] allEnemies = GameObject.FindObjectsOfType<EnemyBase>();
        foreach (EnemyBase currentEnemy in allEnemies)
        {
            float distanceToEnemy = Vector3.Distance(currentEnemy.transform.position, enemy.transform.position);
            if (distanceToEnemy < healRange)
            {
                if (currentEnemy == enemy) continue; // skip self
                if(currentEnemy.GetComponent<Status>().currentHealth < currentEnemy.GetComponent<Status>().maxHealth)
                return true;
            }
        }
        return false;
    }
}
