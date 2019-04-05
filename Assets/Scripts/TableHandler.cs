using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableHandler
{
    public struct ExpectedMove
    {
        public int Number;
        public bool[] AcceptedSuits; //0 = hearts, 1 = diamonds, 2 = clubs, 3 = spades
    }

    public const int TABLEAU_SLOTS = 7;
    public const int FOUNDATION_SLOTS = 4;

    private Card[] Tableau = new Card[TABLEAU_SLOTS];
    private Card[] Foundation = new Card[FOUNDATION_SLOTS];
    private Card Waste = new Card(0,-2);
    private Card DeckSlot = new Card(-1, -2);
    private ExpectedMove[] TableauExpectedMoves = new ExpectedMove[TABLEAU_SLOTS];
    private ExpectedMove[] FoundationExpectedMoves = new ExpectedMove[FOUNDATION_SLOTS];

    public TableHandler()
    {
        for(int i = 0; i< TABLEAU_SLOTS; i++)
        {
            Tableau[i] = new Card(14, -2);
            Tableau[i].SetAsSlot();
        }

        for (int i = 0; i < FOUNDATION_SLOTS; i++)
        {
            Foundation[i] = new Card(0, i);
            Foundation[i].SetAsSlot();

        }

        CalculateExpectedMoves();
    }

    #region Tableau
    /// <summary>
    /// Try to move the specified card to the specified tableau column. Fails if it's not a valid move.
    /// </summary>
    /// <param name="c"> The card to move</param>
    /// <param name="column"> The TAbleau column</param>
    /// <returns>The last card before adding this card. null if the move is not valid</returns>
    public Card MoveCardToTableauColumn(Card c, int column)
    {
        return MoveCardToTableauColumn(c, column, false);

    }

    /// <summary>
    /// Try to move the specified card to the specified tableau column. Fails if it's not a valid move.
    /// </summary>
    /// <param name="c"> The card to move</param>
    /// <param name="column"> The TAbleau column</param>
    /// <param name="forced">If true the move won't check for calidity conditions</param>
    /// <returns>The last card before adding this card. null if the move is not valid</returns>
    public Card MoveCardToTableauColumn(Card c, int column, bool forced)
    {
        CalculateExpectedMoves();
        if (column >= TABLEAU_SLOTS)
            return null;
        if (!forced)
            if (!CheckValidityOfTableauMove(c, column))
                return null;
        
        return AddCardToTableau(c, column);
    }

    public Card[] GetTableauSlots()
    {
        return Tableau;
    }

    public Card[] GetTableauLastCards()
    {
        Card[] tmp = new Card[TABLEAU_SLOTS];
        for (int i = 0; i < TABLEAU_SLOTS; i++)
        {
            Card tmpCard = Tableau[i];
            while (tmpCard.NextCard() != null)
            {
                tmpCard = tmpCard.NextCard();
            }
            tmp[i] = tmpCard;
        }
         
        return tmp;
    }

    private bool CheckValidityOfTableauMove(Card c, int column)
    {
        return TableauExpectedMoves[column].AcceptedSuits[c.Suit] && TableauExpectedMoves[column].Number == c.Number;
    }

    private Card AddCardToTableau(Card c, int column)
    {
        return Tableau[column].AddCard(c);
    }

    public void RemoveCardFromTableau(Card c, int column)
    {
        Tableau[column].RemoveCard(c);
    }

    #endregion
    #region Foundation
    /// <summary>
    /// Try to move the specified card to the specified tableau column. Fails if it's not a valid move.
    /// </summary>
    /// <param name="c"> The card to move</param>
    /// <param name="column"> The TAbleau column</param>
    /// <returns>The last card before adding this card. null if the move is not valid</returns>
    public Card MoveCardToFoundationColumn(Card c, int column)
    {
        return MoveCardToFoundationColumn(c, column, false);

    }

    /// <summary>
    /// Try to move the specified card to the specified tableau column. Fails if it's not a valid move.
    /// </summary>
    /// <param name="c"> The card to move</param>
    /// <param name="column"> The TAbleau column</param>
    /// <param name="forced">If true the move won't check for calidity conditions</param>
    /// <returns>The last card before adding this card. null if the move is not valid</returns>
    public Card MoveCardToFoundationColumn(Card c, int column, bool forced)
    {
        CalculateExpectedMoves();
        if (column >= FOUNDATION_SLOTS)
            return null;
        if (!forced)
            if (!CheckValidityOfFoundationMove(c, column))
                return null;

        return AddCardToFoundation(c, column);
    }

    public Card[] GetFoundationSlots()
    {
        return Foundation;
    }

    public Card[] GetFoundationLastCards()
    {
        Card[] tmp = new Card[FOUNDATION_SLOTS];
        for (int i = 0; i < FOUNDATION_SLOTS; i++)
        {
            Card tmpCard = Foundation[i];
            while (tmpCard.NextCard() != null)
            {
                tmpCard = tmpCard.NextCard();
            }
            tmp[i] = tmpCard;
        }

        return tmp;
    }

    private bool CheckValidityOfFoundationMove(Card c, int column)
    {
        return FoundationExpectedMoves[column].AcceptedSuits[c.Suit] && FoundationExpectedMoves[column].Number == c.Number;
    }

    private Card AddCardToFoundation(Card c, int column)
    {
        return Foundation[column].AddCard(c);
    }

    public void RemoveCardFromFoundation(Card c, int column)
    {
        Foundation[column].RemoveCard(c);
    }

    #endregion
    private void CalculateExpectedMoves()
    {
        //Tableau expected moves
        for(int i = 0; i < TABLEAU_SLOTS; i ++)
        {
            Card tmp = Tableau[i];
            while(tmp.NextCard() != null)
            {
                tmp = tmp.NextCard();
            }
            TableauExpectedMoves[i].Number = tmp.Number - 1;

            //if the color is negative it means that we are dealing with the base table slot, so any suit is ok.
            //arrays initialized manually because it's faster.
            if(tmp.Color < 0)
                TableauExpectedMoves[i].AcceptedSuits = new bool[] { true, true, true, true };
            else if (tmp.Color < 1)
            {
                TableauExpectedMoves[i].AcceptedSuits = new bool[] { false, false, true, true };
            }
            else
            {
                TableauExpectedMoves[i].AcceptedSuits = new bool[] { true, true, false, false };
            }
        }

        //foundation expected moves
        for (int i = 0; i < FOUNDATION_SLOTS; i++)
        {
            Card tmp = Foundation[i];
            while (tmp.NextCard() != null)
            {
                tmp = tmp.NextCard();
            }
            FoundationExpectedMoves[i].Number = tmp.Number + 1;
            FoundationExpectedMoves[i].AcceptedSuits = new bool[4];
            for (int j = 0; j < FOUNDATION_SLOTS; j++)
            {
                if(i!=j)
                    FoundationExpectedMoves[i].AcceptedSuits[j] = false;
                else
                    FoundationExpectedMoves[i].AcceptedSuits[j] = true;
            }
        }
    }
}
