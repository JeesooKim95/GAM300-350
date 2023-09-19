using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spade10EnemyTree : MonoBehaviour
{
    bool isGrowFinished = false;
    bool rotatingFinished = false;
    float speed = 70f;
    float timer = 0.2f;
    private Rigidbody rigidBody;
    private float rotationForce = 300f;
    private bool letsFollow = false;
    Vector3 playerPosition;
    public GameObject destroyParticle;
    public float waitTimer;


    private bool playSound = false;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        StartCoroutine(WaitBefore());
    }

    void FixedUpdate()
    {
        if(isGrowFinished)
        {
            if(letsFollow)
            {
                //Vector3 targetDirection = playerPosition - rigidBody.position;
                //targetDirection.Normalize();
                //Vector3 rotationAmount = Vector3.Cross(transform.forward, targetDirection);
                //rigidBody.angularVelocity = rotationAmount * rotationForce;
                //rigidBody.velocity = transform.forward * speed;
                //rigidBody.velocity = targetDirection * (speed * Time.deltaTime);

                float step =  speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, playerPosition, step);

                if (Vector3.Distance(transform.position, playerPosition) < 0.01f)
                {
                    Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f);
                    foreach (var hitCollider in hitColliders)
                    {
                        if(hitCollider.gameObject.tag == "Player")
                        {
                            Vector3 knockbackDir = hitCollider.gameObject.transform.position - transform.position;
                            hitCollider.gameObject.GetComponent<PlayerStatus>().OnTakeDamage(10, knockbackDir);
                        }
                    }
                    Destroy(gameObject);
                    GameObject particle = GameObject.Instantiate(destroyParticle, transform.position, Quaternion.identity);
                    Destroy(particle, 1f);
                }
            }
        }
        else
        {
            if(transform.localScale.x > 1f)
                isGrowFinished = true;
            else
                transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
        }

        if(isGrowFinished && letsFollow)
        {
            if(playSound == false)
            {
                playSound = true;

                PlayThrowSound();
            }
        }

    }

    private IEnumerator WaitBefore()
    {
        yield return new WaitForSeconds(waitTimer);
        
        letsFollow = true;
        playerPosition = GameObject.FindGameObjectWithTag("Player").gameObject.transform.position;
    }
    // void OnCollisionEnter(Collision collision)
    // {
    //     if(collision.gameObject.tag == "Player")
    //     {
    //         Destroy(gameObject);
    //         Vector3 knockbackDir = collision.gameObject.transform.position - transform.position;
    //         collision.gameObject.GetComponent<PlayerStatus>().OnTakeDamage(10, knockbackDir);
    //         GameObject particle = GameObject.Instantiate(destroyParticle, transform.position, Quaternion.identity);
    //         Destroy(particle, 1f);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //         GameObject particle = GameObject.Instantiate(destroyParticle, transform.position, Quaternion.identity);
    //         Destroy(particle, 1f);
    //     }
    // }

    // Sinil.kang - for sake of playing forest elites pattern sounds
    public void PlayThrowSound()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        
        audioManager.Play("ThrowBalls");
    }
}
