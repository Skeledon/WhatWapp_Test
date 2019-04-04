using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    private Card nextCard;

    public int Number { get; private set; } // 14 = tableau slot. 0 = foundation slot. -1 = waste slot. -2 = deck slot
    public int Suit { get; private set; } // 0= hearts, 1 = diamonds, 2 = clubs, 3 = spades, -2 = undefined
    public int Color {  get { return Suit / 2; } } // 0 = red, 1 = black, -1 = uncolored

    public bool Movable = true;
    public bool CanBeMoved { get { return Movable && !IsFaceDown; } }

    public bool IsFaceDown = true;

    public int ID { get; private set; }

    public Card(int number, int suit)
        : this(number,suit,-1)
    {

    }

    public Card(int number, int suit, int uniqueID)
    {
        Number = number;
        Suit = suit;
        ID = uniqueID;
    }

    /// <summary>
    /// Adds card to the column the card c is in. Returns the ID of the card it positioned itself directly above
    /// </summary>
    /// <param name="c">The card to add</param>
    /// <returns>The card directly above the added card. </returns>
    public Card AddCard(Card c)
    {
        if (nextCard == null)
        {
            nextCard = c;
            return this;
        }
        else
        {
            return nextCard.AddCard(c);
        }
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
