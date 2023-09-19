/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                2/2/2022
 *  Contributor:       Sinil Kang
 *  Description:       Script to flip card in MainMenu
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseFlipCard : MonoBehaviour
{
    public float x, y, z;

    public GameObject cardFront;
    public GameObject cardBack;

    public GameObject description;
    public GameObject descriptionTextBackground;

    public bool cardBackIsActive;

    public int timer;

    public bool isFliped = false;

    public int flipDegree = 0;

    private bool isHovered = false;

    // Start is called before the first frame update
    void Start()
    {
        cardBackIsActive = false;
        isHovered = false;
    }

    // Update is called once per frame
    void Update()
    {
        isHovered = IsHovered();
        if (isFliped == false)
        {
            if (Input.GetMouseButtonDown(0) && isHovered)
            {
                Flip();
            }
        }

        if(isHovered)
        {
            description.SetActive(true);
            Vector3 descriptionSize = descriptionTextBackground.transform.TransformVector(0.5f * descriptionTextBackground.transform.GetComponent<RectTransform>().sizeDelta);
            description.transform.position = new Vector3(Input.mousePosition.x + (1.2f * descriptionSize.x), Input.mousePosition.y - (1.45f*descriptionSize.y), -10);
        }
        else
        {
            description.SetActive(false);
        }
    }

    public void Flip()
    {
        if (cardBackIsActive == true)
        {
            cardFront.SetActive(true);
            cardBack.SetActive(false);
            cardBackIsActive = false;
        }
        else
        {
            cardFront.SetActive(false);
            cardBack.SetActive(true);
            cardBackIsActive = true;
        }
    }

    private bool IsHovered()
    {
        Vector3 sizeDelta;
        Vector3 positiveXY;

        if(cardBackIsActive)
        {
            sizeDelta = cardBack.transform.GetComponent<RectTransform>().sizeDelta;
            positiveXY = cardBack.transform.TransformVector(0.5f * sizeDelta);
        }
        else
        {
            sizeDelta = cardFront.transform.GetComponent<RectTransform>().sizeDelta;
            positiveXY = cardFront.transform.TransformVector(0.5f * sizeDelta);
        }


        if (Input.mousePosition.x < gameObject.transform.position.x + positiveXY.x &&
            Input.mousePosition.x > gameObject.transform.position.x - positiveXY.x &&
            Input.mousePosition.y < gameObject.transform.position.y + positiveXY.y &&
            Input.mousePosition.y > gameObject.transform.position.y - positiveXY.y
            )
        {
            return true;
        }

        return false;
    }
}
