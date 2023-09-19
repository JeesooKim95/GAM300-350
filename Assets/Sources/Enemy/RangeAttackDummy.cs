using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackDummy : EnemyBase
{
    DiamondEnemyAttack attackState = new DiamondEnemyAttack();
    //DiamondEnemyPatrol patrolState = new DiamondEnemyPatrol();
    //DiamondEnemyChase chaseState = new DiamondEnemyChase();


    [SerializeField] private GameObject projectile;

    public float attackInterval = 1.5f;
    private float shotTime;
    private AudioManager audioManager;
    public float playerDetectRange = 15.0f;
    public float attackRange = 14.0f;
    protected override void Start()
    {
        base.Start();
        InitializeWithState(attackState);

        audioManager = FindObjectOfType<AudioManager>();
    }

    protected override void FixedUpdate()
    {
        
        base.FixedUpdate();
        if (currentState == attackState)
        {
            if (attackState.IsAttackDone())
            {
                if ((Time.time - shotTime > attackInterval))
                {
                    attackState.Initialize(gameObject, anim);
                }
            }
        }
    }

    public override Rigidbody OnRangeAttack()
    {
        // Sinil - For sake of playing appropriate sound
        FindObjectOfType<AudioManager>().PlaySpatial("EnemyRangeAttack", gameObject.transform.position);
        Vector3 initPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        Rigidbody rb = Instantiate(projectile, initPos, Quaternion.identity).GetComponent<Rigidbody>();
        shotTime = Time.time;
        return rb;
    }
}
