/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                2/2/2022
 *  Contributor:       Sinil Kang
 *  Description:       Script to Activate portal
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalActivator : MonoBehaviour
{
    public Portal p;

    // Start is called before the first frame update
    void Start()
    {
        p.Activate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
