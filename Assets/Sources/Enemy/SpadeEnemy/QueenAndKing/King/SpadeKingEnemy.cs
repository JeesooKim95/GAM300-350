using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class SpadeKingEnemy : EnemyBase
{
    private ClubEnemyChase chasingState = new ClubEnemyChase();
    private SpadeKingAttack meleeAttackState = new SpadeKingAttack();

    public float meleeAttackRange = 5.0f;
    public AudioManager audioManager;
    public GameObject queen;

    public CameraShake camShake;


    private float timer;
    private GameObject player;

    [SerializeField]
    private GameObject rushArea;
    private GameObject rushHolder = null;

    private GameObject dangerArea;
    private GameObject dangerHolder = null;

    public bool charging = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        timer = 0f;
        InitializeWithState(chasingState);
        rushHolder = null;
        audioManager = FindObjectOfType<AudioManager>();
        status = GetComponent<Status>();
        status.SetProtection(true);
        camShake = GameObject.Find("MainCamera").GetComponent<CameraShake>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (status.IsProtected() == true)
        {
            if (queen != null)
            {
                if (queen.GetComponent<Status>().currentHealth < 10)
                {
                    status.SetProtection(false);
                }
            }
            else
            {
                status.SetProtection(false);
            }
        }

        if (currentState == meleeAttackState)
        {
            if (meleeAttackState.IsAttackDone() == true)
            {
                SetNextState(chasingState);
            }
        }
        else if (currentState == chasingState)
        {

            if (chasingState.IsDone() == false)
                return;

            float distance = Vector3.Distance(this.transform.position,
                FindObjectOfType<PlayerMovement>().transform.position);

            if (distance < meleeAttackRange)
            {
                SetNextState(meleeAttackState);
            }

            //float distance = Vector3.Distance(this.transform.position,
            //    FindObjectOfType<PlayerMovement>().transform.position);
            //if (distance < meleeAttackRange)
            //{
            //    SetNextState(meleeAttackState);
            //}
        }
    }

    public override void Attack()
    {
        audioManager.PlaySpatial("EnemyAttack", gameObject.transform.position);


        Vector3 initPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        rushHolder = Instantiate(rushArea, transform);
        rushHolder.transform.position = initPos + (transform.forward * 2f);
            rushHolder.transform.localScale = new Vector3(4, 4, 4);
        ShakeCamera();
    }

    public void ShakeCamera()
    {
        StartCoroutine(camShake.Shake(.15f, .4f));
    }
}
