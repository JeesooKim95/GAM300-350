/*
    Team    : Speaking Potato
    Author  : Sangmin Kim
    Date    : 10/17/2021
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerUpgrade : MonoBehaviour
{
    HandsType type;
    public WeaponSystem gunInfo;
    WeaponSystem secondGunInfo;
    public GameObject secondWeapon;

    [SerializeField]
    private GameObject skillTimerObj;
    public GameObject gun;
    public GameObject weaponSpriteObject;

    public float timer = 10f;
    private Coroutine prevCo = null;
    private Action prevAct = null;


    // Jina Hyun: 11/04/2021
    private UpgradeInfo upgradeInfo = null;
    public Skills skill;

    void Start()
    {
        type = HandsType.None;

        if(secondWeapon == null)
            secondWeapon = transform.Find("SecondWeapon").gameObject;

        //upgradeInfo = GameObject.Find("LevelManager").GetComponent<UpgradeInfo>();
        upgradeInfo = GetComponent<UpgradeInfo>();
        secondGunInfo = secondWeapon.GetComponent<WeaponSystem>();

        

        SetToDefault();
    }

    IEnumerator InvokeActionAfterTimer(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        
        action();
    }
    
    // Jina Hyun: 11/04
    void SetToDefault()
    {
        prevCo = null;
        upgradeInfo.basic.SetWeaponSystem(gunInfo, secondWeapon);
    }


    public void SetHands(HandsType hand)
    {
        type = hand;

        gunInfo.handType = hand;
        secondGunInfo.handType = hand;

        Debug.Log("SetHands");
        skill.UseSkill(hand);

        if(skillTimerObj == null)
        {
            SkillTimer temp = GameObject.FindObjectOfType<SkillTimer>();
            if(temp != null)
            {
                skillTimerObj = temp.gameObject;
            }
        }

        if(skillTimerObj != null)
        {
            skillTimerObj.SetActive(true);
            skillTimerObj.GetComponent<SkillTimer>().SetTimer(timer);    
        }
        else
        {
            Debug.Log("skillTimer Image is not setted in PlayerUpgrade.cs");
        }
    }



}
