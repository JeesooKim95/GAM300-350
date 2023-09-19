using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    private GameObject spawnedObject = null;
    
    // Update is called once per frame
    void Update()
    {
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        }
    }
}
