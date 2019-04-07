using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public InterfaceController MyInterfaceController;
    public PointsHandler MyPointsHandler;
    public TimeHandler MyTimeHandler;
    public MoveCountHandler MyMoveCountHandler;

    private Deck MyDeck = new Deck();
    private TableHandler MyTableHandler = new TableHandler();
    private MoveLog MyMoveLog = new MoveLog();

    public int TableauIndex { get; private set; }
    public int FoundationIndex { get; private set; }
    public int DeckIndex { get; private set; }
    public int WasteIndex { get; private set; }

    private bool isGameStarted = false;

    #region Initialization
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        SetupIndexes();
        StartGame();
    }

    private void SetupIndexes()
    {
        TableauIndex = 0;
        FoundationIndex = TableHandler.TABLEAU_SLOTS;
        DeckIndex = TableHandler.FOUNDATION_SLOTS + FoundationIndex;
        WasteIndex = DeckIndex + 1;
    }

    private void StartGame()
    {
        MyDeck.CreateDeck();

        MyInterfaceController.GenerateTableauSlots(MyTableHandler.GetTableauSlots());
        MyInterfaceController.GenerateFoundationSlots(MyTableHandler.GetFoundationSlots());
        MyInterfaceController.GenerateWasteSlot(MyTableHandler.GetWasteSlot());

        MyInterfaceController.GenerateCards(MyDeck.GetCardList());

        MyDeck.ShuffleDeck();
        DealCards();
        FlipLastTableauCards();
        isGameStarted = true;


    }

    private void DealCards()
    {
        for (int i = 0; i < TableHandler.TABLEAU_SLOTS; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                ExecuteMove(new Move(MyDeck.DrawFirstCard(), DeckIndex, TableauIndex + i, true));
            }
        }
    }
    #endregion
    #region Deck
    public void DrawFromDeck()
    {
        if (MyDeck.CardsRemaining > 0)
            ExecuteMove(new Move(MyDeck.DrawFirstCard(), DeckIndex, WasteIndex, true));
        else
            RollWasteToDeck();
    }

    private void RollWasteToDeck()
    {
        while(MyTableHandler.GetWasteLastCard().ID >= 0)
        {
            ExecuteMove(new Move(MyTableHandler.GetWasteLastCard(), WasteIndex, DeckIndex, true, true), false, true);
        }
        MyPointsHandler.ChangePoints(PointsHandler.RECYCLE);
        MyMoveCountHandler.AddMove();
    }
    #endregion
    #region MovesExecution
    public void UndoMove()
    {
        Move m;
        m = MyMoveLog.UndoMove();
        if (m != null)
        {
            ExecuteMove(m, true, m.RollBack);
        }
        if (MyMoveLog.GetLastMove() != null)
            if (m.RollBack && MyMoveLog.GetLastMove().RollBack)
                UndoMove();
    }

    public void TryFoundationMove(Card c, int from)
    {
        ExecuteMove(new Move(c, from, FoundationIndex + c.Suit, false));
    }


    public void ExecuteMove(Move m)
    {
        ExecuteMove(m, false, false);
    }

    public void ExecuteMove(Move m, bool undoMove, bool rollBack)
    {
        bool validMove = false;
        if (undoMove)
            FlipBackTableauCards(m);
        //Move to Tableau
        if(m.To < FoundationIndex)
        {
            Card ret = MyTableHandler.MoveCardToTableauColumn(m.MovedCard, m.To, m.IsForced);
            if (ret != null)
            {
                RemoveCardFromPreviousPosition(m, undoMove);
                MyInterfaceController.MoveCardToTableau(m.MovedCard, ret, m.To);
                validMove = true;


            }
        }
        //Move to foundation
        else if (m.To < DeckIndex)
        {
            Card ret = MyTableHandler.MoveCardToFoundationColumn(m.MovedCard, m.To - FoundationIndex, m.IsForced);
            if (ret != null)
            {
                RemoveCardFromPreviousPosition(m, undoMove);
                MyInterfaceController.MoveCardToFoundation(m.MovedCard, ret, m.To - FoundationIndex);
                validMove = true;

            }
        }
        //Move to deck
        else if (m.To < WasteIndex)
        {
            MyDeck.AddCardOnTopOfDeck(m.MovedCard);
            RemoveCardFromPreviousPosition(m, undoMove);
            MyInterfaceController.MoveCardToDeck(m.MovedCard);
            MyInterfaceController.FlipCard(m.MovedCard.ID);
            validMove = true;

        }
        //Move to waste
        else
        {

            Card ret = MyTableHandler.MoveCardToWaste(m.MovedCard, m, DeckIndex, m.IsForced);
            if (ret != null)
            {
                RemoveCardFromPreviousPosition(m, undoMove);
                MyInterfaceController.MoveCardToWaste(m.MovedCard, ret);
                validMove = true;
            }
            
        }



        if (isGameStarted && !undoMove && validMove)
        {

            FlipLastTableauCards(m);

            MyMoveLog.LogMove(m);
        }
        if (isGameStarted)
        {
            FlipLastWasteCard();

            //In case of UndoMove the start and the destination are inverted.
            //This is because the points need to be calculated on the original move direciton, not the undo
            if (undoMove)
                SetPointsForMove(m, undoMove, m.To, m.From);
            else
                SetPointsForMove(m, undoMove, m.From, m.To);
            //The moves are counted here because undos count as a move
            if (validMove && !rollBack)
                MyMoveCountHandler.AddMove();
        }

    }

    private void RemoveCardFromPreviousPosition(Move m)
    {
        RemoveCardFromPreviousPosition(m, false);
    }

    private void RemoveCardFromPreviousPosition(Move m, bool undoMove)
    {
        Card c = m.MovedCard;
        int from = m.From;
        //Tableau move
        if (from < FoundationIndex)
        {
            MyTableHandler.RemoveCardFromTableau(c, from);
        }
        //Foundation move
        else if (from < DeckIndex)
        {
            MyTableHandler.RemoveCardFromFoundation(c, from - FoundationIndex);
        }
        //Deck move
        else if (from < WasteIndex)
        {
            if(undoMove)
                MyDeck.RemoveCard(c);
        }
        //Waste move
        else
        {
            MyTableHandler.RemoveCardFromWaste(c);
        }

    }

    #endregion
    #region CardGetter
    public Card[] GetTableauLastCards()
    {
        return MyTableHandler.GetTableauLastCards();
    }

    public Card[] GetFoundationLastCards()
    {
        return MyTableHandler.GetFoundationLastCards();
    }

    public Card GetWasteLastCard()
    {
        return MyTableHandler.GetWasteLastCard();
    }
    #endregion
    #region FlipHandlers
    private void FlipLastTableauCards()
    {
        FlipLastTableauCards(null);
    }

    private void FlipLastTableauCards(Move m)
    {
        Card[] lastTableauCard = GetTableauLastCards();
        for(int i = 0; i < lastTableauCard.Length; i++)
        {
            Card c = lastTableauCard[i];
            if (c.IsFaceDown && c.ID >= 0)
            {
                c.FlipCard();
                MyInterfaceController.FlipCard(c.ID);
                if(m!= null)
                    m.FlipTableau(i);
                if(isGameStarted)
                    MyPointsHandler.ChangePoints(PointsHandler.FLIP_TABLEAU);
            }
        }
    }   

    private void FlipBackTableauCards(Move m)
    {
        if(m.TableauFlipped >=0)
        {
            Card[] lastTableauCard = GetTableauLastCards();
            int n = m.TableauFlipped;
            lastTableauCard[n].FlipCard();
            MyInterfaceController.FlipCard(lastTableauCard[n].ID);
            MyPointsHandler.ChangePoints(-PointsHandler.FLIP_TABLEAU);
        }
    }

    private void FlipLastWasteCard()
    {
        Card c = GetWasteLastCard();
        {
            if(c.IsFaceDown && c.ID >= 0)
            {
                c.FlipCard();
                MyInterfaceController.FlipCard(c.ID);
            }
        }
    }

    #endregion
    #region Points

    private void SetPointsForMove(Move m, bool undo, int from, int to)
    {
        int mult = undo ? -1 : 1;
        int points = 0;

        if (isGameStarted)
        {
            if (to < FoundationIndex)
            {
                if (from == WasteIndex)
                    points = PointsHandler.WASTE_TO_TABLEAU;
                else if (from < DeckIndex && from >= FoundationIndex)
                    points = PointsHandler.FOUNDATION_TO_TABLEAU;
            }
            else if (to < DeckIndex)
            {
                if (from < FoundationIndex)
                    points = PointsHandler.TABLEAU_TO_FOUNDATION;
                else if (from == WasteIndex)
                    points = PointsHandler.WASTE_TO_FOUNDATION;

            }
            points *= mult;
            MyPointsHandler.ChangePoints(points);
        }
    }
    #endregion
}
