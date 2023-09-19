using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LargestIndicator : MonoBehaviour
{
    TextMeshProUGUI text;
    float timer = .3f;
    float ogTimer = .3f;
    private bool blinkFlag = true;
    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        ogTimer = timer;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Color textColor = text.color;
            if (blinkFlag)
            {
                blinkFlag = false;
                text.color = Color.yellow;
            }
            else
            {
                blinkFlag = true;
                text.color = Color.black;
            }
            //if(textColor == Color.white)
            //{
            //    text.color = Color.yellow;
            //}
            //else if(textColor == Color.yellow)
            //{
            //    text.color = Color.blue;
            //}
            //else if(textColor == Color.blue)
            //{
            //    text.color = Color.red;
            //}
            //else if(textColor == Color.red)
            //{
            //    text.color = Color.black;
            //}
            //else if(textColor == Color.black)
            //{
            //    text.color = Color.white;
            //}
            timer = ogTimer;
        }       
    }
}
