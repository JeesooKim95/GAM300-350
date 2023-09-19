using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObject : MonoBehaviour
{
    private float life = 1.0f;

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
        //Destroy(gameObject);
    }
}
