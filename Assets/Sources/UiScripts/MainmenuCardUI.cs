using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainmenuCardUI : MonoBehaviour
{
    private const int cardSize = 3;
    public GameObject[] cards;
    public float[] rotationNotHovered;
    public float[] rotationHovered;
    public Vector2[] positionNotHovered;
    public Vector2[] positionHovered;
    public bool[] hovered;
    private int hoveredNum;
    private bool isUpdateStart;
    private int previousHoveredNum;

    private float timer = 0f;

    public float[] rotationStart;
    public Vector2[] positionStart;

    public bool isAnyHovered;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < cardSize; i++)
        {
            rotationStart[i] = rotationNotHovered[i];
            positionStart[i] = positionNotHovered[i];
        }

        hoveredNum = -1;
        isUpdateStart = false;
        isAnyHovered = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * 5;

        previousHoveredNum = hoveredNum;
        for (int i = 0; i < cardSize; i++)
        {
            hovered[i] = false;
        }

        hoveredNum = isMouseHovered();
        if (hoveredNum < 0)
        {
            isAnyHovered = false;
        }
        else
        {
            isAnyHovered = true;
            hovered[hoveredNum] = true;
        }


        if (hoveredNum != previousHoveredNum)
        {
            isUpdateStart = true;
        }

        UpdateStartings();

        Vector3 destinationAngle = new Vector3(0f, 0f);
        Vector3 destinationPos = new Vector3(0f, 0f);

        for (int i = 0; i < cardSize; i++)
        {
            GetDestinations(i, ref destinationAngle, ref destinationPos);


            RectTransform rect = cards[i].transform.GetComponent<RectTransform>();
            rect.eulerAngles = new Vector3(0, 0, Mathf.Lerp(rotationStart[i], destinationAngle.z, easeOutExpo(timer)));
            float x = Mathf.Lerp(positionStart[i].x, destinationPos.x, timer);
            float y = Mathf.Lerp(positionStart[i].y, destinationPos.y, timer);

            rect.localPosition = new Vector3(x, y, 0);
            
        }
    }

    private int isMouseHovered()
    {
        RectTransform rect0 = cards[0].transform.GetComponent<RectTransform>();
        Vector3 sizeDelta0 = rect0.sizeDelta;
        Vector3 positiveXY0 = cards[0].transform.TransformVector(0.8f * sizeDelta0);

        RectTransform rect1 = cards[1].transform.GetComponent<RectTransform>();
        Vector3 sizeDelta1 = rect1.sizeDelta;
        Vector3 positiveXY1 = cards[1].transform.TransformVector(0.8f * sizeDelta1);

        RectTransform rect2 = cards[2].transform.GetComponent<RectTransform>();
        Vector3 sizeDelta2 = rect2.sizeDelta;
        Vector3 positiveXY2 = cards[2].transform.TransformVector(0.8f * sizeDelta2);

        if (Input.mousePosition.x < cards[2].transform.position.x + positiveXY2.x &&
            Input.mousePosition.x > cards[0].transform.position.x - positiveXY0.x &&
            Input.mousePosition.y < cards[1].transform.position.y + positiveXY1.y &&
            Input.mousePosition.y > cards[1].transform.position.y - positiveXY1.y
            )
        {
            Vector3 card1 = Input.mousePosition - cards[0].transform.position;
            Vector3 card2 = Input.mousePosition - cards[1].transform.position;
            Vector3 card3 = Input.mousePosition - cards[2].transform.position;

            Vector3 smaller12;
            if (card1.sqrMagnitude < card2.sqrMagnitude)
            {
                if (card1.sqrMagnitude < card3.sqrMagnitude)
                {
                    return 0;
                }
            }

            if (card2.sqrMagnitude < card1.sqrMagnitude)
            {
                if (card2.sqrMagnitude < card3.sqrMagnitude)
                {
                    return 1;
                }
            }

            if (card3.sqrMagnitude < card1.sqrMagnitude)
            {
                if (card3.sqrMagnitude < card2.sqrMagnitude)
                {
                    return 2;
                }
            }



        }

        return -1;
    }

    private float easeOutExpo(float x)
    {
        if (x >= 1f)
        {
            return 1f;
        }

        return 1 - Mathf.Pow(2, -10 * x);
    }

    private void GetDestinations(int index, ref Vector3 angle, ref Vector3 pos)
    {
        // Update Position when it is hovered
        if (hovered[index])
        {
            RectTransform rect = cards[index].transform.GetComponent<RectTransform>();
            angle = new Vector3(0, 0, rotationHovered[index]);
            pos = new Vector3(positionHovered[index].x, positionHovered[index].y, rect.position.z);
        }
        else
        {
            // Update position when it is not hovered but any cards are hovered. (Player hover mouse onto any cards)
            if (isAnyHovered)
            {
                // When the card is right side than hovered card
                if (hoveredNum < index)
                {
                    RectTransform rect = cards[index].GetComponent<RectTransform>();
                    angle = new Vector3(0, 0, rotationNotHovered[index]);
                    pos = new Vector3(positionNotHovered[index].x + 80f, positionNotHovered[index].y, rect.position.z);
                }

                // When the card is left side than hovered card
                else
                {
                    RectTransform rect = cards[index].GetComponent<RectTransform>();
                    angle = new Vector3(0, 0, rotationNotHovered[index]);
                    pos = new Vector3(positionNotHovered[index].x - 80f, positionNotHovered[index].y, rect.position.z);
                }
            }
            // Update position when it is completely not hovered. (Player does not hover any cards)
            else
            {
                RectTransform rect = cards[index].GetComponent<RectTransform>();
                angle = new Vector3(0, 0, rotationNotHovered[index]);
                pos = new Vector3(positionNotHovered[index].x, positionNotHovered[index].y, rect.position.z);
            }
        }
    }

    private void UpdateStartings()
    {
        if (isUpdateStart)
        {
            for (int i = 0; i < cardSize; i++)
            {
                RectTransform rect = cards[i].GetComponent<RectTransform>();
                rotationStart[i] = rect.eulerAngles.z;
                if (rotationStart[i] > 180)
                {
                    rotationStart[i] -= 360;
                }
                positionStart[i] = new Vector2(rect.localPosition.x, rect.localPosition.y);
            }

            timer = 0f;
            isUpdateStart = false;
        }
    }
}
