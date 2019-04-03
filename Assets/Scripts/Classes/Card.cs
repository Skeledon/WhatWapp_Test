using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    private Card nextCard;

    public int Number { get; private set; } // 14 = tableau slot. 0 = foundation slot.
    public int Suit { get; private set; } // 0= hearts, 1 = diamonds, 2 = clubs, 3 = spades, -2 = undefined
    public int Color {  get { return Suit / 2; } } // 0 = red, 1 = black, -1 = uncolored

    public bool Movable = true;
    public bool CanBeMoved { get { return Movable && !IsFaceDown; } }

    public bool IsFaceDown = true;

    public int ID { get; private set; }

    public Card(int number, int suit)
    {
        Number = number;
        Suit = suit;
        ID = suit * 13 + number-1;
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

    public void FlipCard()
    {
        IsFaceDown = !IsFaceDown;
    }
}
