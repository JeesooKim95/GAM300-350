/*  Class:          GAM350
 *  Team name:      Speaking Potato
 *  Date:           02/02/2022
 *  Contributor:    Jina Hyun
 *  Description:    Credits scene
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public string menuSceneName = "M5_MainMenu";

    public float startY = -37;
    public float endY = 24;

    public float speed = 1;
    private float acel = 4;

    public bool controlEnable = false;

    void Start()
    {
        // Haewon fixed x pos to follow parent's one
        transform.position = new Vector3(gameObject.GetComponentInParent<Transform>().position.x, startY, transform.position.z);
    }

    void Update()
    {
        if(controlEnable)
        {
            if (Input.GetKey(KeyCode.DownArrow))
                transform.Translate(Vector3.up * speed * acel * Time.deltaTime);
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                if (transform.position.y > startY)
                    transform.Translate(Vector3.down * speed * acel * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
                if (Input.anyKeyDown)
                    SceneManager.LoadScene(menuSceneName);
            }

        }
        else
        {
            if(transform.position.y < endY)
                transform.Translate(Vector3.up * speed * Time.deltaTime);
            if (Input.anyKeyDown)
                SceneManager.LoadScene(menuSceneName);
        }
    }
}
