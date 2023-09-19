/*  Class:               GAM350
 *  Team name:      Speaking Potato
 *  Date:                2/3/2022
 *  Contributor:       Haewon Shon
 *  Description:       Temporary script for instant level change. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempLevelMoveScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            SceneManager.LoadScene("M5_Forest");
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            SceneManager.LoadScene("M5_Desert");
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            SceneManager.LoadScene("M5_Snowfield");
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            SceneManager.LoadScene("Boss_M4");
        }
    }
}
