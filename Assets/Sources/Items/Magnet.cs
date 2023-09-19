/*
    Team    : Speaking Potato
    Author  : Sinil Kang
    Date    : 10/5/2021
    Desc    : Item follows player in sphere collision space
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField]
    private float scaler;
    private Vector3 starting;
    public Vector3 destination;

    private float timer = -2;
    private Rigidbody mRigidBody;

    //Sangmin
    PlayerHandsManager handManager;

    // Start is called before the first frame update
    void Start()
    {
        starting = transform.position;
        destination = transform.position;
        mRigidBody = GetComponent<Rigidbody>();

        handManager = GameObject.FindObjectOfType<PlayerHandsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (timer < -1)
        {
            return;
        }
        else if (timer > 1)
        {
            starting = new Vector3(mRigidBody.position.x, transform.position.y, mRigidBody.position.z);
            timer = 0;
        }
        if(handManager.handsSize < 5)
        {
            //Debug.Log("asd");
            timer += Time.deltaTime * scaler;
            
            if (Mathf.Abs(destination.x - mRigidBody.position.x) < 1)
            {
                // Envoke item acquired function
                mRigidBody.MovePosition(destination);

                CardObject info = GetComponent<CardObject>();
                if(info != null)
                {
                    info.SetPlayerHand();
                }

                //Sean
                if(gameObject.tag == ("Key"))
                {
                    GameObject obj = GameObject.FindGameObjectWithTag("Portal");

                    if(obj != null)
                    {
                        Portal portal = obj.GetComponent<Portal>();

                        if(portal != null)
                        {
                            portal.Activate();
                            Debug.Log("Found Key!");
                        }
                    }
                }
                if(gameObject.tag == ("TutorialWeapon"))
                {
                    TutorialManager weapon = GameObject.FindGameObjectWithTag("Tutorial").GetComponent<TutorialManager>();
                    if(weapon != null)
                    {
                        //weapon.ActivateWeapon();
                    }                    
                    Debug.Log("Found Weapon!");
                }
                Destroy(gameObject);

                FindObjectOfType<AudioManager>().Play("ItemGet");
            }

            float t = easeOutCubic(timer);
            mRigidBody.MovePosition((1 - t) * starting + t * destination);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(handManager.handsSize < 5)
        {
            if (other.tag == "Player")
            {
                starting = new Vector3(mRigidBody.position.x, transform.position.y, mRigidBody.position.z);
                destination = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
                timer = 0;


                //Sangmin
                //22.01.15
                if(gameObject.tag == "Key")
                {
                    GameObject obstacle = GameObject.FindGameObjectWithTag("Obstacle");
                    Destroy(obstacle);
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(handManager.handsSize < 5)
        {
            if (other.tag == "Player")
            {
                starting = new Vector3(mRigidBody.position.x, transform.position.y, mRigidBody.position.z);
                destination = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
                timer = 0;
            }
        }
    }

    float easeOutCubic(float x)
    {
        return 1 - Mathf.Pow(1 - x, 3);
    }

    float easeOutBack(float x)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;

        return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
    }
}
