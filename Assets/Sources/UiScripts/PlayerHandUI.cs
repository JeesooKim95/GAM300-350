/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                10/26/2021
 *  Contributor:       Haewon Shon
 *  Description:      script for player hand card UI
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandUI : MonoBehaviour
{
    private Image background;

    //sangmin
    public Text cardText;
    public Sprite originalSprite;
    public Font font;

    private void Start()
    {
        background = GetComponent<Image>();
        cardText.font = font;
    }

    public void SetText(string text)
    {
        cardText.text = text;
    }

    public void SetColor(Color color)
    {
        cardText.color = color;
    }
    public void SetImage(Sprite sprite)
    {
        background.sprite = sprite;
    }

    public void ResetCard()
    {
        cardText.text = "";
        background.color = Color.white;
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0.5f);
        background.sprite = originalSprite;
    }
    public void BackToOriginalSprite()
    {
        background.sprite = originalSprite;
    }
}
