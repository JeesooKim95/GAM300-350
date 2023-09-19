/*
    Team    : Speaking Potato
    Author  : Sinil Kang
    Date    : 10/5/2021
    Desc    : Item floating itself
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    [SerializeField]
    private float verticalRangeScaler;

    [SerializeField]
    private float floatingScaler;

    // Start is called before the first frame update
    private float startY;
    void Start()
    {
        startY = transform.localPosition.y + (verticalRangeScaler);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, startY + verticalRangeScaler*(easeOutSine(Time.time * floatingScaler)), transform.localPosition.z);
    }

    float easeOutSine(float x)
    {
        return Mathf.Sin((x* Mathf.PI) / 2);
    }
}
