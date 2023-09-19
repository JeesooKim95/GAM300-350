
using UnityEngine;

public class CustomBullet : MonoBehaviour
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
    float timer = 5.0f;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if(timer < 0.0f)
        {
            Destroy(gameObject);
        }
        else
        {
            timer -= Time.deltaTime;
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
            enemies[i].GetComponent<Status>().OnTakeDamage(explosionDamage, transform.position);
        }
        //Add delay 
        Invoke("Delay", 0.01f);
    }

    private void OnTriggerEnter(Collider collider)
    {        
        //Explode if it hits enemy directly
        bool collidedWithEnemy = collider.gameObject.CompareTag("Enemy") | collider.gameObject.CompareTag("Elite");
        bool collidedWithElite = collider.gameObject.CompareTag("Elite");
        bool collidedWithWall = collider.gameObject.CompareTag("Wall");
        bool collidedWithGround = collider.gameObject.CompareTag("Ground");

        // 04/13 Haewon, to remove bullet when colliding with elite enemy's barrier
        bool collidedWithBarrier = collider.gameObject.CompareTag("EliteBarrier");

        if(!collider.gameObject.CompareTag("Bullet"))
        {
            if(collidedWithEnemy || collidedWithWall || collidedWithGround || collidedWithElite || collidedWithBarrier)
            {
                if (collidedWithEnemy || collidedWithElite)
                {
                    // Sinil - for sake of playing audio
                    FindObjectOfType<AudioManager>().PlaySpatial("EnemyGetWaterDamage", gameObject.transform.position);

                    collider.gameObject.GetComponent<Status>().OnTakeDamage(explosionDamage, transform.position);
                }

                if(effectObj != null)
                {
                    if(!isInstantiatedEffect)
                    {
                        for(int i = 0; i < numEffects; ++i)
                        {
                            GameObject obj = Instantiate(effectObj, transform.position, Quaternion.identity);
                            Destroy(obj, 0.3f);
                        }
                        isInstantiatedEffect = true;
                    }

                }

                //Invoke("Delay", 0.02f);
                Destroy(gameObject);
            }
        }
    }

    private void Delay()
    {
        Destroy(gameObject);
        //Debug.Log("ENEMY");
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