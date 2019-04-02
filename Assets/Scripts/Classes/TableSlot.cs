using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableSlot : ICardLinker
{
    private Card nextCard;

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
