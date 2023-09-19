/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                10/17/2021
 *  Contributor:       Su Kim
 *  Description:      Temp object for attack motion of club enemy
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClubEnemyAttackObject : MonoBehaviour
{
    private float life = 0.5f;

    private void FixedUpdate()
    {
        life -= Time.deltaTime;
        if (life <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Vector3 knockback = (other.gameObject.transform.position - gameObject.transform.position).normalized * 20.0f;
            knockback.y = Mathf.Sqrt(0.5f * -2f * -9.81f);
            other.gameObject.GetComponent<PlayerStatus>().OnTakeDamage(10, knockback);
        }
    }
}