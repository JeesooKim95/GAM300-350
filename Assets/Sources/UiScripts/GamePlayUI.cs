/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                10/24/2021
 *  Contributor:       Su Kim
 *  Description:      Set the Status UI for Game play
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{
    public Image reloadImage;
    public Text reloadText;
    private float reloadingTime;
    private bool reloadCheck;
    public float remainTime;

    public GameObject reloadImageObj;
    public GameObject reloadTextObj;


    public Image dashImage;
    public Text dashText;
    private float dashTime;
    private bool dashCheck;
    public float remainTimeforDash;

    public GameObject dashImageObj;
    public GameObject dashTextObj;

    public GameObject weaponType1;
    public GameObject weaponType2;

    public int weaponType;



    // Start is called before the first frame update
    void Start()
    {
        GameObject gun = GameObject.FindWithTag("Gun");
        if (gun != null)
        {
            reloadingTime = gun.GetComponent<WeaponSystem>().reloadTime;
            reloadingTime -= 1.31f;
            remainTime = 0.0f;
        }

        dashTime = GameObject.FindWithTag("Player").GetComponent<PlayerDash>().cooldownTime;
        remainTimeforDash = dashTime;

        if (!reloadImage)
            reloadImage = transform.Find("ReloadImage").gameObject.GetComponent<Image>();

        if (!reloadText)
            reloadText = transform.Find("ReloadCount").gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused == true) return;

        ReloadingUI();
        dashUI();
        Timer();
        //CurrentGunUI();
    }


    void ReloadingUI()
    {
        if (GameObject.FindWithTag("Player") != null && PauseMenu.GameIsPaused == false)
        {
            GameObject gun = GameObject.FindWithTag("Gun");
            if (gun != null)
            { 
                reloadCheck = gun.GetComponent<WeaponSystem>().reloading;
            }
        }

        if (reloadCheck == false)
        {
            SetTimer(reloadingTime, 1);
            //reloadImageObj.SetActive(false);
            //reloadTextObj.SetActive(false);
        }
        else
        {
            reloadImageObj.SetActive(true);
            //reloadTextObj.SetActive(true);
        }
    }

    void dashUI()
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            dashCheck = GameObject.FindWithTag("Player").GetComponent<PlayerDash>().isDashing;
        }

        if (dashCheck == false)
        {
            SetTimer(dashTime, 2);
            //dashImageObj.SetActive(false);
            //dashTextObj.SetActive(false);
        }
        else
        {
            dashImageObj.SetActive(true);
            //dashTextObj.SetActive(true);
        }

    }
    void CurrentGunUI()
    {
        //weaponType = GameObject.Find("Weapon Holder").GetComponent<WeaponSwitch>().currentWeapon;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponType = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponType = 2;
        }


        if (weaponType == 1)
        {

            weaponType1.SetActive(true);
            weaponType2.SetActive(false);
        }
        else
        {
            weaponType1.SetActive(false);
            weaponType2.SetActive(true);
        }


        
    }

    void Timer()
    {
        if (remainTime > 0.0f)
        {
            reloadImage.fillAmount = remainTime / reloadingTime;
            remainTime -= Time.deltaTime;
        }
        

        if (remainTimeforDash > 0.0f)
        {
            dashImage.fillAmount = remainTimeforDash / dashTime;
            remainTimeforDash -= Time.deltaTime;
            //dashText.text = ((int) remainTimeforDash + 1).ToString();
        }
        else
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.GetComponent<PlayerDash>().isDashing = false;
                remainTimeforDash = dashTime;
            }
        }
    }


    public void SetTimer(float set, int a)
    {
        if (a == 1)
        {
            reloadingTime = set;
            remainTime = reloadingTime;
            reloadImage.fillAmount = 1.0f;
        }
        else if (a == 2)
        {
            dashTime = set;
            remainTimeforDash = dashTime;
        }
    }


}
