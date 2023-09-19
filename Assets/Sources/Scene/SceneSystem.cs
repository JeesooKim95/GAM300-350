/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                10/3/2021
 *  Contributor:       Su Kim
 *  Description:       Basic system for managing the scene
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public static class SceneSystem
{
    public enum Scene
    {
        SampleScene,
        SampleScene2,
        MenuScene,
    }

    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

}
