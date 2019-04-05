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

    public void ExecuteMove(Move m)
    {
        //Tableau move
        if(m.To < FoundationIndex)
        {
            Card ret = MyTableHandler.MoveCardToTableauColumn(m.MovedCard, m.To, m.IsForced);
            if (ret != null)
            {
                RemoveCardFromPreviousPosition(m.MovedCard, m.From);
                MyInterfaceController.MoveCardToTableau(m.MovedCard, ret, m.To);


            }
        }
        //Foundation move
        else if (m.To < DeckIndex)
        {
            Card ret = MyTableHandler.MoveCardToFoundationColumn(m.MovedCard, m.To - FoundationIndex, m.IsForced);
            if (ret != null)
            {
                RemoveCardFromPreviousPosition(m.MovedCard, m.From);
                MyInterfaceController.MoveCardToFoundation(m.MovedCard, ret, m.To - FoundationIndex);


            }
        }
        //Deck move
        else if (m.To < WasteIndex)
        {

        }
        //Waste move
        else
        {

        }
        MyMoveLog.LogMove(m);



        if (isGameStarted)
            FlipLastTableauCards();


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
}
