/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 10/05/2021
    Desc    : Base class for FSM enemy classes.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    private bool showDebugLog = false;
    protected Rigidbody rigidBody;
    protected State currentState = null;
    protected float stopTimer;
    public Vector3 velocity { get; set; }

    // 11 09 Haewon, temp for notifying
    private bool notifyFlag = false;

    //Jina Hyun: 10/28: Temp for m3
    public bool throwItem = false;
    public SkinnedMeshRenderer meshRenderer = null; // Use SkinnedMeshRenderer instead of MeshRenderer
    protected EnemyAnimBase anim = null;

    // 11/18 Haewon, do not count for map clear condition if it is false
    public bool isSummoned = false;

    // 11/30 Haewon, For navmesh test purpose
    public NavMeshAgent agent;

    // 02/3 Sean, for boss onDeath
    private bool isBoss = false;

    // sinil kang
    protected Status status;

    //04/14 Sangmin to add destroy particle
    public GameObject destroyParticle;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        currentState = new EmptyState();
        
        //Sangmin
        if(cardValue == 0)
        {
            int randomValue;
            int randomType = Random.Range(1, 5);

            Scene currentScene = SceneManager.GetActiveScene();
            Debug.Log("currentScene" + currentScene.name);

            if(currentScene.name == "M5_Forest")
                randomValue = Random.Range(1, 13);
            else if(currentScene.name == "M5_Desert")
                randomValue = Random.Range(5, 13);
            else if(currentScene.name == "M5_Snowfield")
                randomValue = Random.Range(7, 13);
            else if(currentScene.name == "Boss_M4")
                randomValue = Random.Range(9, 13);
            else
                randomValue = Random.Range(1, 13);

            cardValue = randomValue;
            enemyType = (Card.CardType)randomType;
        }

        if (!meshRenderer)
            meshRenderer = gameObject.transform.Find("body").GetComponent<SkinnedMeshRenderer>();

        anim = gameObject.GetComponent<EnemyAnimBase>();
        agent = gameObject.GetComponent<NavMeshAgent>();


        status = GetComponent<Status>();
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (!isFrozen && stopTimer < 0f)
        {
            currentState.FixedUpdate();

            rigidBody.MovePosition(transform.position + velocity * Time.deltaTime);

        }
        else
        {
            stopTimer -= Time.deltaTime;
        }

    }

    protected void LogEnemyDebug(string text)
    {
        if (showDebugLog)
        {
            Debug.Log(text);
        }
    }

    public void SetStopTimer(float time)
    {
        stopTimer = time;
    }

    public bool IsStopped()
    {
        return stopTimer > 0.0f;
    }

    protected void InitializeWithState(State initState)
    {
        currentState = initState;
        currentState.Initialize(gameObject, anim);
        LogEnemyDebug(gameObject.name + " initialized with " + initState.name);
    }

    protected void SetNextState(State nextState)
    {
        if(currentState != null)
            currentState.Exit();
        currentState = nextState;
        currentState.Initialize(gameObject, anim);
    }

    public virtual void OnNotifyPlayer()
    {

    }

    public void NotifyPlayer(float range)
    {
        if (notifyFlag == false)
        {
            EnemyBase[] allEnemies = GameObject.FindObjectsOfType<EnemyBase>();
            foreach (EnemyBase currentEnemy in allEnemies)
            {
                float distanceToEnemy = Vector3.Distance(currentEnemy.transform.position, transform.position);
                if (distanceToEnemy < range)
                {
                    currentEnemy.OnNotifyPlayer();
                }
            }
            notifyFlag = true;
        }
    }

    public virtual void Attack() { }

    //     Author  : Sean Kim
    //     Date    : 10/5/2021
    public virtual Rigidbody OnRangeAttack() { return rigidBody; }

    // Author   : Sinil Kang, 11/17/2021
    public virtual void PlayMoveSound(Vector3 position) { }

    public virtual void OnHeal(int healAmount)
    {
        GetComponentInChildren<HealEffectManager>().Play();

        GetComponent<Status>().OnHeal(healAmount);
    }

    public virtual void OnDrawGizmos()
    {
        if (currentState != null)
        {
            currentState.OnDrawGizmos();
        }
    }
    //Sean
    public void SetBoss()
    {
        isBoss = true;
    }
    public void OnDeath()
    {
        if(status)
            status.EndCombat();

        if(isBoss)
        {
            Debug.Log(gameObject.name + " On Death");
        }
        else
        {
            ThrowItem();
            Destroy(gameObject);

            //Sangmin 04-14
            if (destroyParticle)
            {
                Vector3 pos = transform.position;
                Vector3 newTargetPos = new Vector3(pos.x, pos.y + 1f, pos.z);

                Vector3 playerToEnemy =
                    newTargetPos - GameObject.FindGameObjectWithTag("Player").transform.position;
                Quaternion rot = Quaternion.LookRotation(playerToEnemy);
                GameObject obj = GameObject.Instantiate(destroyParticle, newTargetPos, rot);
                Destroy(obj, 5f);
            }



                //Debug.Log("enemy dead");
                // for M3 build - no lv manager
                if (isSummoned == true)
            {
                return;
            }

            GameObject lvManager = GameObject.Find("LevelManager");
            if (lvManager != null)
            {
                lvManager.GetComponent<LevelManager>().IncreaseKilledEnemyNum();
            }

            //Sangmin - commented by Haewon. Replaced for card drop from enemy
            //GameObject.Find("PlayerHandsManager").GetComponent<PlayerHandsManager>().Add(new Card(enemyType, cardValue));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 knockback = (collision.gameObject.transform.position - gameObject.transform.position).normalized * 20.0f;
            knockback.y = Mathf.Sqrt(1f * -2f * -9.81f);
            collision.gameObject.GetComponent<PlayerStatus>().OnTakeDamage(10, knockback);
        }
    }

    //     Author  : Sinil Kang
    //     Date    : 10/5/2021
    [SerializeField]
    public GameObject itemPrefab;
    public void ThrowItem()
    {
        if (throwItem)
        {
            GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            item.GetComponent<Rigidbody>().velocity = new Vector3(0, 5, 1);

            //Sangmin
            //22/01/15
            CardObject itemInfo = item.GetComponent<CardObject>();

            if(itemInfo != null)
            {
                itemInfo.card.type = enemyType;
                itemInfo.card.value = cardValue;
            }
            
            FindObjectOfType<AudioManager>().Play("ItemThrowed");
        }
    }
    // Author : Sangmin Kim
    // Data : 10/18/2021
    [SerializeField]
    public Card.CardType enemyType = Card.CardType.None;
    public int cardValue = 0;
    private bool isFrozen = false;
    public Material freezeMaterial;
    public Material ogMaterial;
    public void Freeze()
    {
        isFrozen = true;

        if (freezeMaterial && ogMaterial)
        {
            meshRenderer.material = freezeMaterial;
        }

        Invoke("UnFreeze", 3.0f);
    }
    private void UnFreeze()
    {
        isFrozen = false;
        meshRenderer.material = ogMaterial;
    }

    public virtual void SummonStart()
    {
        
    }
}
