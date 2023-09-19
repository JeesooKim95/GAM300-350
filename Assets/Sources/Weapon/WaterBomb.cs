/*
    Team    : Speaking Potato
    Author  : Jeesoo Kim
    Date    : 10/1/2021
    Desc    : Water Bomb
*/
using UnityEngine;

public class WaterBomb : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

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

    int collisions;
    PhysicMaterial physicsMat;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (collisions > maxCollision)
        {
            Explode();
        }

        //count down bullet lifetime
        maxLifeTime -= Time.deltaTime;
        if (maxLifeTime <= 0)
        {
            Explode();
        }
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
            enemies[i].GetComponent<Status>().OnTakeDamage(explosionDamage, Vector3.zero);
        }
        //Add delay 
        Invoke("Delay", 0.01f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisions++;

        //Explode if it hits enemy directly
        if (collision.collider.CompareTag("Enemy"))
        {
            if (explodeOnTouch)
            {
                Explode();
            }
        }
        //else
        //{
        //    Destroy(gameObject);
        //    Debug.Log("NOT ENEMY");
        //}
    }

    private void Delay()
    {
        Destroy(gameObject);
        Debug.Log("ENEMY");
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