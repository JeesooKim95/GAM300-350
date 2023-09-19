using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStain : MonoBehaviour
{
     public float liveTime = 0.05f;
    void Start()
    {
        Invoke("Delay", liveTime);
    }
    private void Delay()
    {
        Destroy(gameObject);
    }

}
