using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Skills : MonoBehaviour
{
    enum skillType{
        WeaponUpgrade = 0,
        Heal,
        Status
        
    }

    public class Skill
    {
        public List<Action> weaponSkills = new List<Action>();
        public List<Action> healthSkills = new List<Action>();
        public List<Action> statusSkills = new List<Action>();
    }
    Skill top;
    Skill pair;
    Skill twopair;
    Skill triple;
    Skill fullHouse;
    Skill fourCard;
    Skill straight;
    Skill flush;
    Skill straightFlush;
    List<float> lightIntensities;
    public UpgradeInfo upgradeInfo;
    public GameObject secondWeapon;
    public GameObject secondWeaponHolder;
    public MeshRenderer gunMesh;
    private MeshRenderer currentChangedMesh;
    public MeshRenderer waterHolderMesh;
    public Material gunGlowingMat;
    public Material gunMeshDefaultMaterial;
    public Material waterHolderMeshDefaultMaterial;
    public WeaponSystem weaponSystem;
    public WeaponSystem secondWeaponSystem;
    public PlayerStatus playerStatus;
    public PlayerMovement movement;
    public GameObject[] electricParticles;
    public SwapGun swapGun;
    private Color blueColor = new Color(0, 0.3568f, 0.7490f);

    private Coroutine prevCo;
    private float originalSpeed;
    public float timer;
    public float maxPlayerSpeed;    
    private bool skillUsed;
    private bool isRecoveringMode;
    private int recoverAmount;
    private float recoverTimer;
    public float ogRecoverRateTimer;

    // Sinil - For sake of playing heal sounds
    AudioManager audioManager;

    IEnumerator InvokeActionAfterTimer(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        
        action();
    }

    void UseCard(Skill skill)
    {
        //ParticleSystem particleInfo = particle.GetComponent<ParticleSystem>();
        //Debug.Log("UseCard");
        //Debug.Log("Skill Count : " + skill.weaponSkills.Count);
        
        int randomIndex;
        if(playerStatus.currentHealth < playerStatus.maxHealth)
        {
            if(skill.healthSkills.Count > 0)
            {
                randomIndex = UnityEngine.Random.Range(0, skill.healthSkills.Count);

                skill.healthSkills[randomIndex]();

                //particleInfo.startColor = Color.green;
                return;
            }
        }
        
        if(skill.statusSkills.Count > 0)
        {
            randomIndex = UnityEngine.Random.Range(0, skill.statusSkills.Count);

            skill.statusSkills[randomIndex]();
            //particleInfo.startColor = Color.blue;
            return;
        }
        randomIndex = UnityEngine.Random.Range(0, skill.weaponSkills.Count);
        skill.weaponSkills[randomIndex]();

        if(skill != twopair && skill != fullHouse)
        {
            secondWeaponSystem.reloading = false;
        }
        waterHolderMesh.material = gunGlowingMat;
        gunMesh.material = gunGlowingMat;

        foreach(GameObject obj in electricParticles)
        {
            obj.SetActive(true);
        }
    }

    void Start()
    {
        timer = 10.0f;
        ogRecoverRateTimer = .5f;
        recoverTimer = ogRecoverRateTimer;
        skillUsed = false;
        prevCo = null;
        //isInfiteBulletMode = false;
        isRecoveringMode = false;
        recoverAmount = 0;
        secondWeaponSystem = secondWeapon.GetComponent<WeaponSystem>();

        top = new Skill();
        pair = new Skill();
        twopair = new Skill();
        triple = new Skill();
        fullHouse = new Skill();
        fourCard = new Skill();
        straight = new Skill();
        flush = new Skill();
        straightFlush = new Skill();
        lightIntensities = new List<float>();

        top.weaponSkills.Add(Top_AttackSpeed);
        pair.weaponSkills.Add(Pair_AttackSpeed);
        twopair.weaponSkills.Add(TwoPair_SecondGun);
        triple.weaponSkills.Add(Triple_AttackSpeed);
        fullHouse.weaponSkills.Add(FullHouse_Attack);
        fourCard.weaponSkills.Add(Fourcard_Attack);
        straight.weaponSkills.Add(Straight_Attack);
        flush.weaponSkills.Add(Flush_Attack);
        straightFlush.weaponSkills.Add(StraightFlush_Attack);

        upgradeInfo = GetComponent<UpgradeInfo>();
        swapGun = GetComponent<SwapGun>();

        originalSpeed = movement.speed;

        audioManager = FindObjectOfType<AudioManager>();

        float intensityStep = 0.3f;

        for(int i = 0; i < 9; ++i)
        {
            float defaultIntensity = 1.5f;
            float adder = intensityStep * i;

            lightIntensities.Add(defaultIntensity + adder);
        }
    }

    void Update()
    {
        if(skillUsed == true)
        {
            //Debug.Log("Update");

            if(timer > 0f)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                if(!secondWeaponSystem.reloading)
                {
                    SetDefaultStatus();
                    skillUsed = false;
                }
            }
        }

        // if(isInfiteBulletMode)
        //     weaponSystem.bulletsLeft = weaponSystem.magazineSize;

        if(isRecoveringMode)
        {
            if(recoverTimer > 0f)
            {
                recoverTimer -= Time.deltaTime;
            }
            else
            {
                recoverTimer = ogRecoverRateTimer;
                playerStatus.Heal(recoverAmount);
            }
        }
    }

    public void UseSkill(HandsType type)
    {
        Debug.Log("UseSkill");
        switch(type)
        {
            case HandsType.Top:
            UseCard(top);
            break;

            case HandsType.Pair:
            UseCard(pair);
            break;
            
            case HandsType.TwoPair:
            UseCard(twopair);
            break;

            case HandsType.Triple:
            UseCard(triple);
            break;

            case HandsType.FullHouse:
            UseCard(fullHouse);
            break;

            case HandsType.FourCard:
            UseCard(fourCard);
            break;

            case HandsType.Straight:
            UseCard(straight);
            break;

            case HandsType.Flush:
            UseCard(flush);
            break;
            
            case HandsType.StarightFlush:
            UseCard(straightFlush);
            break;

            default:
            UseCard(top);
            break;
        }
        timer = 10f;
        skillUsed = true;
        weaponSystem.isShootSkillBullet = true;
        
        if(prevCo != null)
        {
            StopCoroutine(prevCo);
            prevCo = null;
        }
    }
    void ChangeElecEmission(int rate)
    {
        for(int i = 0; i < electricParticles.Length; ++i)
        {
            ParticleSystem elec = electricParticles[i].transform.Find("Electricity").GetComponent<ParticleSystem>();
            var emission = elec.emission;
            emission.rateOverTime = rate;
        }
    }
    void ProcedureEffect(int intensityIndex, int rateOverTime, float shootForce = 0.4f)
    {
        gunGlowingMat.SetColor("_EmissionColor", blueColor * lightIntensities[intensityIndex]);
        ChangeElecEmission(rateOverTime);

        weaponSystem.shootForce = shootForce;
        secondWeaponSystem.shootForce = shootForce;
    }
    void Top_AttackSpeed()
    {
        upgradeInfo.top.SetWeaponSystem(weaponSystem, secondWeapon);
        Top_Heal();
        Top_SpeedUp();
        ProcedureEffect(0, 0);
    }
    void Top_Heal()
    {
        isRecoveringMode = true;
        recoverAmount = 1;
        ogRecoverRateTimer = 0.7f;
        
        PlayHealSkillSound();
        
    }
    void Top_SpeedUp()
    {
        float increasedSpeed = movement.speed + 5f;
        movement.speed = Mathf.Min(increasedSpeed, maxPlayerSpeed);

        PlaySpeedUpSkillSound();
    }
    void Pair_AttackSpeed()
    {
        //Top_SpeedUp();
        Pair_Heal();
        upgradeInfo.pair.SetWeaponSystem(weaponSystem, secondWeapon);

        ProcedureEffect(1, 0);
    }
    void Pair_Heal()
    {
        isRecoveringMode = true;
        recoverAmount = 2;
        ogRecoverRateTimer = 0.5f;

        Top_SpeedUp();

        PlayHealSkillSound();
    }
    void Pair_InfiniteBullet()
    {
    }
    void TwoPair_SecondGun()
    {
        Top_SpeedUp();
        secondWeaponHolder.SetActive(true);
        secondWeaponHolder.GetComponent<WeaponSwitch>().SelectWeapon();

        if(swapGun == null)
        {
            swapGun = GameObject.FindObjectOfType<SwapGun>();
        }

        swapGun.SetSecondGun(GunType.Default);
        upgradeInfo.twoPair.SetWeaponSystem(weaponSystem, secondWeapon);

        ProcedureEffect(2, 100, 0.5f);
    }
    void Triple_AttackSpeed()
    {
        Top_SpeedUp();
        upgradeInfo.triple.SetWeaponSystem(weaponSystem, secondWeapon);

        ProcedureEffect(3, 200, 0.6f);
    }
    void Straight_Attack()
    {
        Top_SpeedUp();
        Top_SpeedUp();
        upgradeInfo.straight.SetWeaponSystem(weaponSystem, secondWeapon);

        ProcedureEffect(4, 300, 0.5f);
    }

    void FullHouse_Attack()
    {
        Top_SpeedUp();
        Top_SpeedUp();
        secondWeaponHolder.SetActive(true);
        secondWeaponHolder.GetComponent<WeaponSwitch>().SelectWeapon();

        if(swapGun == null)
        {
            swapGun = GameObject.FindObjectOfType<SwapGun>();
        }

        swapGun.SetSecondGun(GunType.ShotGun);
        upgradeInfo.fullHouse.SetWeaponSystem(weaponSystem, secondWeapon);

        ProcedureEffect(5, 400, .5f);
    }
    
    void Flush_Attack()
    {
        Top_SpeedUp();
        Top_SpeedUp();
        upgradeInfo.flush.SetWeaponSystem(weaponSystem, secondWeapon);
        ProcedureEffect(6, 500, 0.4f);
    }
    void Fourcard_Attack()
    {
        Top_SpeedUp();
        Top_SpeedUp();
        upgradeInfo.fourCard.SetWeaponSystem(weaponSystem, secondWeapon);

        ProcedureEffect(7, 600, .5f);

    }

    void StraightFlush_Attack()
    {
        Top_SpeedUp();
        Top_SpeedUp();
        upgradeInfo.straightFlush.SetWeaponSystem(weaponSystem, secondWeapon);
        ProcedureEffect(8, 1000, .5f);
    }
    

    public void SetDefaultStatus(bool useSound = true)
    {
        if(useSound)
            audioManager.Play("CardsPowerOff");

        if(!upgradeInfo)
            upgradeInfo = GetComponent<UpgradeInfo>();

        upgradeInfo.basic.SetWeaponSystem(weaponSystem, secondWeapon);

        movement.speed = originalSpeed;
        isRecoveringMode = false;
        recoverAmount = 0;
        weaponSystem.isShootFourCardBullet = false;
        weaponSystem.isShootStraightBullet = false;
        weaponSystem.isShootSkillBullet = false;
        weaponSystem.shootForce = 0.4f;
        secondWeaponSystem.shootForce = 0.4f;
        
        secondWeaponSystem.reloading = false;
        secondWeaponSystem.bulletsLeft = weaponSystem.bulletsLeft;

        secondWeaponHolder.SetActive(false);

        gunMesh.material = gunMeshDefaultMaterial;
        waterHolderMesh.material = waterHolderMeshDefaultMaterial;

        foreach(GameObject obj in electricParticles)
        {
            obj.SetActive(false);
        }
    }

    // Sinil - for sake of playing heal skill sound
    private void PlayHealSkillSound()
    {
        audioManager.Play("PlayerSkillHeal");
    }
    private void PlaySpeedUpSkillSound()
    {
        audioManager.Play("PlayerSkillSpeedUp");
    }
}
