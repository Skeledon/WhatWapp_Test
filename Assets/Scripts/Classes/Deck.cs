using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    public const int CARDS_IN_DECK = 52;
    public const int CARDS_PER_SUIT = 13;

    private List<Card> CardList = new List<Card>();

    public int CardsRemaining
    {
        get
        {
            return CardList.Count;
        }
    }

    public void CreateDeck()
    {
        for(int i = 0; i < CARDS_IN_DECK; i++)
        {
            //The +1 is needed because the cards are numbered between 1 and 13, and not between 0 and 12
            Card tmp = new Card((i % CARDS_PER_SUIT) + 1, i / CARDS_PER_SUIT, i);
            CardList.Add(tmp);
        }
    }

    public void ShuffleDeck()
    {
        //This method creates first a list of indexes, from 0 to CARDS_IN_DECK.
        //Then it takes a random index from the list and use it to select a Card from the main list.
        //That card is put in a support List. The index is then removed from the index list.
        //Repeat for until there are no more values in the index list.

        List<int> tmpIndex = new List<int>();
        for(int i = 0; i< CARDS_IN_DECK; i++)
        {
            tmpIndex.Add(i);
        }

        List<Card> auxList = new List<Card>();
        for (int i = 0; i< CARDS_IN_DECK; i++)
        {
            int randomIndex = Random.Range(0, tmpIndex.Count);
            auxList.Add(CardList[tmpIndex[randomIndex]]);
            tmpIndex.RemoveAt(randomIndex);
        }
        CardList = auxList;
    }

    public List<Card> GetCardList()
    {
        return CardList;
    }

    public Card DrawFirstCard()
    {
        Card tmp = CardList[0];
        CardList.RemoveAt(0);
        return tmp;
    }

    public void RemoveCard(Card c)
    {
        CardList.Remove(c);
    }

    public void AddCardOnTopOfDeck(Card c)
    {
        CardList.Insert(0, c);
        c.FlipCard();
    }


}
