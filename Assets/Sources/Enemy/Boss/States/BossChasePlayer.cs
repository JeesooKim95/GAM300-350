/*
    Team    : Speaking Potato
    Author  : Sinil Kang
    Date    : 10/17/2021
    Desc    : Boss Chase player
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChasePlayer : State
{

    [SerializeField]
    private float scaler = 0.6f;
    private Vector3 starting;
    private Vector3 destination;

    private GameObject player;

    private float localTimer;

    private bool isChaseDone = false;
    private Vector3 position;
    private Vector3 forwardVector;
    private bool isMoveAudioPlayed = false;
    public override void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        name = "Chasing";
        base.Initialize(enemyRef, anim);

        starting = enemy.transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        destination = player.transform.position;

        localTimer = 0f;

        isChaseDone = false;
        position = starting;

        SetForwardVector();

        isMoveAudioPlayed = false;
    }

    public override void FixedUpdate()
    {
        localTimer += Time.deltaTime * scaler;
        destination = player.transform.position;

        SetForwardVector();
        if (localTimer > 0.5f)
        {
            if(isMoveAudioPlayed == false)
            {
                isMoveAudioPlayed = true;


                enemy.PlayMoveSound(destination);
            }
            else
            {

            }
        }
        if (localTimer > 0.925f)
        {
            isChaseDone = true;
        }
        else
        {
            float t = easeInBack(localTimer);
            position = (1 - t) * starting + t * destination;
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

    public bool IsChaseDone()
    {
        return isChaseDone;
    }

    public Vector3 GetEnemyCurrentPosition()
    {
        if(position.y < 15.5f)
        {
            position.y = 15.5f;
        }
        return position;
    }

    public Vector3 GetForwardVector()
    {
        return forwardVector;
    }

    public void SetForwardVector()
    {
        float x = destination.x - starting.x;
        float z = destination.z - starting.z;
        forwardVector = new Vector3(x, 0, z);
    }
}
