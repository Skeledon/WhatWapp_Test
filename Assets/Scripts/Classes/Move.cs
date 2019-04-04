using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public Card MovedCard { get; private set; }
    public int ID { get; private set; }
    public int From { get; private set; }
    public int To { get; private set; }
    public bool IsForced { get; private set; }

    public Move(Card c, int from, int to, bool forced)
    {
        MovedCard = c;
        ID = MovedCard.ID;
        From = from;
        To = to;
        IsForced = forced;
    }

}
