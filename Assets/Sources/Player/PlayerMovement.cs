/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                10/17/2021
 *  Contributor:       Su Kim
 *  Description:      Main logic for MOVEMENT of player character.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.75f;

    public Transform groundCheck;
    public Transform ObstacleCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Vector3 move;

    Vector3 velocity;
    bool isGrounded;
    bool isStuckInObstacle;

    public Vector3 inputVector = Vector3.zero;

    private bool isKnockbacking = false;
    private float knockbackTimer = 0.0f;

    // Sinil.Kang for purpose of playing walking sound
    private bool shouldPlayWalking;
     
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isStuckInObstacle = Physics.CheckSphere(ObstacleCheck.position, groundDistance, groundMask);
        if (isKnockbacking == true)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer < 0.0f)
            {
                isKnockbacking = false;
                velocity = Vector3.zero;
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
                controller.Move(velocity * Time.deltaTime);
                return;
            }
        }

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        inputVector.x = x;
        inputVector.z = z;
        UpdateWalkingSoundFlag(x, z, isGrounded);

        if(isStuckInObstacle)
        {
            move = transform.right * x;
        }
        else
        {
            move = transform.right * x + transform.forward * z;
        }

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            FindObjectOfType<AudioManager>().Play("Jump");
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    // haewon shon, 10/21/2021
    public void Knockback(Vector3 knockbackVelocity, float timer)
    {
        velocity = knockbackVelocity;
        isKnockbacking = true;
        knockbackTimer = timer;
    }

    public void InitPosition(Vector3 position)
    {
        controller.enabled = false;
        controller.transform.position = position;
        controller.enabled = true;
    }

    // haewon shon, 02/03/2022, to init position/rotation together
    public void InitTransform(Transform transform)
    {
        controller.enabled = false;
        controller.transform.position = transform.position;
        controller.transform.rotation = transform.rotation;
        controller.enabled = true;
    }

    // Sinil.Kang walking sound purpose
    public bool ShouldPlayWalkingSound()
    {
        return shouldPlayWalking;
    }
    // Sinil.Kang walking sound purpose
    public void UpdateWalkingSoundFlag(float x, float z, bool isGround)
    {
        int ix = (int)x;
        int iz = (int)z;
        shouldPlayWalking = isGround && ((ix != 0) || (iz != 0));
    }
}