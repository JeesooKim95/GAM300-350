using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBullet_Straight : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
                
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Elite"))
        {
            EnemyBase enemyInfo = other.gameObject.GetComponent<EnemyBase>();
            Status enemyStatus = other.gameObject.GetComponent<Status>();

            enemyInfo.Freeze();
            enemyStatus.OnTakeDamage(10, new Vector3(0.0f, 10.0f, 0.0f));


            // Sinil - for sake of playing bullet sounds
            FindObjectOfType<AudioManager>().PlaySpatial("EnemyGetIceDamage", gameObject.transform.position);
        }

        Destroy(gameObject);
    }
}
