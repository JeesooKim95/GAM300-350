/*
    Team    : Speaking Potato
    Author  : Sinil Kang
    Date    : 10/17/2021
    Desc    : Attack visual feedback will be destroyed automatically after a few seconds.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAttackArea : MonoBehaviour
{
    private float timer = 0;
    private float destroyTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
