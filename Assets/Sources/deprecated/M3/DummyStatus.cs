using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyStatus : Status
{
    public GameObject spawner = null;
    public bool connectToLevelManager = false;

    public void SetSpawner(GameObject dummy_spawner)
    {
        spawner = dummy_spawner;
    }

    public override void OnDeath()
    {
        if (gameObject.tag == "Enemy")
        {
            if(spawner)
                spawner.GetComponent<DummySpawner>().OnDeath();
            EnemyBase eb= gameObject.GetComponent<EnemyBase>();
            eb.ThrowItem();
            GameObject.Find("PlayerHandsManager").GetComponent<PlayerHandsManager>().Add(new Card(eb.enemyType, eb.cardValue));

            if(connectToLevelManager == true)
                GameObject.Find("LevelManager").GetComponent<LevelManager>().IncreaseKilledEnemyNum();

            Destroy(gameObject);
        }
    }
}
