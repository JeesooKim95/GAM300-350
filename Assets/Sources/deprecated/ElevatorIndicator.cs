/*
    Team    : Speaking Potato
    Author  : Jina Hyun
    Date    : 10/24/2021
    Desc    : Elevator indicator 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ElevatorIndicator : MonoBehaviour
{
    private TextMeshPro text = null;
    public void Init()
    {
        text = gameObject.transform.Find("Indicator").Find("Text").GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        Init();
    }

    public void SetText(string str)
    {
        text.text = str;
    }
}
