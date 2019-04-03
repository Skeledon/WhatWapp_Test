using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableHandler : MonoBehaviour
{
    public struct ExpectedMove
    {
        public int Number;
        public bool[] AcceptedSuits; //0 = hearts, 1 = diamonds, 2 = clubs, 3 = spades
    }

    private const int TABLEAU_SLOTS = 7;
    private const int FOUNDATION_SLOTS = 4;
    private Card[] Tableau = new Card[TABLEAU_SLOTS];
    private Card[] Foundation = new Card[FOUNDATION_SLOTS];
    private Card Waste = new Card(0,-2);
    private ExpectedMove[] TableauExpectedMoves = new ExpectedMove[TABLEAU_SLOTS];
    private ExpectedMove[] FoundationExpectedMoves = new ExpectedMove[FOUNDATION_SLOTS];

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i< TABLEAU_SLOTS; i++)
        {
            Tableau[i] = new Card(14, -2);
        }

        for (int i = 0; i < FOUNDATION_SLOTS; i++)
        {
            Foundation[i] = new Card(14, -2);
        }
    }

    public bool MoveCardToTableauColumn(Card c, int column)
    {
        if (column >= TABLEAU_SLOTS)
            return false;
        if (!CheckValidityOfTableauMove(c, column))
            return false;
        AddCardToTableau(c, column);
        return true;

    }

    private bool CheckValidityOfTableauMove(Card c, int column)
    {
        return TableauExpectedMoves[column].AcceptedSuits[c.Suit] && TableauExpectedMoves[column].Number == c.Number;
    }

    private void AddCardToTableau(Card c, int column)
    {
        Tableau[column].AddCard(c);
    }

    private void CalculateExpectedMoves()
    {
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
            else if (tmp.Color < 2)
            {
                TableauExpectedMoves[i].AcceptedSuits = new bool[] { true, true, false, false };
            }
            else
            {
                TableauExpectedMoves[i].AcceptedSuits = new bool[] { false, false, true, true };
            }

        }
    }
}
