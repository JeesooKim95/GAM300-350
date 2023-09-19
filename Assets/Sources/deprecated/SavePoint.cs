/*
    Team    : Speaking Potato
    Author  : Jina Hyun
    Date    : 09/26/2021
    Desc    : Save point collsion trigger
*/
using UnityEngine;
public class SavePoint : MonoBehaviour
{
    // Detect collision trigger event
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            PlayerRespawn respawn = collider.gameObject.GetComponent<PlayerRespawn>();
            if(respawn)
            {
                respawn.SetSavePoint(gameObject.transform.position);
            }
        }
    }
}
