/*
    Team    : Speaking Potato
    Author  : Sean Kim
    Date    : 10/17/2021
    Desc    : class for enemy type - Diamond.
*/

using UnityEngine;

public class DiamondEnemy : EnemyBase
{
    DiamondEnemyPatrol patrolState = new DiamondEnemyPatrol();
    DiamondEnemyChase chaseState = new DiamondEnemyChase();
    DiamondEnemyAttack attackState = new DiamondEnemyAttack();

    [SerializeField] private GameObject projectile;

    public float playerDetectRange = 15.0f;
    public float attackRange = 14.0f;
    public float attackInterval = 1.5f;

    AudioManager audioManager;

    private float shotTime;

    protected override void Start()
    {
        base.Start();
        InitializeWithState(patrolState);

        audioManager = FindObjectOfType<AudioManager>();
        meshRenderer.material.color = Color.red;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Vector3 playerPos = GameObject.Find("Player").transform.position;
        // state chec k
        if (currentState == patrolState)
        {
            // later might change using collider
            if (Vector3.Distance(transform.position, playerPos) <= playerDetectRange)
            {
                Vector3 forward = Vector3.forward;
                forward = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * forward;
                if (Vector3.Dot(playerPos - transform.position, forward) > 0.2f)
                {
                    Aggro();
                    SetNextState(chaseState);
                }
            }
        }
        else if (currentState == chaseState)
        {
            float distance = Vector3.Distance(transform.position, GameObject.Find("Player").transform.position);
            if (distance <= attackRange)
            {
                SetNextState(attackState);
            }
        }
        else if (currentState == attackState)
        {
            transform.LookAt(new Vector3(playerPos.x, transform.position.y, playerPos.z));
            if (attackState.IsAttackDone() )
            {
                float distance = Vector3.Distance(transform.position, GameObject.Find("Player").transform.position);
                if (distance <= attackRange && (Time.time - shotTime > attackInterval))
                {
                    attackState.Initialize(gameObject, anim);
                }
                else if(distance > attackRange)
                {
                    SetNextState(chaseState);
                }
            }
        }
    }

    public override Rigidbody OnRangeAttack()
    {
        // Sinil - For sake of playing appropriate sound
        audioManager.PlaySpatial("EnemyRangeAttack", gameObject.transform.position);
        Vector3 initPos = new Vector3(transform.position.x, transform.position.y + 2.0f, transform.position.z);
        Rigidbody rb = Instantiate(projectile, initPos, Quaternion.identity).GetComponent<Rigidbody>();

        rb.gameObject.transform.rotation = this.transform.rotation;
        shotTime = Time.time;
        return rb;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


    // Sinil - For sake of playing appropriate sound
    private void Aggro()
    {
        audioManager.PlaySpatial("EnemyAggro", gameObject.transform.position);
        status.BeginCombat();
    }

    public override void OnNotifyPlayer()
    {
        if (currentState == patrolState)
        {
            //same as chasing
            Aggro();
            SetNextState(chaseState);
        }
    }
}

