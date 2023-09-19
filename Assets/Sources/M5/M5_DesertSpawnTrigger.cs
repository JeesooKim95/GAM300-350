/*  Class:               GAM350
 *  Team name:      Speaking Potato
 *  Date:                02/17/2022
 *  Contributor:       Haewon Shon
 *  Description:       Simple spawn trigger for desert level
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M5_DesertSpawnTrigger : MonoBehaviour
{
    public M5_DesertSceneManager manager;

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            manager.RunNextStage();
            this.gameObject.SetActive(false);
            FindObjectOfType<AudioManager>().Play("EnemySummon");
        }
    }
}
