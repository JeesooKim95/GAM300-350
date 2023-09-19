/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                10/27/2021
 *  Contributor:       Su Kim
 *  Description:      Main logic for Dash of movement of player character.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] ParticleSystem forwardDashParticle;
    [SerializeField] ParticleSystem leftDashParticle;
    [SerializeField] ParticleSystem rightDashParticle;
    [SerializeField] ParticleSystem backDashParticle;

    private PlayerMovement playerMovement;

    public float dashSpeed;
    public float dashTime;

    public float cooldownTime = 1.0f;
    private float nextDashTime = 0.0f;

    public bool isDashing = false;

    // Sinil - for sake of playing dash sounds
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextDashTime)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isDashing = true;
                // Sinil - for sake of playing dash sounds
                audioManager.PlayDash();
                StartCoroutine(Dash());
                nextDashTime = Time.time + cooldownTime;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                audioManager.Play("PlayerImpossible");
            }
        }
        
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        PlayParticle();
        while (Time.time < startTime + dashTime)
        {
            playerMovement.controller.Move(playerMovement.move * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }


    public void PlayParticle()
    {
        Vector3 inputVector = playerMovement.inputVector;
        //Forward Dash
        if (inputVector.z > 0 && Mathf.Abs(inputVector.x) <= inputVector.z)
        {
            forwardDashParticle.Play();
            return;
        }
        //Back Dash
        if (inputVector.z < 0 && Mathf.Abs(inputVector.x) <= Mathf.Abs(inputVector.z))
        {
            backDashParticle.Play();
            return;
        }
        //Right Dash
        if (inputVector.x > 0)
        {
            rightDashParticle.Play();
            return;
        }
        //Left Dash
        if (inputVector.x < 0)
        {
            leftDashParticle.Play();
            return;
        }
        //in case of default
        forwardDashParticle.Play();
    }
}
