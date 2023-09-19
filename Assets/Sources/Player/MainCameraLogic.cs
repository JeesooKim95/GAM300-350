/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                9/18/2021
 *  Contributor:       Sinil Kang
 *  Description:       Main logic for camera movement.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainCameraLogic : MonoBehaviour
{
    [SerializeField]
    public Vector3 FirstPersonPosition;
    [SerializeField]
    public Quaternion FirstPersonRotation;

    [SerializeField]
    public Vector3 ThirdPersonPosition;
    [SerializeField]
    public Quaternion ThirdPersonRotation;
    // Start is called before the first frame update
    private bool isFPS = true;
    private Vector3 DeltaPosition;
    private Quaternion DeltaRotation;
    private Vector3 eular;

    [SerializeField]
    private float rotationSpeed = 1.0f;
    private float YRotation = 0f;

    Transform transf;

    // Haewon Shon, 10/21/2021 for shooting direction purpose
    public float yDegree;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        DeltaPosition = ThirdPersonPosition - FirstPersonPosition;


        FirstPersonRotation.eulerAngles = new Vector3(FirstPersonRotation.x, FirstPersonRotation.y, FirstPersonRotation.z);
        ThirdPersonRotation.eulerAngles = new Vector3(ThirdPersonRotation.x, ThirdPersonRotation.y, ThirdPersonRotation.z);
        eular = ThirdPersonRotation.eulerAngles - FirstPersonRotation.eulerAngles;
        DeltaRotation = new Quaternion(eular.x, eular.y, eular.z, 0);

        transf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        bool pauseStatus = PauseMenu.GameIsPaused;
        
        if( pauseStatus == false)
        {
            float y = Input.GetAxis("Mouse Y") * rotationSpeed;
            Vector3 newRotation = GetComponent<Transform>().localRotation.eulerAngles + new Vector3(-y, 0f, 0f);
            if (newRotation.x > 90f && newRotation.x < 180f)
            {
                newRotation.x = 90f;
            }
            else if (newRotation.x < 270f && newRotation.x > 180f)
            {
                newRotation.x = 270f;
            }
            newRotation.y = 0.0f;
            newRotation.z = 0.0f;

            yDegree = newRotation.x;

            transf.localRotation = Quaternion.Euler(newRotation);
        }
        
    }

    void ChangeCameraPosition(bool isFPS)
    {
        if(isFPS)
        {
            transf.localPosition -= DeltaPosition;
            transf.localRotation = Quaternion.Euler(GetComponent<Transform>().localRotation.eulerAngles - eular);
        }
        else
        {
            transf.localPosition += DeltaPosition;
            transf.localRotation = Quaternion.Euler(GetComponent<Transform>().localRotation.eulerAngles + eular);
        }
    }
}
