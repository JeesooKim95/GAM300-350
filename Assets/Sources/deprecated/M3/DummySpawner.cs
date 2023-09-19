using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySpawner : MonoBehaviour
{
    [Header("Game Object")]
    public GameObject prefab = null;
    public GameObject prefabInWorld = null;
    public Transform parent = null;

    [Header("Spawn")]
    public float spawnInterval = 2;

    private Vector3 position;
    private Quaternion rotation;
    private GameObject obj = null;
    private float time = 0;
    private bool waitForSpawn = false;

    void Start()
    {
        obj = prefabInWorld;
        position = prefabInWorld.transform.position;
        rotation = prefabInWorld.transform.rotation;
    }

    public void OnDeath()
    {
        waitForSpawn = true;
    }

    void Update()
    {
        if(waitForSpawn)
        {
            time += Time.deltaTime;
            if(time >=  spawnInterval)
            {
                obj = Instantiate(prefab, position, rotation, parent);
                obj.GetComponent<DummyStatus>().SetSpawner(gameObject);
                waitForSpawn = false;
                time = 0;
            }
        }
    }
}
