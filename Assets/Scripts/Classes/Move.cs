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
    public int TableauFlipped { get; private set; }
    public bool RollBack { get; private set; }

    public Move(Card c, int from, int to, bool forced)
        : this(c, from, to, forced, -1, false)
    {
    }

    public Move(Card c, int from, int to, bool forced, bool rollBackDeck)
        :this(c,from,to,forced,-1, rollBackDeck)
    {
    }

    public Move(Card c, int from, int to, bool forced, int tableauFlipped, bool rollBackDeck)
    {
        MovedCard = c;
        ID = MovedCard.ID;
        From = from;
        To = to;
        IsForced = forced;
        TableauFlipped = tableauFlipped;
        RollBack = rollBackDeck;
    }

    public void FlipTableau(int column)
    {
        TableauFlipped = column;
    }

}
