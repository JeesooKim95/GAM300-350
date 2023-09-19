/*
    Team    : Speaking Potato
    Author  : Jina Hyun
    Date    : 09/18/2021
    Desc    : Status Information
*/
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Status : MonoBehaviour
{
    [Header("Status")]
    [Min(1)]
    public int maxHealth = 50;
    [HideInInspector]
    public int currentHealth = 0;

    protected HealthBar healthbar = null;


    // Sinil.kang - for sake of combat audio
    AudioManager audioManager;
    bool hasSentCombatMessage;

    [SerializeField]
    private bool isNotDieAble = false;
    public bool isNotMoving = false;

    // 04/07/2022 Haewon. Set protection for elite boss
    [HideInInspector]
    private bool isProtected = false;
    public GameObject barrier;

    // Reset the current health to the initial health
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        if (healthbar)
            healthbar.ResetHealth();
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthbar = gameObject.GetComponent<HealthBar>();
        if (healthbar)
        {
            if(isProtected == true)
                healthbar.Initialize(maxHealth, true);
            else
                healthbar.Initialize(maxHealth);

            if (gameObject.GetComponent<BossBehavior>() != null)
            {
                healthbar.SetShield(false);
            }
        }
        audioManager = FindObjectOfType<AudioManager>();
        hasSentCombatMessage = false;

        if (barrier != null)
        {
            if (isProtected == true)
            {
                barrier.SetActive(true);
                healthbar.SetShield(true);
            }
            else
            {
                barrier.SetActive(false);
                healthbar.SetShield(false);
            }
        }
    }

    // The actions when take the damages
    /* 10/20/2021 Haewon : This only called for enemy - For player : check Player/PlayerStatus*/
    public virtual void OnTakeDamage(int damage, Vector3 knockback)
    {
        if (isProtected == true) return; // ignore when protection applied

        if (isNotMoving == false)
        {
            BeginCombat();
        }
        else
        {
            DummyAnim dummy_anim = gameObject.GetComponent<DummyAnim>();
            if (dummy_anim)
                dummy_anim.OnPushed();
        }
        currentHealth -= damage;

        // 11/9/2021 Haewon - for aggro purpose
        gameObject.GetComponent<EnemyBase>().NotifyPlayer(3);
        if (healthbar != null)
            healthbar.UpdateCurrentHealth(currentHealth);
    }

    public void OnHeal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (healthbar != null)
        {
            healthbar.UpdateCurrentHealth(currentHealth);
        }
    }

    protected virtual void Update()
    {
        if (healthbar != null)
        {
            healthbar.UpdateHealthBar();
        }

        if (currentHealth <= 0)
        {
            // If there is no healthbar, destory object imediately
            if (healthbar == null)
                OnDeath();
            // If there is healbar, wait for finishing animation and then destroy object
            else if (healthbar.IsAnimationFinished() == true)
            {
                OnDeath();
            }
        }
    }

    public virtual void OnDeath()
    {
        if (gameObject.tag == "Enemy" || gameObject.tag == "Elite")
        {
            if (isNotDieAble)
            {
                ResetHealth();
            }
            else
            {
                if (isNotMoving == false)
                {
                    EndCombat();
                }
                else
                {
                    DummyAnim dummy_anim = gameObject.GetComponent<DummyAnim>();
                    if (dummy_anim)
                        dummy_anim.OnDied();
                }

                FindObjectOfType<AudioManager>().PlaySpatial("EnemyDeath", gameObject.transform.position);
                healthbar.UpdateHealthBar();
                gameObject.GetComponent<EnemyBase>().OnDeath();
            }
        }
        else if (gameObject.tag == "Player")
        {
            // Sinil - Temporarily Remove respawn and load GameOver Scene
            //SceneManager.LoadScene("GameOverScene");
            // gameObject.GetComponent<PlayerRespawn>().Respawn();

            // 04.14 Haewon, move correct scene
            PlayerProgress progress = gameObject.GetComponent<PlayerProgress>();
            string nextSceneName = "";
            if (progress != null)
            {
                if (progress.GetPlayerProgress(PlayerProgress.Progress.FOREST) == false)
                {
                    nextSceneName = "Loss-Forest";
                }
                else if (progress.GetPlayerProgress(PlayerProgress.Progress.DESERT) == false)
                {
                    nextSceneName = "Loss-Desert";
                }
                else if (progress.GetPlayerProgress(PlayerProgress.Progress.SNOW) == false)
                {
                    nextSceneName = "Loss-Snow";
                }
                else
                {
                    nextSceneName = "Loss-Boss";
                }
            }
            SceneManager.LoadScene(nextSceneName);
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    //Sean Kim
    public void SetHealth(int health)
    {
        currentHealth = health;
        if (healthbar != null)
        {
            healthbar.UpdateCurrentHealth(currentHealth);
        }
        else
        {
            Debug.Log("healthbar not exist");
        }

        FindObjectOfType<AudioManager>().Play("EnemyHurt");
    }

    // Sinil kang - for sake of combat BGMs
    public void BeginCombat()
    {
        if (hasSentCombatMessage == false)
        {
            audioManager.StartCombat();
            hasSentCombatMessage = true;
        }
    }
    public void EndCombat()
    {
        if (hasSentCombatMessage == true)
        {
            audioManager.EndCombat();
            hasSentCombatMessage = false;
        }
    }

    // 04/07 Haewon : boss protection
    public void SetProtection(bool set)
    {
        isProtected = set;
        if (barrier != null)
        {
            if (isProtected == true)
            {
                barrier.SetActive(true);
                if (healthbar)
                    healthbar.SetShield(true);
            }
            else
            {
                barrier.SetActive(false);
                if (healthbar)
                    healthbar.SetShield(false);
            }
        }
    }

    public bool IsProtected()
    {
        return isProtected;
    }
}
