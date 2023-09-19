/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 10/18/2021
    Desc    : class for healing enemy's healing state
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealerHealing : State
{
    [SerializeField]
    private float motionTimer = 3.5f;
    private float healTimer = 0.5f;
    private float startTimer = 0.5f; 

    private float healRange = 8.0f;

    HeartAnim heartAnim = null;
    private bool isHealing = false;

    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        base.Initialize(enemyRef, anim);

        heartAnim =(HeartAnim)anim;
        anim.OnWalk(false);
        anim.OnAttack();

        motionTimer = 3f;
        healTimer = 0.5f;
        startTimer = 0.5f;
        isHealing = false;

        if (enemy.agent != null)
            enemy.agent.speed = 0.0f;
    }

    public void StartHeal()
    {
        Debug.Log("StartHeal called");
        HealEffectManager effectManager = enemy.GetComponentInChildren<HealEffectManager>();
        if (effectManager)
            effectManager.Play();

        isHealing = true;
    }

    public override void FixedUpdate()
    {
        if (startTimer > 0.0f)
        {
            startTimer -= Time.deltaTime;
            if (startTimer <= 0.0f)
            {
                StartHeal();
            }
            return;
        }
        motionTimer -= Time.deltaTime;
        if (isHealing)
        { 
            healTimer -= Time.deltaTime;
            if (healTimer <= 0.0f)
            {
                healTimer = 0.5f;
                Heal();
            }
        }
        if (motionTimer <= 0.5f && isHealing == true)
        {
            heartAnim.EndAttack();
            isHealing = false;
        }
    }

    public override void Exit()
    {
    }

    public bool IsDone()
    {
        return motionTimer <= 0.0f;
    }

    public void Heal()
    {
        EnemyBase[] allEnemies = GameObject.FindObjectsOfType<EnemyBase>();
        foreach (EnemyBase currentEnemy in allEnemies)
        {
            float distanceToEnemy = Vector3.Distance(currentEnemy.transform.position, enemy.transform.position);
            if (distanceToEnemy < healRange)
            {
                currentEnemy.OnHeal(2);
            }
        }
    }
}
