/*
    Team    : Speaking Potato
    Author  : Sinil Kang
    Date    : 10/5/2021
    Desc    : Item rotating itself
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticRotation : MonoBehaviour
{
    [SerializeField]
    private float turnSpeed = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(Time.deltaTime * turnSpeed * Vector3.up, Space.World);
    }
}
