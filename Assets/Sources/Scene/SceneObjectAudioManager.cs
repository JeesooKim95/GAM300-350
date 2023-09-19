/*  Class:               GAM350
 *  Team name:      Speaking Potato
 *  Date:                3/1/2022
 *  Contributor:       Haewon Shon, Sinil kang
 *  Description:      Manager class for unique objects (UI, player, player hand)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectAudioManager : MonoBehaviour
{
    // On start- if object is exist : delete, otherwise keep it and change its name
    void Awake()
    {
        GameObject original = GameObject.Find("SceneObjectAudioManager_Inherited");
        if (original != null)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.name = "SceneObjectAudioManager_Inherited";
            DontDestroyOnLoad(gameObject);
        }
    }
}
