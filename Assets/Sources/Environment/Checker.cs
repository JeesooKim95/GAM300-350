using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    public GameObject spawner3;

    public GameObject teleport;

    // Start is called before the first frame update
    void Start()
    {
        spawner3 = GameObject.Find("Last Indicator");
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0
           && spawner3.GetComponent<M5_Snowfiled>().triggerCheck == true)
            teleport.SetActive(true);
        else
        {
            teleport.SetActive(false);
        }

    }
}
