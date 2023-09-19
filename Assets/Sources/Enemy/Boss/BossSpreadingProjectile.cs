/*
    Team    : Speaking Potato
    Author  : Sean Kim
    Date    : 10/24/2021
    Desc    : Spreading projectiles for boss unit range attack.
*/
using UnityEngine;

public class BossSpreadingProjectile : MonoBehaviour
{
    private float life = 2.5f;
    public int damage = 5;
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
            other.gameObject.GetComponent<Status>().OnTakeDamage(damage, knockback);
        }
        Destroy(gameObject);
    }
}
