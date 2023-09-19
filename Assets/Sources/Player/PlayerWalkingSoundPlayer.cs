using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkingSoundPlayer : MonoBehaviour
{
    [SerializeField]
    public float idealPlaySpeed = 0.01f;

    private float walkLocalTimer;
    PlayerMovement playerMovement;

    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        walkLocalTimer = 0;
        playerMovement = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playerMovement.ShouldPlayWalkingSound() == false)
        {
            return;
        }

        walkLocalTimer += Time.deltaTime * playerMovement.speed * idealPlaySpeed;

        if(walkLocalTimer >= 1f)
        {
            walkLocalTimer = walkLocalTimer % 1f;

            audioManager.PlayWalk();
        }
    }
}
