/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                02/03/2022
 *  Contributor:       Su Kim
 *  Description:       Teleport for player.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{

    public Transform teleportPosition;
    public GameObject elite1;
    public GameObject elite2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().InitPosition(teleportPosition.transform.position);

            elite1.SetActive(true);
            elite2.SetActive(true);
        }
    } 
}
