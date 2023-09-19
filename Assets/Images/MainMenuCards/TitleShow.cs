/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                2/2/2022
 *  Contributor:       Sinil Kang
 *  Description:       Script to hide press any key to start and show title Image.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleShow : MonoBehaviour
{
    public Image title;
    public Image pats;
    public bool ShowTitle = false;
    public bool flag = true;

    [Range(0, 1)]
    public float speed = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ShowTitle == false)
        {
            if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
            {
                ShowTitle = true;
                FindObjectOfType<AudioManager>().Play("ButtonClick");
            }
        }
        else if(flag == true)
        {
            pats.color = new Color(1f, 1f, 1f, pats.color.a - speed * 5 * Time.deltaTime);
            title.color = new Color(1f, 1f, 1f, title.color.a + speed * Time.deltaTime);

            if(title.color.a >= 1f)
            {
                flag = false;
            }
        }
    }
}
