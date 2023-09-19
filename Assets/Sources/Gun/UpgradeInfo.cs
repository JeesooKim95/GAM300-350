/*
    Team    : Speaking Potato
    Author  : Jina Hyun
    Date    : 11/04/2021
    Desc    : Gun power up information
*/
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PowerUp
{
    public enum Speed { Default, X2, X3, X4 };

    public GunType gunType = GunType.Default;
    public Speed speed = Speed.Default;
    public bool hasDualGun = false;
    public bool hasFreezeAbility = false;
    public Sprite spriteForUI = null;
    public Sprite spriteForUI_2 = null;

    [HideInInspector]
    public SwapGun swapGun = null;
    [HideInInspector]
    public GameObject weaponUI = null;
    [HideInInspector]
    public GameObject weaponUI_2 = null;

    Vector2 GetShootingTime()
    {
        // x : Time between shooting
        // y : Time between shots
        switch (speed)
        {
            case Speed.X2:
                return new Vector2(0.06f, 0.06f);
            case Speed.X3:
                return new Vector2(0.04f, 0.04f);
            case Speed.X4:
                return new Vector2(0.03f, 0.03f);
        }
        return new Vector2(0.08f, 0.08f);
    }

    public void SetWeaponSystem(WeaponSystem ws, GameObject dualGun)
    {
        Vector2 time = GetShootingTime();


        ws.timeBetweenShooting = time.x;
        ws.timeBetweenShots = time.y;

        ws.isShootFourCardBullet = (gunType == GunType.BombGun);
        ws.isShootStraightBullet = hasFreezeAbility;
        ws.flushSkill = (gunType == GunType.ShotGun);

        if (dualGun)
        {
            WeaponSystem ws2 = dualGun.GetComponent<WeaponSystem>();
            ws2.timeBetweenShooting = time.x;
            ws2.timeBetweenShots = time.y;
            if (hasDualGun)
                dualGun.SetActive(true);
            else
                dualGun.SetActive(false);
        }

        if (weaponUI)
        {
            Image image = weaponUI.GetComponent<Image>();
            if (image)
            {
                if (spriteForUI == null)
                {
                    weaponUI.SetActive(false);
                }
                else
                {
                    weaponUI.SetActive(true);
                    image.sprite = spriteForUI;
                }
            }
        }
        if (weaponUI_2)
        {
            Image image = weaponUI_2.GetComponent<Image>();
            if (image)
            {
                if (spriteForUI_2 == null)
                {
                    weaponUI_2.SetActive(false);
                }
                else
                {
                    weaponUI_2.SetActive(true);
                    image.sprite = spriteForUI_2;
                }
            }
        }

        if(swapGun != null)
            swapGun.ChangeGunModelTo(gunType);
    }
}

public class UpgradeInfo : MonoBehaviour
{
    public GameObject powerUpUI = null;
    public GameObject powerUpUI_2 = null;

    [Header("Poker Hands")]
    public PowerUp basic = new PowerUp();
    public PowerUp top = new PowerUp();
    public PowerUp pair = new PowerUp();
    public PowerUp twoPair = new PowerUp();
    public PowerUp triple = new PowerUp();
    public PowerUp fullHouse = new PowerUp();
    public PowerUp fourCard = new PowerUp();
    public PowerUp straight = new PowerUp();
    public PowerUp flush = new PowerUp();
    public PowerUp straightFlush = new PowerUp();

    private void Start()
    {
        SwapGun swap_model = gameObject.GetComponent<SwapGun>();
        basic.swapGun = swap_model;
        top.swapGun = swap_model;
        pair.swapGun = swap_model;
        twoPair.swapGun = swap_model;
        triple.swapGun = swap_model;
        fullHouse.swapGun = swap_model;
        fourCard.swapGun = swap_model;
        straight.swapGun = swap_model;
        flush.swapGun = swap_model;
        straightFlush.swapGun = swap_model;

        basic.weaponUI = powerUpUI;
        top.weaponUI = powerUpUI;
        pair.weaponUI = powerUpUI;
        twoPair.weaponUI = powerUpUI;
        triple.weaponUI = powerUpUI;
        fullHouse.weaponUI = powerUpUI;
        fourCard.weaponUI = powerUpUI;
        straight.weaponUI = powerUpUI;
        flush.weaponUI = powerUpUI;
        straightFlush.weaponUI = powerUpUI;

        basic.weaponUI_2 = powerUpUI_2;
        top.weaponUI_2 = powerUpUI_2;
        pair.weaponUI_2 = powerUpUI_2;
        twoPair.weaponUI_2 = powerUpUI_2;
        triple.weaponUI_2 = powerUpUI_2;
        fullHouse.weaponUI_2 = powerUpUI_2;
        fourCard.weaponUI_2 = powerUpUI_2;
        straight.weaponUI_2 = powerUpUI_2;
        flush.weaponUI_2 = powerUpUI_2;
        straightFlush.weaponUI_2 = powerUpUI_2;
    }
}
