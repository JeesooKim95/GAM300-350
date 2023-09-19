/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 10/20/2021
    Desc    : Player-specialized status class
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : Status
{

    public CameraShake cameraShake;
    public float cameraShakeScale = 0.3f;

    [Header("Player Shield")]
    public int maxShield;
    public float shieldRecoverDelay;
    public int shieldRecoverRate;

    private int currentShield;
    private float currentShieldFloat;
    private float shieldRecoverTimer;

    protected ShieldBar shieldBar = null;
    
    [Header("Heal/Damage Effect")]
    public float invincibleTime;
    private float timer = 0.0f;
    public GameObject hurtEffectImage;
    public Color hurtEffectColor;

    public float healTime;
    private float healTimer;
    public GameObject healEffectImage;
    public Color healEffectColor;

    // Sinil - For sake of playing sounds
    private AudioManager audioManager;
    private bool playShieldSFX;
    private bool playedShieldSFX;
    private bool shouldResume;
    private void Start()
    {
        healthbar = gameObject.GetComponent<HealthBar>();
        shieldBar = gameObject.GetComponent<ShieldBar>();
        if (healthbar)
            healthbar.Initialize(maxHealth);
        if (shieldBar)
            shieldBar.Initialize(maxShield);
        else
            Debug.Log("shieldBar init not called");

        currentHealth = maxHealth;
        currentShield = maxShield;
        currentShieldFloat = currentShield;
        shieldRecoverTimer = 0.0f;
        timer = 0.0f;
        healTimer = 0.0f;

        audioManager = FindObjectOfType<AudioManager>();
        playShieldSFX = false;
        playedShieldSFX = false;
        shouldResume = false;

        hurtEffectImage.SetActive(false);
        healEffectImage.SetActive(false);
    }

    protected override void Update()
    {
        timer -= Time.deltaTime;
        healTimer -= Time.deltaTime;
        shieldRecoverTimer -= Time.deltaTime;

        if (timer > 0.0f)
        {
            hurtEffectImage.GetComponent<Image>().color = new Color(hurtEffectColor.r, hurtEffectColor.g, hurtEffectColor.b, hurtEffectColor.a * timer / invincibleTime);
        }
        else
        {
            hurtEffectImage.SetActive(false);
        }

        if (healTimer > 0.0f)
        {
            float alphaMultiplier = Mathf.Sin((healTime - healTimer) * Mathf.PI / 2.0f - 0.5f) / 4.0f + 0.75f;
            healEffectImage.GetComponent<Image>().color = new Color(healEffectColor.r, healEffectColor.g, healEffectColor.b, hurtEffectColor.a * alphaMultiplier); // / healTime);
        }
        else
        {
            healEffectImage.SetActive(false);
        }


        PlayShieldRecoverSFX();

        if (currentShield < maxShield && shieldRecoverTimer <= 0.0f)
        {
            currentShieldFloat = Mathf.Min(maxShield, currentShieldFloat + (shieldRecoverRate * Time.deltaTime));
            currentShield = (int)currentShieldFloat;
            shieldBar.UpdateCurrentShield(currentShield);
        }
        base.Update();

        cameraShake = GameObject.Find("MainCamera").GetComponent<CameraShake>();
    }

    public override void OnTakeDamage(int damage, Vector3 knockback)
    {
        if (timer >= 0.0f)
        {
            return;
        }

        timer = invincibleTime;
        shieldRecoverTimer = shieldRecoverDelay;
        hurtEffectImage.SetActive(true);

        int shieldTakeDamage = Mathf.Min(currentShield, damage);
        currentShield -= shieldTakeDamage; // deduct shield first
        currentShieldFloat = (float)currentShield;
        damage -= shieldTakeDamage;
        currentHealth -= damage; // deduct health second

        if (healthbar != null)
        {
            healthbar.UpdateCurrentHealth(currentHealth);
        }
        if (shieldBar != null)
        {
            shieldBar.UpdateCurrentShield(currentShield);
        }

        // camera shake
        ShakeCamera(0.15f);
        // Sinil - play appropriate sound
        audioManager.Play("PlayerGetDamaged");
        // set knockback
        gameObject.GetComponent<PlayerMovement>().Knockback(knockback, 0.5f);
    }


    /*(
     * Sinil.Kang
     * 10/23/2021
     */
    public void ShakeCamera(float time = 0.1f)
    {
        // Do Shake Camera
        StartCoroutine(cameraShake.Shake(time, cameraShakeScale, new Vector3(0.167f, 1.161f, 0.049f)));
    }
    /*(
     * Sinil.Kang
     * 1/20/2022
     */
    public void PlayShieldRecoverSFX()
    {
        // playShieldSFX -> A condition which should play music
        // playedShieldSFX -> Is SFX played?

        if (currentShield >= maxShield)
        {
            audioManager.Stop("PlayerShieldCharge");
            playShieldSFX = false;
            playedShieldSFX = false;
        }
        else if(currentShield < maxShield && shieldRecoverTimer <= 0.0f)
        {
            playShieldSFX = true;
        }

        if(shouldResume == false && playedShieldSFX == false && playShieldSFX == true)
        {
            audioManager.Play("PlayerShieldCharge");
            playedShieldSFX = true;
        }
        else if(shieldRecoverTimer > 0.0f)
        {
            audioManager.Stop("PlayerShieldCharge");
            playShieldSFX = false;
            playedShieldSFX = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && shouldResume == true)
        {
            audioManager.Play("PlayerShieldCharge");
            shouldResume = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            audioManager.Stop("PlayerShieldCharge");
            shouldResume = true;
        }
    }

    public void Heal(int amount = 10)
    {
        currentHealth += amount;
        currentShield += amount;

        currentHealth = Mathf.Min(currentHealth, maxHealth);
        currentShield = Mathf.Min(currentShield, maxShield);

        if(healTimer < 0.0f) 
            healTimer = healTime;
        healEffectImage.SetActive(true);

        if (healthbar != null && shieldBar != null)
        {
            healthbar.UpdateCurrentHealth(currentHealth);
            shieldBar.UpdateCurrentShield(currentShield); 
        }
        else
        {
            Debug.Log("Health & shield are null in player status!");
        }
    }

    public void ResetShield(int amount)
    {
        currentShield = amount;
        shieldBar.ResetShield();
    }
}
