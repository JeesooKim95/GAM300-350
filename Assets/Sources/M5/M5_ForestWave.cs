using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class M5_ForestWave : MonoBehaviour
{
    public List<GameObject> Wave1;
    public List<GameObject> Wave2;
    public List<GameObject> Wave3;
    public List<Transform> spawnPoses;
    public float spawnPosesDiffs;

    public bool colliderCheck = false;
    private bool playerArrived = false;
    private bool spawnOnce = false;
    public List<GameObject> currentWave;
    private int currentWaveOriginalCount = 0;
    private MeshRenderer meshRenderer = null;
    public float minimumDistanceBetweenPlayer = 5f;
    public GameObject spawnParticle;

    public int totalEnemy;
    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        totalEnemy = 0;
    }
    bool CheckCurrentWaveValidate()
    {
        int count = 0;
        foreach (GameObject obj in currentWave)
        {
            //means validate
            if (obj == null)
                count++;

            if (count > (currentWaveOriginalCount / 2))
                return false;

            //if(obj != null)
            //    return true;
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerArrived)
        {
            if (Wave1 != null)
                SpawnEnemies(ref Wave1);
            else if (Wave2 != null)
                SpawnEnemies(ref Wave2);
            else if (Wave3 != null)
                SpawnEnemies(ref Wave3);
        }
    }
    private bool CheckCollidingWithPlayer(Vector3 pos)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, minimumDistanceBetweenPlayer);

        foreach (Collider col in colliders)
        {
            if (col.gameObject.CompareTag("Player"))
                return false;
        }
        return true;
    }

    private void SpawnEnemies(ref List<GameObject> objs)
    {
        if (objs != null)
        {
            if (!spawnOnce)
            {
                foreach (GameObject obj in objs)
                {
                    Vector3 randomPos;
                    do
                    {
                        randomPos = GetRandomSpawnPosBasedOnPosition();
                    } while (!CheckCollidingWithPlayer(randomPos));

                    GameObject enemy = GameObject.Instantiate(obj, randomPos, Quaternion.identity);
                    totalEnemy++;
                    if (enemy.name == "Spade10(Clone)")
                    {
                        enemy.name = "Boss";
                        enemy.GetComponent<Status>().SetProtection(true);
                    }
                    currentWave.Add(enemy);

                    if(spawnParticle)
                    {
                        GameObject particle = GameObject.Instantiate(spawnParticle, new Vector3(randomPos.x, 0f, randomPos.z), Quaternion.identity);
                        Destroy(particle, 1f);
                    }
                }
                currentWaveOriginalCount = currentWave.Count;
                spawnOnce = true;
            }
            if (!CheckCurrentWaveValidate())
            {
                currentWave.Clear();
                objs = null;
                spawnOnce = false;
                currentWaveOriginalCount = 0;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerArrived = true;
            colliderCheck = playerArrived;
            meshRenderer.enabled = false;
            FindObjectOfType<AudioManager>().Play("EnemySummon");
        }
    }
    Vector3 GetRandomSpawnPosBasedOnPosition()
    {
        int randomSpawnerPos = Random.Range(0, spawnPoses.Count);
        Transform spawnerPos = spawnPoses[randomSpawnerPos];

        float randomX = Random.Range(spawnerPos.position.x - spawnPosesDiffs, spawnerPos.position.x + spawnPosesDiffs);
        float y = spawnerPos.position.y;
        float randomZ = Random.Range(spawnerPos.position.z - spawnPosesDiffs, spawnerPos.position.z + spawnPosesDiffs);

        return new Vector3(randomX, y, randomZ);
    }
}
