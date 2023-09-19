/*
    Team    : Speaking Potato
    Author  : Sean Kim
    Date    : 10 / 26 / 2021
    Desc: Obstacle that moves around and damage player in boss scene.
*/
using UnityEngine;

public class BossObstacle : MonoBehaviour
{
    public int damage = 5;
    public float speed = 500.0f;
    public float distance = 100.0f;
    public bool moveX = false;
    public bool moveZ = false;

    private Vector3 start;
    private Rigidbody rb;
    private bool direction;
    private bool reverse;

    void Start()
    {
        direction = true;
        reverse = false;
        rb = gameObject.GetComponent<Rigidbody>();
        start = gameObject.transform.position;
    }

    void Update()
    {
        if(reverse)
        {
            if (moveX)
            {
                transform.position = new Vector3(start.x - Mathf.PingPong(Time.time * speed, distance), transform.position.y, transform.position.z);
            }
            if (moveZ)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, start.z - Mathf.PingPong(Time.time * speed, distance));
            }
        }
        else
        {
            if (moveX)
            {
                transform.position = new Vector3(start.x + Mathf.PingPong(Time.time * speed, distance), transform.position.y, transform.position.z);
            }
            if (moveZ)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, start.z + Mathf.PingPong(Time.time * speed, distance));
            }
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

    }
}
