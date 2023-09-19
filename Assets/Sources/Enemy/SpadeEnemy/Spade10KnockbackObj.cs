using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spade10KnockbackObj : MonoBehaviour
{
    private float timer;
    private GameObject player;
    private GameObject boss;
    public GameObject explosion;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boss = GameObject.FindObjectOfType<Spade10Enemy>().gameObject;
        timer = 3f;
    }

    void Update()
    {
        if(boss)
        {
            Vector3 bossPos = boss.transform.position;
            transform.position = new Vector3(bossPos.x, 0.1f, bossPos.z);

            if(timer > 0f)
                timer -= Time.deltaTime;
            else
            {
                FindObjectOfType<AudioManager>().Play("Explosion");

                float distanceBtwPlayer = Vector3.Distance(gameObject.transform.position, player.transform.position);

                if(distanceBtwPlayer < 10f)
                {
                    Vector3 toPlayer = player.transform.position - transform.position;
                    toPlayer = toPlayer.normalized;
                    float pushForce = 10f;

                    PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
                    playerStatus.OnTakeDamage(10, toPlayer * pushForce);
                }

                GameObject explosionObj = GameObject.Instantiate(explosion, transform.position, Quaternion.identity);

                explosionObj.transform.localScale = new Vector3(3f, 3f, 3f);

                Destroy(explosionObj, 1f);

                Destroy(gameObject);
            }
        }
        else
            Destroy(gameObject);

    }

    
}
