/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                9/18/2021
 *  Contributor:       Sinil Kang
 *  Description:       Main logic for ROTATION of player character.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerRotation : MonoBehaviour
{
    [SerializeField]
    private float turnSpeed = 3;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool pauseStatus = PauseMenu.GameIsPaused;
        if (pauseStatus == false)
        {
            var horizontal = Input.GetAxis("Mouse X");
            transform.Rotate(horizontal * turnSpeed * Vector3.up, Space.World);
        }

    }
}
