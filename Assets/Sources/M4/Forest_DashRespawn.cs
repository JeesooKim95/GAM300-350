/*
    sangmin.kim
    2021/11/16
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_DashRespawn : MonoBehaviour
{
    public GameObject respawnPoint;

    AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CharacterController cc = other.gameObject.GetComponent<CharacterController>();

            cc.enabled = false;
            cc.transform.position 
            = respawnPoint.gameObject.transform.position;
            cc.enabled = true;

            audioManager.Play("PlayerGetDamaged");
        }
    }
}
