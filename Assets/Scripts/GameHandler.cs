using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public InterfaceController MyInterfaceController;

    private Deck MyDeck = new Deck();
    private TableHandler MyTableHandler = new TableHandler();
    private MoveLog MyMoveLog = new MoveLog();

    public int TableauIndex { get; private set; }
    public int FoundationIndex { get; private set; }
    public int DeckIndex { get; private set; }
    public int WasteIndex { get; private set; }

    private bool isGameStarted = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        SetupIndexes();
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        isGameStarted = true;
        FlipLastTableauCards();

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
            ExecuteMove(new Move(MyTableHandler.GetWasteLastCard(), WasteIndex, DeckIndex, true));
        }
    }

    public void TryFoundationMove(Card c, int from)
    {
        for(int i = 0; i < TableHandler.FOUNDATION_SLOTS; i++)
        {
            ExecuteMove(new Move(c, from, FoundationIndex + i, false));
        }
    }

    public void ExecuteMove(Move m)
    {
        //Move to Tableau
        if(m.To < FoundationIndex)
        {
            Card ret = MyTableHandler.MoveCardToTableauColumn(m.MovedCard, m.To, m.IsForced);
            if (ret != null)
            {
                RemoveCardFromPreviousPosition(m.MovedCard, m.From);
                MyInterfaceController.MoveCardToTableau(m.MovedCard, ret, m.To);
                MyMoveLog.LogMove(m);

            }
        }
        //Move to foundation
        else if (m.To < DeckIndex)
        {
            Card ret = MyTableHandler.MoveCardToFoundationColumn(m.MovedCard, m.To - FoundationIndex, m.IsForced);
            if (ret != null)
            {
                RemoveCardFromPreviousPosition(m.MovedCard, m.From);
                MyInterfaceController.MoveCardToFoundation(m.MovedCard, ret, m.To - FoundationIndex);
                MyMoveLog.LogMove(m);

            }
        }
        //Move to deck
        else if (m.To < WasteIndex)
        {
            MyDeck.AddCardOnTopOfDeck(m.MovedCard);
            RemoveCardFromPreviousPosition(m.MovedCard, m.From);
            MyInterfaceController.MoveCardToDeck(m.MovedCard);
            MyInterfaceController.FlipCard(m.MovedCard.ID);
            MyMoveLog.LogMove(m);
        }
        //Move to waste
        else
        {

            Card ret = MyTableHandler.MoveCardToWaste(m.MovedCard, m, DeckIndex, m.IsForced);
            if (ret != null)
            {
                RemoveCardFromPreviousPosition(m.MovedCard, m.From);
                MyInterfaceController.MoveCardToWaste(m.MovedCard, ret);
                MyMoveLog.LogMove(m);

            }
            
        }



        if (isGameStarted)
        {
            FlipLastTableauCards();
            FlipLastWasteCard();
        }


    }

    private void RemoveCardFromPreviousPosition(Card c, int from)
    {
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

        }
        //Waste move
        else
        {
            MyTableHandler.RemoveCardFromWaste(c);
        }

    }

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

    private void FlipLastTableauCards()
    {
        Card[] lastTableauCard = GetTableauLastCards();
        foreach(Card c in lastTableauCard )
        {
            if (c.IsFaceDown && c.ID >= 0)
            {
                c.FlipCard();
                MyInterfaceController.FlipCard(c.ID);
            }
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
}
