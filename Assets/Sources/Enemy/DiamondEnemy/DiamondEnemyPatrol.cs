/*
    Team    : Speaking Potato
    Author  : Sean Kim
    Date    : 10/19/2021
    Desc    : class for enemy type - Diamond.
*/
using UnityEngine;

public class DiamondEnemyPatrol : State
{
    private Vector3 roamingAreaCenter;
    private float roamingAreaRadius = 3f;
    private float speed = 1.0f;
    private float rotationTimer = 0.2f;
    private float rotationAngle = 0.0f;

    private float directionTimer = 5.0f;

    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        name = "Patrol";
        base.Initialize(enemyRef, anim);
        roamingAreaCenter = new Vector3(enemyRef.transform.position.x, 0.0f, enemyRef.transform.position.z);
        SetNewVelocity();
        if (!enemy.IsStopped())
            enemy.SetStopTimer(0.5f);
        rotationTimer = 0.0f;
        enemy.transform.rotation = Quaternion.LookRotation(enemy.velocity, Vector3.up);
        anim.OnWalk(true);

        if (enemy.agent != null)
        {
            enemy.agent.enabled = true; // false;
            enemy.agent.SetDestination(enemy.transform.position);
        }
    }

    public override void FixedUpdate()
    {
        directionTimer -= Time.deltaTime;

        if (rotationTimer > 0.0f)
        {
            Rotate();
            rotationTimer -= Time.deltaTime;
            if (rotationTimer <= 0.0f)
            {
                if (enemy.agent != null)
                {
                    enemy.agent.speed = 3.0f;
                }
                directionTimer = 5.0f;
            }
        }
        else if (directionTimer < 0.0f || Vector3.Distance(new Vector3(enemy.transform.position.x, 0.0f, enemy.transform.position.z), new Vector3(enemy.agent.destination.x, 0.0f, enemy.agent.destination.z)) <= 0.1f)
        {
            float randomTime = Random.Range(0.5f, 0.7f);
            enemy.SetStopTimer(randomTime);
            rotationTimer = randomTime;
            SetNewVelocity();
            Rotate();
        }

        enemy.transform.LookAt(enemy.transform.position + enemy.velocity);
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
        enemy.agent.SetDestination(nextSpot);
        enemy.agent.speed = 0.0f;
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
