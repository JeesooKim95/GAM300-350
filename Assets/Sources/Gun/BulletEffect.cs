using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    public float liveTime = 0.05f;
    private bool isIntiatedStain = false;
    private float timer = 0.0f;
    public GameObject stain;
    void Start()
    {
    }
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 1.0f)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!isIntiatedStain)
        {
            GameObject obj = collision.gameObject;
            if(obj != null)
            {
                if(!obj.CompareTag("Bullet"))
                {
                    if(obj.CompareTag("Wall"))
                    {
                        Vector3 rotate = obj.transform.eulerAngles;

                        if(rotate.y == 90.0f)
                        {
                            if(stain != null)
                            {
                                GameObject stainObj = Instantiate(stain, transform.position, Quaternion.identity);
                                isIntiatedStain = true;
                                Destroy(gameObject);
                            }
                        }
                        else if(rotate.y == 0.0f)
                        {
                            if(stain != null)
                            {
                                Vector3 rotVec = new Vector3(0, 90, 0);
                                //Quaternion rot = Quaternion.LookRotation(rotVec);
                                GameObject stainObj = Instantiate(stain, transform.position, Quaternion.identity);
                                stainObj.transform.Rotate(rotVec);
                                isIntiatedStain = true;
                                Destroy(gameObject);
                            }
                        }
                    }
                    else if(obj.CompareTag("Ground"))
                    {
                        if(stain != null)
                        {
                            Vector3 rotVec = new Vector3(0, 0, 90);
                            GameObject stainObj = Instantiate(stain, transform.position, Quaternion.identity);
                            stainObj.transform.Rotate(rotVec);
                            isIntiatedStain = true;
                            Destroy(gameObject);
                        }
                    }

                }
            }

        }
    }


}