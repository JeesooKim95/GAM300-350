/*
    Team    : Speaking Potato
    Author  : Sinil Kang, Sean Kim
    Date    : 10/17/2021
    Desc    : Boss's Behavior code.
*/
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BossBehavior : EnemyBase
{
    BossMeleeAttack baseAttackState = new BossMeleeAttack();
    BossChasePlayer baseChaseState = new BossChasePlayer();
    BossMoveToRangePos bossMovingToRangeState = new BossMoveToRangePos();
    BossRangeAttack rangeAttackState = new BossRangeAttack();

    public float attackInterval = 3.0f;
    public GameObject[] spawnPool;
    public GameObject rangeAttackObject;
    public GameObject ground;

    public GameObject[] obstaclePos;
    private GameObject player;
    [SerializeField]
    

    public GameObject rangeAttackPos;
    public GameObject Obstacle1;
    public GameObject Obstacle2;
    public CameraShake cameraShake;

    //Projectile related
    private float shotTime;
    public float forwardForce = 700.0f;
    public float upForce = 0.0f;

    public int maxHealth = 2000;
    public TextMeshProUGUI healthUI;

    [SerializeField]
    private GameObject attackArea;
    private GameObject areaHolder = null;

    private bool isMoving = false;
    private Vector3 playerPos;
    private bool isFirstPhase = true;
    private bool thirdPhaseBegun = true;
    private int secondPhaseHealth;
    private int thirdPhaseHealth;
    private GameObject[] enemyPool;

    //M3
    private bool usedFirstCheat = false;
    private bool usedSecondCheat = false;
    private bool planeChanged = false;
    //Sinil - for sake of audio
    private AudioManager audioManager;

    //Sangmin
    public BossJackPotWhenClear jackPot;

    // Start is called before the first frame update
    protected override void Start()
    {
        gameObject.GetComponent<EnemyBase>().SetBoss();
        base.Start();
        areaHolder = null;
        audioManager = FindObjectOfType<AudioManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        InitializeWithState(baseChaseState);
        secondPhaseHealth = maxHealth - 500;
        thirdPhaseHealth = secondPhaseHealth - 500;
        cameraShake = GameObject.Find("MainCamera").GetComponent<CameraShake>();

        GetComponent<Status>().SetProtection(false);
    }
    public int GetLastPhaseHealth()
    {
        return thirdPhaseHealth - 500;
    }
    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        int currentHealth = gameObject.GetComponent<Status>().GetHealth();

        if (currentHealth <= 0)
        {
            //SceneManager.LoadScene("ClearScene");            
        }
        
            //Boss Health Bar display
            if (healthUI != null)
            {
                healthUI.SetText(currentHealth + " / " + maxHealth);
            }

            //Boss States
            if (currentHealth <= secondPhaseHealth && currentHealth > thirdPhaseHealth && !isMoving)
            {
                SetNextState(bossMovingToRangeState);
                isMoving = true;
            }
            if (currentState == baseAttackState)
            {
                if(baseAttackState.IsAttackDone())
                {
                    SetNextState(baseChaseState);
                }
            }
            else if(currentState == baseChaseState)
            {
                transform.forward = baseChaseState.GetForwardVector();
                rigidBody.MovePosition(baseChaseState.GetEnemyCurrentPosition());
                if (baseChaseState.IsChaseDone())
                {
                    SetNextState(baseAttackState);
                }
            }
            //if boss health goes below half, move boss to 2nd phase position
            //then change to range attacking and spawn enemies        
            else if (currentState == bossMovingToRangeState)
            {
                transform.forward = bossMovingToRangeState.GetForwardVector();
                rigidBody.MovePosition(bossMovingToRangeState.GetEnemyCurrentPosition());
                GetComponent<Status>().SetProtection(true);
                if (bossMovingToRangeState.IsMovingDone())
                {
                    SetNextState(rangeAttackState);
                GetComponent<Status>().SetProtection(false);
                isFirstPhase = false;
                }
            }
            else if (currentState == rangeAttackState)
            {
                playerPos = GameObject.Find("Player").transform.position;
                playerPos.y = 15.5f;
            transform.LookAt(playerPos);
            if (Time.time - shotTime > attackInterval)
                {
                    rangeAttackState.Initialize(gameObject, anim);
                }
                if(currentHealth <= thirdPhaseHealth && thirdPhaseBegun)
                {
                    if (!planeChanged)
                    {
                        ground.SetActive(false);
                        planeChanged = true;
                    }
                    for(int i = 0; i < 4; i++)
                    {
                        Instantiate(Obstacle1, obstaclePos[i].transform.position, obstaclePos[i].transform.rotation).GetComponent<Rigidbody>();
                    }
                    for (int i = 4; i < 8; i++)
                    {
                        obstaclePos[i].transform.rotation.Set(obstaclePos[i].transform.rotation.x, 90.0f, obstaclePos[i].transform.rotation.z, obstaclePos[i].transform.rotation.w);
                        Instantiate(Obstacle2, obstaclePos[i].transform.position, obstaclePos[i].transform.rotation).GetComponent<Rigidbody>();
                    }
                    thirdPhaseBegun = false;
                }
                else if(currentHealth > thirdPhaseHealth)
                {
                    GameObject[] obstacles = GameObject.FindGameObjectsWithTag("BossObstacle");
                    for(int i = 0; i < obstacles.Length; i++)
                    {
                        Destroy(obstacles[i]);
                    }
                }
            }
        

    }
    public void Update()
    {
        int currentHealth = gameObject.GetComponent<Status>().GetHealth();

        if (currentHealth <= 0)
        {
            audioManager.Play("BossDead");

            //SceneManager.LoadScene("ClearScene");
            if(jackPot == null)
                jackPot = GameObject.FindObjectOfType<BossJackPotWhenClear>();
            
            jackPot.bossDie = true;

            healthUI.SetText("0");

            gameObject.GetComponent<EnemyBase>().meshRenderer.enabled = false;

            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("BossObstacle");
            for(int i = 0; i < obstacles.Length; i++)
            {
                Destroy(obstacles[i]);
            }

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemies.Length; i++)
            {
                Destroy(enemies[i]);
            }
        }

        //int currentHealth = gameObject.GetComponent<Status>().GetHealth();
        //FOR M3 : cheat code for M3
        if (Input.GetKeyDown(KeyCode.M) && !usedFirstCheat)
        {
            gameObject.GetComponent<Status>().SetHealth(secondPhaseHealth);
            usedFirstCheat = true;
        }
        if (Input.GetKeyDown(KeyCode.M) && currentHealth <= secondPhaseHealth && !usedSecondCheat)
        {
            gameObject.GetComponent<Status>().SetHealth(thirdPhaseHealth);
            usedSecondCheat = true;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            gameObject.GetComponent<Status>().ResetHealth();
            usedSecondCheat = false;
            usedFirstCheat = false;
            thirdPhaseBegun = true;
            planeChanged = false;
            isMoving = false;
            SetNextState(baseChaseState);
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            gameObject.GetComponent<Status>().SetHealth(0);
        }
    }
    public override void Attack()
    {
        areaHolder = Instantiate(attackArea, transform);
        areaHolder.transform.localPosition = new Vector3(0f, -1.5f, (transform.localScale.y + transform.localScale.z) / 7f);
        //areaHolder.transform.localScale = new Vector3(1f, transform.localScale.z / transform.localScale.y, (transform.localScale.y / transform.localScale.z));
        areaHolder.transform.localScale = new Vector3(2f, (transform.localScale.z / transform.localScale.y) / 2.0f, (transform.localScale.y / transform.localScale.z));
        ShakeCamera();
        audioManager.Play("BossAttack");
    }

    public override Rigidbody OnRangeAttack()
    {
        audioManager.Play("BossCannonBigAttack");
        Vector3 initPos = new Vector3(transform.position.x, player.transform.position.y - .35f, transform.position.z);
        Rigidbody rb = Instantiate(rangeAttackObject, initPos, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * forwardForce, ForceMode.Impulse);
        rb.AddForce(transform.up * upForce, ForceMode.Impulse);
        shotTime = Time.time;
        return rb;
    }

    public void ShakeCamera()
    {
        StartCoroutine(cameraShake.Shake(.15f, .5f));
    }

    public override void PlayMoveSound(Vector3 position)
    {
        audioManager.PlaySpatial("BossMove", position);
    }
}
