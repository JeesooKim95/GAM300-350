/*
    Team    : Speaking Potato
    Author  : Sangmin Kim
    Date    : 10/23/2021
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTimer : MonoBehaviour
{
    public Image image;
    public Text indicator;
    public float remainingTimer;
    public float maxTime = 10.0f;



    void Start()
    {
        remainingTimer = maxTime;

        if (!image)
            image = gameObject.GetComponent<Image>();

        // if (!indicator)
        //     indicator = transform.Find("TimerTextIndicator").gameObject.GetComponent<Text>();
    }

    void Update()
    {
        if(remainingTimer > 0.0f)
        {
            remainingTimer -= Time.deltaTime;
            image.fillAmount = remainingTimer / maxTime;
            //indicator.text = ((int)remainingTimer + 1).ToString();
        }
        else
        {
            remainingTimer = maxTime;
            gameObject.SetActive(false);
        }
    }
    public void SetTimer(float time = 10.0f)
    {
        maxTime = time;
        remainingTimer = maxTime;
        gameObject.SetActive(true);
    }
}
