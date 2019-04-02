using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ICardLinker
{
    private Card nextCard;

    public int Number { get; private set; }
    public int Suit { get; private set; }

    public bool Movable = true;
    public bool CanBeMoved { get { return Movable && !IsCovered; } }

    public bool IsCovered = false;

    public Card(int number, int suit)
    {
        Number = number;
        Suit = suit;
    }

    public void AddCard(Card c)
    {
        if (nextCard == null)
            nextCard = c;
        else
            nextCard.AddCard(c);
    }

    public Card NextCard()
    {
        return nextCard;
    }
}
