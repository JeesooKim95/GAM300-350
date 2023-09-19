/*
    Team    : Speaking Potato
    Author  : Sangmin Kim
    Date    : 10/17/2021
*/
public class Card
{
    public Card(CardType ltype, int lvalue)
    {
        value = lvalue;
        type = ltype;
    }
    public CardType type;
    public int value;
    public enum CardType{
        None = 0,
        Spade,
        Heart,
        Clover,
        Diamond
    }
}

public enum HandsType{
    None = 0,
    Top,
    Pair,
    TwoPair,
    Triple,
    Straight,
    BackStraight,
    Flush,
    FullHouse,
    FourCard,
    StarightFlush,
    BackStraightFlush,
    RoyalStraightFlush
}

// Jina Hyun: 11/04/2021
public enum GunType
{
    Default,
    ShotGun,
    BombGun
}
