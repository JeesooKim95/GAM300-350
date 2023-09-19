/*
    Team    : Speaking Potato
    Author  : Jeesoo Kim
    Date    : 10/12/2021
    Desc    : Weapon Switch
*/
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public int currentWeapon = 0; 

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        int prevWeapon = currentWeapon;

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if(currentWeapon >= transform.childCount - 1)
            {
                currentWeapon = 0;
            }
            else
            {
                currentWeapon++;
            }            
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (currentWeapon <= 0)
            {
                currentWeapon = transform.childCount - 1;
            }
            else
            {
                currentWeapon--;
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = 0;
        }

        // if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        // {
        //     currentWeapon = 1;
        // }

        if (prevWeapon != currentWeapon)
        {
            SelectWeapon();
        }
    }

    //Sangmin 
    //Turn to public to use in skills.cs (-_-)b,,
    public void SelectWeapon()
    {
        int weaponIndex = 0;
        foreach(Transform weapon in transform)
        {
            if(weaponIndex == currentWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            weaponIndex++;
        }
    }
}
