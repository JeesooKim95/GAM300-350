/*
    Team    : Speaking Potato
    Author  : Sean Kim
    Date    : 10/23/2021
    Desc    : Moving to range attack state for boss unit.
*/
using UnityEngine;

public class BossMoveToRangePos : State
{
    [SerializeField]
    private float scaler = 0.3f;

    private Vector3 start;
    private Vector3 destination;

    private GameObject player;

    private float localTimer;

    private Vector3 position;
    private Vector3 forwardVector;
    private bool isMoveDone = false;

    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        name = "Moving";                
        base.Initialize(enemyRef, anim);
        start = enemy.transform.position;
        destination = GameObject.FindGameObjectWithTag("BossDestination").transform.position;
        localTimer = 0f;
        position = start;
        SetForwardVector();
        isMoveDone = false;
    }

    public override void FixedUpdate()
    {
        destination = GameObject.FindGameObjectWithTag("BossDestination").transform.position;
        localTimer += Time.deltaTime * scaler;

        SetForwardVector();

        if (localTimer > 1.0f)
        {
            isMoveDone = true;
        }
        else
        {
            float t = easeInBack(localTimer);
            position = (1 - t) * start + t * destination;
        }
    }

    public override void Exit()
    {

    }

    public float easeInBack(float x)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;

        return c3 * x * x * x - c1 * x * x;
    }

    public bool IsMovingDone()
    {
        return isMoveDone;
    }

    public Vector3 GetEnemyCurrentPosition()
    {
        if (position.y < 6f)
        {
            position.y = 6f;
        }
        return position;
    }

    public Vector3 GetForwardVector()
    {
        return forwardVector;
    }

    public void SetForwardVector()
    {
        float x = destination.x - start.x;
        float z = destination.z - start.z;
        forwardVector = new Vector3(x, 0, z);
    }
    //private Vector3 start;
    //private Vector3 destination;
    //private bool isMoveDone = false;
    //private float t = 0.0f;
    //private float speed = 0.5f;

    //public override void Initialize(GameObject enemyRef)
    //{
    //    name = "Moving";
    //    base.Initialize(enemyRef);
    //    start = enemy.transform.position;
    //    destination = GameObject.FindGameObjectWithTag("BossDestination").transform.position;
    //}

    //public override void FixedUpdate()
    //{
    //    Vector3 currentPos = enemy.transform.position;
    //    if (currentPos == destination)
    //    {
    //        isMoveDone = true;
    //    }
    //    MoveBoss();
    //    t += Time.deltaTime * speed;
    //}

    //private void MoveBoss()
    //{
    //    enemy.transform.position = Vector3.Lerp(start, destination, t);
    //}

    //public override void Exit()
    //{

    //}

    //public bool IsMovingDone()
    //{
    //    return isMoveDone;

    //}
}
