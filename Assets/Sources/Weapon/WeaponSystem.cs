/*
    Team    : Speaking Potato
    Author  : Jeesoo Kim
    Date    : 09/17/2021
    Desc    : Weapon System
*/
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;

public class WeaponSystem : MonoBehaviour
{
    //bullet
    public GameObject bullet;
    public float shootForce, upForce;

    //Weapon State
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulltetPerTap;
    public bool allowButtonHold, reloading;
    public int bulletsLeft, bulletsShot;

    bool shooting, readytoShoot;

    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;
    public Camera Cam;
    public Transform aimPoint;

    //Recoil
    public Rigidbody playerRB;
    public float recoilForce;

    //player
    public GameObject player;


    //Sangmin
    public bool isShootStraightBullet;
    public bool isShootFourCardBullet;
    public bool isShootSkillBullet;
    public GameObject skillBullet;
    public GameObject straightBullet;
    public GameObject fourcardBullet;
    public GameObject waterHolder;
    private Material waterMat;
    public ParticleSystem muzzleParticle;
    public bool flushSkill;
    public int flushBulletShootingCount = 3;
    public LayerMask cardItemLayer;
    public GameObject cardExplainUIObj;
    public Text cardExplainText;
    public PostProcessVolume postVolume;
    public Animator animator;
    public HandsType handType;

    //PostProcessProfile profile;
    //ChromaticAberration chromatic;
    //public ChromaticAberration chromaticEffect;

    // 10/26/2021 Haewon Shon, weapon reload motion purpose
    [SerializeField] private GameObject weaponHolder;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    // SinilKang - Play Shooting sound when shoot
    public AudioManager audioManager;
    public bool previouslyMousePressed;
    public bool previouslyReloading;
    public float volume = 1f;
    
    public void Awake()
    {
        bulletsLeft = magazineSize;
        readytoShoot = true;
        isShootStraightBullet = false;
        isShootStraightBullet = false;
        isShootSkillBullet = false;
        flushSkill = false;

        if (waterHolder != null)
        {
            Renderer rend = waterHolder.GetComponent<Renderer>();
            waterMat = rend.material;
        }

        //profile = postVolume.profile;
        //chromatic = profile.GetSetting<ChromaticAberration>();


        // Sinil - Init audio manager
        audioManager = FindObjectOfType<AudioManager>();
        volume = 1f;
    }


    private void Update()
    {
        reloadTime = 15.0f * timeBetweenShooting;
        MyInput();
        if (ammunitionDisplay != null)
        {
            ammunitionDisplay.SetText(bulletsLeft / bulltetPerTap + " / " + magazineSize / bulltetPerTap);
        }

        // Sinil - for sake of unique sound at the first shot.
        previouslyMousePressed = Input.GetKey(KeyCode.Mouse0);
        previouslyReloading = reloading;
        
        if(volume > 1f)
        {
            volume -= Time.deltaTime * 3;
            if(volume < 1f)
            {
                volume = 1f;
            }
        }
    }


    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            StartCoroutine(Reload());
        }
        //Reload when empty
        if (readytoShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            StartCoroutine(Reload());
        }

        //SHooting
        if (readytoShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            if (!flushSkill)
                Shoot();
            else
            {
                for (int i = 0; i < flushBulletShootingCount; ++i)
                {
                    Shoot();
                }
            }

            if (waterHolder != null)
            {
                float waterFill = waterMat.GetFloat("_FillAmount");

                waterMat.SetFloat("_FillAmount", waterFill + 0.0021f);
            }
        }

    }

    private void Shoot()
    {
        PlayShootingSounds(isShootStraightBullet);

        readytoShoot = false;

        Ray ray = Cam.ViewportPointToRay(new Vector3(.5f, .5f, .0f));  //ray through the middle of screen
        RaycastHit hit;

        //check for hit
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        //direction
        //Vector3 directionNoSpread = targetPoint - aimPoint.position;
        Vector3 directionSpread = player.GetComponentInChildren<MainCameraLogic>().transform.forward;
        //Vector3 directionSpread = targetPoint - aimPoint.position;
        // if(flushSkill)
        // {
        //     float spreadForce = 0.1f;
        //     directionSpread.x += Random.Range(-spreadForce , spreadForce);
        //     directionSpread.y += Random.Range(-spreadForce , spreadForce);
        //     directionSpread.z += Random.Range(-spreadForce , spreadForce);
        // }

        float spreadForce = 0.03f;
        directionSpread.x += Random.Range(-spreadForce, spreadForce);
        directionSpread.y += Random.Range(-spreadForce, spreadForce);
        directionSpread.z += Random.Range(-spreadForce, spreadForce);

        //speed
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //spread
        //Vector3 directionSpread = directionNoSpread + new Vector3(x, y, 0);

        //Sangmin
        GameObject usingBullet = bullet;
        
        if (isShootStraightBullet)
                usingBullet = straightBullet;
        else if (isShootFourCardBullet)
                usingBullet = fourcardBullet;
        else if(isShootSkillBullet)
                usingBullet = skillBullet;

        //Instantiate bullet
        GameObject currentBullet = Instantiate(usingBullet, aimPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionSpread.normalized;

        //Force of bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(Cam.transform.up * upForce, ForceMode.Impulse);


        bulletsLeft--;
        bulletsShot++;

        //Invokce resetShoot function
        Invoke("ResetShoot", timeBetweenShooting);

        //Repeat this if muliple bulletsPerTap
        //if (bulletsShot < bulltetPerTap && bulletsLeft > 0)
        if (bulletsShot <= bulltetPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);

            //if upgrade > pair
            if(handType > HandsType.Pair)
                animator.SetTrigger("Shoot");

            //Add Recoil
            //playerRB.AddForce(-directionSpread.normalized * recoilForce, ForceMode.Impulse);
            //player.GetComponent<PlayerMovement>().Knockback(transform.forward * -1f * recoilForce, 0.2f);
        }

        if (muzzleParticle != null)
        {
            muzzleParticle.Emit(100);
        }
    }

    private void ResetShoot()
    {
        readytoShoot = true;
    }
    //Sangmin 2022/03/01
    private IEnumerator Reload()
    {
        reloading = true;

        FindObjectOfType<AudioManager>().Play("Reloading");

        animator.SetBool("Reload", reloading);
        yield return new WaitForSeconds(reloadTime);

        ReloadComplete();
        animator.SetBool("Reload", reloading);
    }


    private void ReloadComplete()
    {
        bulletsLeft = magazineSize;
        reloading = false;
        if (waterHolder != null)
        {
            waterMat.SetFloat("_FillAmount", 0.4f);
        }
        weaponHolder.transform.localPosition = originalPosition;
        weaponHolder.transform.localRotation = originalRotation;
    }

    // Sinil - for sake of play shoot sound or ice shoot sound
    private void PlayShootingSounds(bool isIceShoot)
    {
        if (audioManager != null)
        {
            if((previouslyMousePressed == false && Input.GetKeyDown(KeyCode.Mouse0))|| previouslyReloading)
            {
                audioManager.Play("FirstShot");
                volume = 4f;
            }

            if (isIceShoot)
            {
                audioManager.PlayIceShoot(volume);
            }
            else
            {
                audioManager.PlayShoot(volume);
            }
        }
    }
}
