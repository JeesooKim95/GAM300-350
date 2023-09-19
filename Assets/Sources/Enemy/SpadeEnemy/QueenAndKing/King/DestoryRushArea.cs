using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryRushArea : MonoBehaviour
{
    // Start is called before the first frame update
    private float timer;
    private float destroyTime;
    void Start()
    {
        timer = 0;
        destroyTime = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
