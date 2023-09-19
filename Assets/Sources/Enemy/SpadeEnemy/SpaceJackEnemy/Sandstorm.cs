/*
    Team    : Speaking Potato
    Author  : Sean Kim
    Date    : 10/17/2021
    Desc    : Range attack object for diamond unit.
*/
using UnityEngine;

public class Sandstorm : MonoBehaviour
{
    public float growTime = 2.0f;
    public float lifeTime = 10.0f;
    public int damage = 10;
    public Vector3 velocity;
    public float velocityScale;

    private void Start()
    {}

    private void FixedUpdate()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0.0f) Destroy(gameObject);
        if (growTime >= 0.0f)
        {
            growTime -= Time.deltaTime;
        }
        else
        {
            this.transform.Translate(velocity * velocityScale * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("HIT : " + other.gameObject.name);
        if (other.gameObject.CompareTag("Enemy") == true
            || other.gameObject.CompareTag("EnemyBullet") == true) return; // ignore when colliding with enemy

        if (other.gameObject.name == "Player")
        {
            Vector3 knockback = velocity * 10.0f;
            knockback.y = Mathf.Sqrt(0.5f * -2f * -9.81f);
            other.gameObject.GetComponent<Status>().OnTakeDamage(damage, knockback);
        }
    }
}
