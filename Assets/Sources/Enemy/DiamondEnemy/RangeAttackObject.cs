/*
    Team    : Speaking Potato
    Author  : Sean Kim
    Date    : 10/17/2021
    Desc    : Range attack object for diamond unit.
*/
using UnityEngine;

public class RangeAttackObject : MonoBehaviour
{
    private float life = 3.0f;
    public int damage = 5;
    public float forwardForce = 32.0f;
    private Vector3 forward;

    private void Start()
    {
        forward = transform.forward;
        this.transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
    }

    private void FixedUpdate()
    {
        life -= Time.deltaTime;
        if (life <= 0.0f)
        {
            Destroy(gameObject);
        }
        else
        {
            GetComponent<Rigidbody>().AddForce(forward * forwardForce, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("HIT : " + other.gameObject.name);
        if (other.gameObject.CompareTag("Enemy") == true) return; // ignore when colliding with enemy

        if (other.gameObject.name == "Player")
        {
            Vector3 knockback = GetComponent<Rigidbody>().velocity.normalized * 10.0f;
            knockback.y = Mathf.Sqrt(0.5f * -2f * -9.81f);
            other.gameObject.GetComponent<Status>().OnTakeDamage(damage, knockback);
        }
        Destroy(gameObject);
    }
}

