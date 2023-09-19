/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                10/17/2021
 *  Contributor:       Su Kim
 *  Description:      Set the mechanism of Club Enemy
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClubEnemy : EnemyBase
{
    // Start is called before the first frame update
    ClubEnemyPatrol patrolState = new ClubEnemyPatrol();
    ClubEnemyChase chaseState = new ClubEnemyChase();
    ClubEnemyAttack attackState = new ClubEnemyAttack();

    [SerializeField] private GameObject attackObject;

    public float playerDetectRange = 15.0f;
    public float attackRange = 4.0f;
    private Color orangeColor = new Color(1.0f, 0.65f, 0.0f);

    private AudioManager audioManager;

    protected override void Start()
    {
        base.Start();
        InitializeWithState(patrolState);
        
        audioManager = FindObjectOfType<AudioManager>();

        if (isSummoned == true)
        {
            SetNextState(chaseState);
        }
        anim.OnWalk(true);
        meshRenderer.material.color = orangeColor;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (stopTimer > 0.0f) return;

        Vector3 playerPos = GameObject.Find("Player").transform.position;

        if (currentState == patrolState)
        {
            if (Vector3.Distance(transform.position, playerPos) <= playerDetectRange)
            {
                Vector3 forward = Vector3.forward;
                forward = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * forward;
                if (Vector3.Dot(playerPos - transform.position, forward) > 0f)
                {
                    Aggro();
                    SetNextState(chaseState);
                    //meshRenderer.material.color = orangeColor;
                    NotifyPlayer(30.0f);
                }
            }
        }
        else if (currentState == chaseState)
        {
            float distance = Vector3.Distance(transform.position, GameObject.Find("Player").transform.position);
            if (distance <= attackRange)
            {
                SetNextState(attackState);
                //meshRenderer.material.color = Color.red;
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
                    SetNextState(chaseState);
                    //meshRenderer.material.color = orangeColor;
                }
            }
        }

    }

    // Sinil - For sake of playing appropriate sound
    public override void Attack()
    {
        audioManager.PlaySpatial("EnemyAttack", gameObject.transform.position);

        Vector3 initPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        Instantiate(attackObject, initPos + transform.forward, Quaternion.identity);
    }

    // Sinil - For sake of playing appropriate sound
    private void Aggro()
    {
        audioManager.PlaySpatial("EnemyAggro", gameObject.transform.position);
        status.BeginCombat();
    }

    public override void OnNotifyPlayer()
    {
        if(currentState == patrolState)
        {
            //same as chasing
            SetNextState(chaseState);
            //meshRenderer.material.color = orangeColor;
            NotifyPlayer(30.0f);
            Aggro();
        }
    }

    public override void SummonStart()
    {
        //base.Start();
        //InitializeWithState(patrolState);

        //audioManager = FindObjectOfType<AudioManager>();
    }
}
