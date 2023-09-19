/*
    Team    : Speaking Potato
    Author  : Sangmin Kim
    Date    : 10/23/2021
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBullet_FourCard : MonoBehaviour
{
   public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;
    public GameObject effectObj;

    //stats
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    //Damage
    public int explosionDamage;
    public float explosionRange;

    //Lifetime
    public int maxCollision;
    public float maxLifeTime;
    public bool explodeOnTouch = false;
    public float effectForce = 2.0f;
    private bool isInstantiatedEffect = false;
    public int numEffects = 4;
    int collisions;
    PhysicMaterial physicsMat;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
    }

    private void Explode()
    {
        //Instantiate explosion
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }

        //check for enemy
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            //Get enemy comp and call damage function
            //enemies[i].GetComponent<Status>().OnTakeDamage(explosionDamage);
            enemies[i].GetComponent<Status>().OnTakeDamage(explosionDamage, transform.position);
        }
        //Add delay 
        Invoke("Delay", 0.01f);
    }

    private void OnCollisionEnter(Collision collision)
    {        
        //Explode if it hits enemy directly

        if(!collision.gameObject.CompareTag("Bullet"))
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Status>().OnTakeDamage(explosionDamage, transform.position);
            }

            if(effectObj != null)
            {
                if(!isInstantiatedEffect)
                {
                    for(int i = 0; i < numEffects; ++i)
                    {
                        GameObject obj = Instantiate(effectObj, transform.position, Quaternion.identity);
                        Rigidbody objRb = obj.GetComponent<Rigidbody>();
                        Vector3 randomVec;

                        randomVec.x = Random.Range(-effectForce, effectForce);
                        randomVec.y = Random.Range(-effectForce, effectForce);
                        randomVec.z = Random.Range(-effectForce, effectForce);
                        
                        objRb.AddForce(randomVec, ForceMode.Impulse);
                        obj.transform.rotation = Quaternion.LookRotation(randomVec);
                    }
                    isInstantiatedEffect = true;
                }

            }
            Explode();
            //Invoke("Delay", 0.02f);

            // Sinil - for sake of playing bullet sounds
            FindObjectOfType<AudioManager>().PlaySpatial("EnemyGetExplosion", gameObject.transform.position);
        }

    }

    private void Delay()
    {
        Destroy(gameObject);
    }

    private void Init()
    {
        //Create Physics Material
        physicsMat = new PhysicMaterial();
        physicsMat.bounciness = bounciness;
        physicsMat.frictionCombine = PhysicMaterialCombine.Minimum;
        physicsMat.bounceCombine = PhysicMaterialCombine.Maximum;

        //Assign material to collider
        GetComponent<SphereCollider>().material = physicsMat;

        //Set gravity
        rb.useGravity = useGravity;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
