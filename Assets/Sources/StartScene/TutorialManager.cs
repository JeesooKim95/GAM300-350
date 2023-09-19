/*
    Team    : Speaking Potato
    Author  : Sean Kim
    Date    : 3/17/2022
    Desc    : Tutorial Manager (Messages and wait for action).

 * Tutorial Order
 * 1. How to move (WASD)
 * 2. How to jump (Space)
 * 3. How to dash (L-Shift)
 * 4. How to shoot (left click)
 * 5. How to use skill (E)
 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] message;
    public GameObject wall;
    private int msgIndex;
    private float waitTimer = 3.0f;
    private float wallTimer = 8.0f;
    private bool isWallOn = true;
    private int dummyCount = 10;
    Scene scene;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F9) || Input.GetKeyDown(KeyCode.F10) || Input.GetKeyDown(KeyCode.F11) || Input.GetKeyDown(KeyCode.F12))
        {
            foreach(GameObject msg in message)
            {
                msg.SetActive(false);
            }            
        }
        dummyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        for (int i = 0; i < message.Length; i++)
        {    
            if (i == msgIndex)
            {
                if(waitTimer <= 0)
                {
                    if (i == 4)
                    {
                        wall.SetActive(false);
                        isWallOn = false;
                    }
                    message[i].SetActive(true);
                    if (i == 3)
                    {
                        waitTimer = 8.0f;
                    }
                    else
                    {
                        waitTimer = 2.0f;
                    }
                    
                }
                else
                {
                    waitTimer -= Time.deltaTime;
                }
            }
            else
            {
                message[i].SetActive(false);
            }
        }
        //1. How to move (WASD)
        if (msgIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                msgIndex++;
            }
        }
        //2.How to jump(Space)
        else if (msgIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                msgIndex++;
            }
        }
        //3. How to dash (L-Shift)
        else if (msgIndex == 2)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                msgIndex++;
            }
        }
        //4. How to shoot (left click)
        else if (msgIndex == 3)
        {
            if (Input.GetMouseButton(0))
            {
                msgIndex++;
            }
        }
        //5. How to use skill (E)
        else if (msgIndex == 4)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                msgIndex++;
            }
        }

        if(isWallOn == true && dummyCount <= 5)
        {
            wall.SetActive(false);
            isWallOn = false;
        }
    }

}
