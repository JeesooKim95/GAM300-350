using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlizzardSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject FlyingObject;
    public int amount;
    public float destroyDelay;
    public float spawnInterval;
    public float spawnRadius;
    public float spawnForce;
    public Vector3 spawnOffset;

    private bool isDead = false;
    private float spawnIntervalTimer;


    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (spawnIntervalTimer <= 0)
            {
                spawnIntervalTimer = spawnInterval;

                amount -= 1;

                var spawnPosition = transform.position + new Vector3(Random.insideUnitCircle.x * spawnRadius,
                    0,
                    Random.insideUnitCircle.y * spawnRadius) + spawnOffset;

                var obj = Instantiate(FlyingObject, spawnPosition, Quaternion.identity);
                obj.transform.SetParent(transform);

                var forceDirection = transform.position - (transform.position + spawnOffset);
                obj.GetComponent<Rigidbody>().AddForce(forceDirection * spawnForce, ForceMode.VelocityChange);

                Destroy(obj, destroyDelay);

                if (amount <= 0)
                {
                    isDead = true;
                    Destroy(gameObject, destroyDelay);
                }
            }
            else
            {
                spawnIntervalTimer -= Time.deltaTime;
            }
        }
    }
}
