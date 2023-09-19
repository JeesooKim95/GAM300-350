/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 11/9/2021
    Desc    : Card object dropped from enemies
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    public Card card = new Card(Card.CardType.None, 0);

    public void SetPlayerHand()
    {
        GameObject.Find("PlayerHandsManager").GetComponent<PlayerHandsManager>().Add(card);
    }
}
