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

    public void GenerateTableauSlots(Card[] slots)
    {
        MyVisualTableHandler.GenerateTableauSlotsCard(slots);
    }

    public void GenerateFoundationSlots(Card[] slots)
    {
        MyVisualTableHandler.GenerateFoundationSlotsCard(slots);
    }

    public void GenerateWasteSlot(Card c)
    {
        MyVisualTableHandler.GenerateWasteSlotCard(c);
    }

    public void GenerateDeckSlot()
    { }


    public void FlipCard(int id)
    {
        MyCardsHandler.FlipCard(id);
    }

    #region Tableau
    public void MoveCardToTableau(Card c, Card destinationCard, int column)
    {
        MyVisualTableHandler.MoveCardToTableau(c, column);
    }

    public Card[] GetTableauLastCards()
    {
        return MyGameHandler.GetTableauLastCards();
    }
    #endregion

    #region Foundation
    public void MoveCardToFoundation(Card c, Card destinationCard, int column)
    {
        MyVisualTableHandler.MoveCardToFoundation(c, column);
    }

    public Card[] GetFoundationLastCards()
    {
        return MyGameHandler.GetFoundationLastCards();
    }
    #endregion

    #region Waste

    public void MoveCardToWaste(Card c, Card destinationCard)
    {
        MyVisualTableHandler.MoveCardToWaste(c);
    }

    public Card GetWasteLastCard()
    {
        return MyGameHandler.GetWasteLastCard();
    }
    #endregion

    #region Deck

    public void MoveCardToDeck(Card c)
    {
        MyVisualTableHandler.MoveCardToDeck(c);
    }

    #endregion
    public void ExecuteMove(Move m)
    {
        MyGameHandler.ExecuteMove(m);
    }

    public void TryFoundationMove(Card c, int from)
    {
        MyGameHandler.TryFoundationMove(c, from);
    }

    public void DrawFromDeck()
    {
        MyGameHandler.DrawFromDeck();
    }
}
