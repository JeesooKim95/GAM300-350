/*
    Team    : Speaking Potato
    Author  : Sangmin Kim
    Date    : 10/17/2021
*/

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandsManager : MonoBehaviour
{
    private Dictionary<int, int> hands;
    List<Card> handsToDisplayToUi;
    public int handsSize = 0;
    public HandsType currentHandsType = HandsType.None;

    public PlayerHandUI[] handUis;
    //public Text handsType;
    public Text skillUseIndicator;
    public TextMeshProUGUI handsType;
    public Sprite backCardImage;
    public PlayerUpgrade playerUpgrade;


    // Sinil - Purpose of Play sounds
    public AudioManager audioManager;
    //public TempsavePlayerHands tempsave;

    int SortByValue(Card a, Card b)
    {
        return a.value.CompareTo(b.value);
    }

    public void Add(Card card)
    {
        if (handsToDisplayToUi.Count <= 4)
        {
            PlayDrawCard(handsToDisplayToUi.Count);

            handsSize++;

            if (hands.ContainsKey(card.value))
            {
                //means already exist
                hands[card.value]++;
            }
            else
            {
                //means not exist
                hands.Add(card.value, 1);
            }
            //handTypes.Add(card.type);


            handsToDisplayToUi.Add(card);

            handsToDisplayToUi.Sort(SortByValue);

            for (int i = 0; i < handsToDisplayToUi.Count; ++i)
            {
                handUis[i].SetImage(backCardImage);
                // Card cardValue = handsToDisplayToUi[i];
                // string text = "";
                // if (cardValue.value <= 10)
                // {                    
                //     text = cardValue.value.ToString();
                //     if (cardValue.value == 1)
                //     {
                //         text = "A";
                //     }
                // }
                // else
                // {
                //     int diff = cardValue.value - 10;

                //     switch (diff)
                //     {
                //         case 1:
                //             text = "J";
                //             break;

                //         case 2:
                //             text = "Q";
                //             break;

                //         case 3:
                //             text = "K";
                //             break;
                //     }
                // }

                // switch (cardValue.type)
                // {
                //     case Card.CardType.Spade:
                //         //text += "S";
                //         text += "♠";
                //         handUis[i].SetColor(Color.black);
                //         break;

                //     case Card.CardType.Heart:
                //         //text += "H";
                //         text += "♥";
                //         handUis[i].SetColor(Color.red);
                //         break;

                //     case Card.CardType.Diamond:
                //         //text += "D";
                //         text += "♦";
                //         handUis[i].SetColor(Color.red);
                //         break;

                //     case Card.CardType.Clover:
                //         //text += "C";
                //         text += "♣";
                //         handUis[i].SetColor(Color.black);
                //         break;

                //     default:
                //         text += "None";
                //         break;
                // }
                // handUis[i].SetText(text);
            }


            // if (handsSize > 4)
            // {
            //     handsSize--;
            // }

            // if(tempsave)
            // {
            //     tempsave.cards.Add(card);
            // }
            CheckHand();
        }

    }
    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        hands = new Dictionary<int, int>();
        handsToDisplayToUi = new List<Card>();

        if (!playerUpgrade)
            playerUpgrade = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUpgrade>();

        audioManager = FindObjectOfType<AudioManager>();
    }

    void CheckHand()
    {
        if (handsSize >= 5)
        {
            int dictionaryCount = hands.Count;
            if (dictionaryCount >= 5)
            {
                //means all cards value is not duplicated,
                //needs to check straight, flush

                //check flush first
                Card.CardType prevType = Card.CardType.None;
                bool isAllIdentical = true;
                bool flushFlag = false;

                foreach (Card type in handsToDisplayToUi)
                {
                    if (prevType != Card.CardType.None)
                        if (prevType != type.type)
                        {
                            isAllIdentical = false;
                        }
                    prevType = type.type;
                }

                if (isAllIdentical)
                    flushFlag = true;

                bool straightFlag = false;

                Card prevCard = null;
                bool isStraight = true;
                foreach (Card cardVal in handsToDisplayToUi)
                {
                    if (prevCard != null)
                    {
                        if (prevCard.value != cardVal.value - 1)
                            isStraight = false;
                    }
                    prevCard = cardVal;
                }

                if (isStraight)
                    straightFlag = true;

                if (straightFlag == true && flushFlag == true)
                {
                    currentHandsType = HandsType.StarightFlush;
                }
                else if (straightFlag == true && flushFlag == false)
                {
                    currentHandsType = HandsType.Straight;
                }
                else if (straightFlag == false && flushFlag == true)
                {
                    currentHandsType = HandsType.Flush;
                }
                else
                {
                    currentHandsType = HandsType.Top;
                }
            }
            else
            {
                bool pairFlag = false;
                bool twoPairFlag = false;
                bool tripleFlag = false;
                bool fourFlag = false;
                bool fullHouseFlag = false;

                foreach (int key in hands.Keys)
                {
                    int dicValue = hands[key];
                    if (dicValue == 2)
                    {
                        if (!pairFlag)
                        {
                            pairFlag = true;
                        }
                        else
                        {
                            twoPairFlag = true;
                        }

                        if (tripleFlag)
                            fullHouseFlag = true;
                    }
                    else if (dicValue == 3)
                    {
                        tripleFlag = true;

                        if (pairFlag)
                            fullHouseFlag = true;
                    }
                    else if (dicValue >= 4)
                    {
                        fourFlag = true;
                    }
                }

                if (pairFlag)
                    currentHandsType = HandsType.Pair;

                if (twoPairFlag)
                    currentHandsType = HandsType.TwoPair;

                if (tripleFlag)
                    currentHandsType = HandsType.Triple;

                if (fullHouseFlag)
                    currentHandsType = HandsType.FullHouse;

                if (fourFlag)
                    currentHandsType = HandsType.FourCard;

                if (currentHandsType == HandsType.None)
                    currentHandsType = HandsType.Top;

                bool isItFlush = true;
                Card.CardType cardType = handsToDisplayToUi[0].type;

                foreach (Card cardVal in handsToDisplayToUi)
                {
                    if (cardVal.type != cardType)
                    {
                        isItFlush = false;
                    }
                }

                if (isItFlush)
                {
                    currentHandsType = HandsType.Flush;
                }
            }
            switch (currentHandsType)
            {
                case HandsType.None:
                    break;

                case HandsType.Pair:
                    handsType.text = "Press E: Pair\n(Power Up)";
                    break;

                case HandsType.Triple:
                    handsType.text = "Press E: Triple\n(Rapid Fire)";
                    break;

                case HandsType.FullHouse:
                    handsType.text = "Press E: Full House\n(Rapid Double Gun)";
                    break;

                case HandsType.TwoPair:
                    handsType.text = "Press E: Two Pair\n(Double Pistol)";
                    break;

                case HandsType.FourCard:
                    handsType.text = "Press E: Four Card\n(Grenade Launcher)";
                    break;

                case HandsType.Top:
                    handsType.text = "Press E: Top\n(Power Up)";
                    break;

                case HandsType.StarightFlush:
                    handsType.text = "Press E: Straight Flush\n(Icy Shotgun)";
                    break;

                case HandsType.Flush:
                    handsType.text = "Press E: Flush\n(Shotgun)";
                    break;

                case HandsType.Straight:
                    handsType.text = "Press E: Straight\n(Icy Gun)";
                    break;
            }

            if (currentHandsType != HandsType.None)
            {
                for (int i = 0; i < handsToDisplayToUi.Count; ++i)
                {
                    handUis[i].BackToOriginalSprite();
                    Card cardValue = handsToDisplayToUi[i];
                    string text = "";
                    if (cardValue.value <= 10)
                    {
                        text = cardValue.value.ToString();
                        if (cardValue.value == 1)
                        {
                            text = "A";
                        }
                    }
                    else
                    {
                        int diff = cardValue.value - 10;

                        switch (diff)
                        {
                            case 1:
                                text = "J";
                                break;

                            case 2:
                                text = "Q";
                                break;

                            case 3:
                                text = "K";
                                break;
                        }
                    }

                    switch (cardValue.type)
                    {
                        case Card.CardType.Spade:
                            //text += "S";
                            text += "♠";
                            handUis[i].SetColor(Color.black);
                            break;

                        case Card.CardType.Heart:
                            //text += "H";
                            text += "♥";
                            handUis[i].SetColor(Color.red);
                            break;

                        case Card.CardType.Diamond:
                            //text += "D";
                            text += "♦";
                            handUis[i].SetColor(Color.red);
                            break;

                        case Card.CardType.Clover:
                            //text += "C";
                            text += "♣";
                            handUis[i].SetColor(Color.black);
                            break;

                        default:
                            text += "None";
                            break;
                    }
                    handUis[i].SetText(text);
                }
            }

            if (skillUseIndicator)
                skillUseIndicator.gameObject.SetActive(true);

            handsType.gameObject.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerUpgrade upgradeInfo = other.gameObject.GetComponent<PlayerUpgrade>();

            // if(tempsave == null)
            // {
            //     tempsave = GameObject.FindObjectOfType<TempsavePlayerHands>();

            //     if(tempsave == null)
            //     {
            //         GameObject temp = GameObject.Find("HandsSaver");

            //         if(temp != null)
            //         {
            //             tempsave = temp.GetComponent<TempsavePlayerHands>();
            //         }
            //     }
            // }

            // if(tempsave != null)
            // {
            //     List<Card> cards_ = new List<Card>();

            //     foreach(Card card in tempsave.cards)
            //     {
            //         cards_.Add(card);
            //     }

            //     for(int i = 0; i < cards_.Count; ++i)
            //     {
            //         Add(cards_[i]);
            //     }
            //     tempsave.cards.Clear();
            // }

            BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
            boxCollider.enabled = false;
        }
    }

    void Update()
    {
        bool leftCtrl = Input.GetKey(KeyCode.LeftControl);

           if (Input.GetKeyDown(KeyCode.Alpha1) && leftCtrl)
           {
               ClearHands();

               Add(new Card(Card.CardType.Diamond, 7));
               Add(new Card(Card.CardType.Spade, 1));
               Add(new Card(Card.CardType.Diamond, 2));
               Add(new Card(Card.CardType.Diamond, 3));
               Add(new Card(Card.CardType.Diamond, 4));


               //playerUpgrade.SetHands(HandsType.Top);
           }
           if (Input.GetKeyDown(KeyCode.Alpha2) && leftCtrl)
           {
               ClearHands();

               Add(new Card(Card.CardType.Diamond, 7));
               Add(new Card(Card.CardType.Spade, 7));
               Add(new Card(Card.CardType.Diamond, 2));
               Add(new Card(Card.CardType.Diamond, 3));
               Add(new Card(Card.CardType.Diamond, 4));

               //playerUpgrade.SetHands(HandsType.Pair);
           }
           if (Input.GetKeyDown(KeyCode.Alpha3) && leftCtrl)
           {
               ClearHands();

               Add(new Card(Card.CardType.Diamond, 7));
               Add(new Card(Card.CardType.Spade, 7));
               Add(new Card(Card.CardType.Diamond, 2));
               Add(new Card(Card.CardType.Spade, 2));
               Add(new Card(Card.CardType.Diamond, 4));

               //playerUpgrade.SetHands(HandsType.TwoPair);
           }
           if (Input.GetKeyDown(KeyCode.Alpha4) && leftCtrl)
           {
               ClearHands();


               Add(new Card(Card.CardType.Diamond, 7));
               Add(new Card(Card.CardType.Spade, 7));
               Add(new Card(Card.CardType.Heart, 7));
               Add(new Card(Card.CardType.Diamond, 3));
               Add(new Card(Card.CardType.Diamond, 4));

               //playerUpgrade.SetHands(HandsType.Triple);
           }
           if (Input.GetKeyDown(KeyCode.Alpha5) && leftCtrl)
           {
               ClearHands();

               Add(new Card(Card.CardType.Diamond, 7));
               Add(new Card(Card.CardType.Spade, 7));
               Add(new Card(Card.CardType.Heart, 7));
               Add(new Card(Card.CardType.Diamond, 3));
               Add(new Card(Card.CardType.Spade, 3));

               //playerUpgrade.SetHands(HandsType.FullHouse);
           }
           if (Input.GetKeyDown(KeyCode.Alpha6) && leftCtrl)
           {
               ClearHands();

               Add(new Card(Card.CardType.Diamond, 7));
               Add(new Card(Card.CardType.Spade, 7));
               Add(new Card(Card.CardType.Clover, 7));
               Add(new Card(Card.CardType.Heart, 7));
               Add(new Card(Card.CardType.Diamond, 4));
               //playerUpgrade.SetHands(HandsType.FourCard);
           }
           if (Input.GetKeyDown(KeyCode.Alpha7) && leftCtrl)
           {
               ClearHands();

               Add(new Card(Card.CardType.Diamond, 1));
               Add(new Card(Card.CardType.Diamond, 7));
               Add(new Card(Card.CardType.Diamond, 3));
               Add(new Card(Card.CardType.Diamond, 8));
               Add(new Card(Card.CardType.Diamond, 5));
               //playerUpgrade.SetHands(HandsType.Flush);
           }
           if (Input.GetKeyDown(KeyCode.Alpha8) && leftCtrl)
           {
               ClearHands();


               Add(new Card(Card.CardType.Diamond, 1));
               Add(new Card(Card.CardType.Diamond, 2));
               Add(new Card(Card.CardType.Diamond, 3));
               Add(new Card(Card.CardType.Diamond, 4));
               Add(new Card(Card.CardType.Diamond, 5));
               //playerUpgrade.SetHands(HandsType.StarightFlush);
           }

        if (Input.GetKeyDown(KeyCode.E) && currentHandsType != HandsType.None)
        {
            PlayActivateCard(currentHandsType);

            playerUpgrade.SetHands(currentHandsType);
            ClearHands();
            handsType.text = "";

            if (skillUseIndicator)
                skillUseIndicator.gameObject.SetActive(false);

            handsType.gameObject.SetActive(false);
        }
    }

    public void ClearHands()
    {
        hands.Clear();
        foreach (PlayerHandUI text in handUis)
        {
            text.ResetCard();
        }
        handsSize = 0;
        currentHandsType = HandsType.None;
        handsToDisplayToUi.Clear();

        if (skillUseIndicator)
            skillUseIndicator.gameObject.SetActive(false);

        handsType.gameObject.SetActive(false);
    }
    void PutRandom5Cards()
    {
        for (int i = 0; i < 5; ++i)
        {
            int randomType = Random.Range(1, 4);
            int randomValue = Random.Range(1, 13);

            Card randomCard = new Card((Card.CardType)randomType, randomValue);

            Add(randomCard);
        }
    }

    /*
     Sinil.kang
    10/25/2021
     */
    private void PlayDrawCard(int numHandCards)
    {
        if(numHandCards >= 4)
        {
            audioManager.Play("FifthCard");
        }
        else
        {
            audioManager.PlayDrawCard();
        }
    }
    /*
     Sinil.kang
    10/25/2021
     */
    private void PlayActivateCard(HandsType type)
    {
        switch (type)
        {
            case HandsType.Top:
                audioManager.Play("Top");
                break;
            case HandsType.Pair:
                audioManager.Play("Pair");
                break;
            case HandsType.TwoPair:
                audioManager.Play("TwoPair");
                break;
            case HandsType.Triple:
                audioManager.Play("Triple");
                break;
            case HandsType.Straight:
                audioManager.Play("Straight");
                break;
            case HandsType.Flush:
                audioManager.Play("Flush");
                break;
            case HandsType.FullHouse:
                audioManager.Play("FullHouse");
                break;
            case HandsType.FourCard:
                audioManager.Play("Fourcard");
                break;
            case HandsType.StarightFlush:
                audioManager.Play("StraightFlush");
                break;
            default:
                break;
        }
    }
}
