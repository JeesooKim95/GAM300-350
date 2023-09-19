/*  Class:               GAM350
 *  Team name:      Speaking Potato
 *  Date:                1/26/2022
 *  Contributor:       Haewon Shon
 *  Description:      Manager class for unique objects (UI, player, player hand)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectManager : MonoBehaviour
{
    // On start- if object is exist : delete, otherwise keep it and change its name
    void Awake()
    {
        GameObject original = GameObject.Find("SceneObjectManager_Inherited");
        if (original != null)
        {
            // initialize player transform with that already in the map
            original.GetComponentInChildren<PlayerMovement>().InitTransform(gameObject.GetComponentInChildren<PlayerMovement>().transform);
            Destroy(gameObject);
        }
        else
        {
            gameObject.name = "SceneObjectManager_Inherited";
            DontDestroyOnLoad(gameObject);
        }
    }
}
