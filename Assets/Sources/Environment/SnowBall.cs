/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                01/25/2022
 *  Contributor:       Su Kim
 *  Description:      Temp object for attack motion of club enemy
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Vector3 knockback = (other.gameObject.transform.position - gameObject.transform.position).normalized * 20.0f;
            knockback.y = Mathf.Sqrt(0.5f * -2f * -9.18f);
            other.gameObject.GetComponent<PlayerStatus>().OnTakeDamage(10, knockback);
            Destroy(gameObject);
        }
        else if (other.tag == "Barricade" || other.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }

}
