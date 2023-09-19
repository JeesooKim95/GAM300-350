/*
    Team    : Speaking Potato
    Author  : Jina Hyun
    Date    : 11/04/2021
    Desc    : Change the model of the gun
*/
using UnityEngine;

public class SwapGun : MonoBehaviour
{
    public GameObject defaultGun = null;
    public GameObject shotGun = null;
    public GameObject bombGun = null;

    private GameObject current = null;

    public GameObject secondDefaultGun = null;
    public GameObject secondShotGun = null;
    public GameObject secondBombGun = null;
    private void Start()
    {

        if(bombGun != null)
            bombGun.SetActive(false);
        if(shotGun != null)
            shotGun.SetActive(false);

        current = defaultGun;
    }
    public void ChangeGunModelTo(GunType type)
    {
        if (current != null)
            current.SetActive(false);

        switch (type)
        {
            case GunType.Default:
                current = defaultGun;
                break;
            case GunType.ShotGun:
                current = shotGun;
                break;
            case GunType.BombGun:
                current = bombGun;
                break;
        }
        if (current != null)
            current.SetActive(true);
    }

    public void SetSecondGun(GunType type)
    {
        secondShotGun.SetActive(false);
        secondDefaultGun.SetActive(false);
        secondBombGun.SetActive(false);
        switch(type)
        {
            case GunType.Default:
            secondDefaultGun.SetActive(true);
            break;
            case GunType.ShotGun:
            secondShotGun.SetActive(true);
            break;
            case GunType.BombGun:
            secondBombGun.SetActive(true);
            break;
        }
    }
    public void StopSecondGun()
    {
        secondBombGun.SetActive(false);
        secondShotGun.SetActive(false);
        secondDefaultGun.SetActive(false);
    }
    
}
