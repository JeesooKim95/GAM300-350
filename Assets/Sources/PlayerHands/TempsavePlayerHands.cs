using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempsavePlayerHands : MonoBehaviour
{
    public List<Card> cards;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        cards = new List<Card>();
    }




}
