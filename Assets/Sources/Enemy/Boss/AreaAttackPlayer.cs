/*
    Team    : Speaking Potato
    Author  : Sinil Kang
    Date    : 10/17/2021
    Desc    : AttackArea detect player and give damage automatically.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttackPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Give Damage
            Vector3 knockback = (other.gameObject.transform.localPosition - gameObject.transform.localPosition).normalized * 20.0f;
            knockback.y = Mathf.Sqrt(0.5f * -2f * -9.81f);
            other.gameObject.GetComponent<Status>().OnTakeDamage(8, knockback);
        }
    }
}
