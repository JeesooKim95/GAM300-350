/*
    Team    : Speaking Potato
    Author  : Su Kim
    Date    : 09/17/2021
    Desc    : Basic watergun system
*/

using UnityEngine;
using System.Collections;

public class gunSystem : MonoBehaviour
{
    public int damage = 1;
    public float range = 100;
    public float impactForce = 20f;
    public float fireRate = 40f;

    public Camera fpsCam;
    public ParticleSystem waterGunParticle;
    public GameObject ImpactEffect;


    private float nextTimeToFire = 0f;


    void Update()
    {
        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
          nextTimeToFire = Time.time + 1f / fireRate;
          Shoot();
        }


    }

    void Shoot()
    {
        waterGunParticle.Play();
        StartCoroutine(StopParticleSystem(waterGunParticle, 0.2f));

        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Status target = hit.transform.GetComponent<Status>();
            if(target != null)
            {
                target.OnTakeDamage(damage, Vector3.zero);
            }

            if(hit.rigidbody != null)
            {
              hit.rigidbody.AddForce(-hit.normal * impactForce);
            }


            GameObject impactGO = Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
        }
        //Sound
        FindObjectOfType<AudioManager>().Play("BasicWaterGunShoot");
    }

    IEnumerator StopParticleSystem(ParticleSystem particleSystem, float time)
    {
        yield return new WaitForSeconds(time);
        particleSystem.Stop();
    }
}
