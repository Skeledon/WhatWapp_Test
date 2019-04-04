using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceController : MonoBehaviour
{
    public GameHandler MyGameHandler;
    public VisualCardsHandler MyCardsHandler;
    public VisualTableHandler MyVisualTableHandler;
    
    public void GenerateCards(List<Card> c)
    {
        MyCardsHandler.GenerateCards(c);
    }

    public void FlipCard(int id)
    {
        MyCardsHandler.FlipCard(id);
    }

    public void MoveCardToTableau(Card c, Card destinationCard, int column)
    {
        MyVisualTableHandler.MoveCardToTableau(c, column);
    }

    public Card[] GetTableauLastCards()
    {
        return MyGameHandler.GetTableauLastCards();
    }
}
