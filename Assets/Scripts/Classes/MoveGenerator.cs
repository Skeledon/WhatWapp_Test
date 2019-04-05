using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGenerator
{
    private int cardCount = 0;
    private Card cardToMove;
    private int from;
    private int to;
    private bool forced = false;


    public bool AddCardToMove(Card c, int destination)
    {
        if(cardCount == 0)
        {
            if (!c.CanBeMoved)
                return false;
            cardToMove = c;
            from = destination;
            cardCount++;
            Debug.Log("false");
            return false;
        }
        else
        {
            to = destination;
            cardCount = 0;
            Debug.Log("true");
            return true;
        }
    }

    public Move GetMove()
    {
            return new Move(cardToMove, from, to, forced);
    }
}
