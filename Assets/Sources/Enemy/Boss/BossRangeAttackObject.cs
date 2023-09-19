/*
    Team    : Speaking Potato
    Author  : Sean Kim
    Date    : 10/23/2021
    Desc    : Boss range attack object that shoots a large projectile 
                then spreads little projectiles around in circle
*/
using System.Collections.Generic;
using UnityEngine;

public class BossRangeAttackObject : MonoBehaviour
{
    public int damage = 10;
    public GameObject secondProjectile;
    public GameObject player;
    public int secondProjectileNum = 8;
    public float forwardForce = 10.0f;
    public float upForce = -10.0f;

    private float shotTime = 0.0f;
    private float life = 10.0f;
    private bool isReadyForSpread = true;
    private List<GameObject> projectileContainer;
    private float angleOfSpread = 0.0f;

    // Sinil - for sake of audio
    private AudioManager audioManager;
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        projectileContainer = new List<GameObject>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void FixedUpdate()
    {
        life -= Time.deltaTime;
        if (life <= 0.0f)
        {
            Destroy(gameObject);
        }
        Vector3 objectPos = gameObject.transform.position;
        float distance = Vector3.Distance(objectPos, player.transform.position);
        if (distance < 30.0f)
        {
            if (isReadyForSpread)
            {
                audioManager.Play("BossCannonSmallAttack");

                for (int i = 1; i <= secondProjectileNum; i++)
                {
                    float radians = 2 * Mathf.PI / secondProjectileNum * i;
                    float xPos = Mathf.Sin(radians);
                    float zPos = Mathf.Cos(radians);

                    Vector3 direction = new Vector3(xPos, 0.0f, zPos);
                    GameObject projectile = Instantiate(secondProjectile, direction + objectPos, Quaternion.AngleAxis(angleOfSpread, Vector3.up));
                    projectileContainer.Add(projectile);
                    Rigidbody rb = projectile.GetComponent<Rigidbody>();
                    rb.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * forwardForce, ForceMode.Impulse);
                    rb.GetComponent<Rigidbody>().AddForce(projectile.transform.up * upForce, ForceMode.Impulse);

                    shotTime = Time.time;
                    isReadyForSpread = false;
                    angleOfSpread += 360 / secondProjectileNum;
                }
            }
        }
        //if(projectileContainer.Count == secondProjectileNum)
        //{
        //    foreach(GameObject obj in projectileContainer)
        //    {
        //        Rigidbody rb = obj.GetComponent<Rigidbody>();
        //        rb.gameObject.GetComponent<Rigidbody>().AddForce(obj.transform.forward * forwardForce, ForceMode.Impulse);
        //        rb.gameObject.GetComponent<Rigidbody>().AddForce(obj.transform.up * upForce, ForceMode.Impulse);
        //    }
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Vector3 knockback = (other.gameObject.transform.position - gameObject.transform.position).normalized * 20.0f;
            knockback.y = Mathf.Sqrt(0.5f * -2f * -9.81f);
            other.gameObject.GetComponent<Status>().OnTakeDamage(damage, knockback);
        }
        if (other.gameObject.tag != "Ground")
        {
            Destroy(gameObject);
        }

    }
}
