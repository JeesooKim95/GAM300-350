using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DoorAnimController))]
public class EntranceDoorContoller : MonoBehaviour
{
    private Transform playerTransform;
    private DoorAnimController doorAnimController;
    private bool isTriggered = false; // one-time action

    public float distanceToOpen;
    public float distanceToClose;


    // Sinil - audio manager
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        doorAnimController = GetComponent<DoorAnimController>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered == false)
        {
            if (doorAnimController.isDoorOpened == true)
            {
                // player passed the door and moved enough distance - close
                if (Vector3.Dot(this.transform.forward, playerTransform.position - this.transform.position) < 0.0f)
                {
                    if (Vector3.Distance(this.transform.position, playerTransform.position) > distanceToClose)
                    {
                        audioManager.Play("ElevatorClosed");
                        doorAnimController.OnActiveDoor();
                        isTriggered = true;
                    }
                }
            }
            else
            {
                if (Vector3.Distance(this.transform.position, playerTransform.position) < distanceToOpen)
                {
                    audioManager.Play("ElevatorOpened");
                    doorAnimController.OnActiveDoor();
                }
            }
        }
    }

    public bool IsEntranceClosed()
    {
        return isTriggered;
    }
}
