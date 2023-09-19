/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 11/23/2021
    Desc    : UI script for weapon "Picture".
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSkillTimer : MonoBehaviour
{
    public float remaining;
    public float maxTime = 10.0f;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        remaining = maxTime;
        image = gameObject.GetComponent<Image>();

        image.material.SetFloat("_MaxTime", maxTime);
        image.material.SetFloat("_RemainingTime", remaining);
    }

    // Update is called once per frame
    void Update()
    {
        image.material.SetFloat("_RemainingTime", remaining);
    }
}
