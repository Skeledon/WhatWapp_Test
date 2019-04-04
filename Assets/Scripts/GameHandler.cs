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


        MyInterfaceController.GenerateCards(MyDeck.GetCardList());

        MyDeck.ShuffleDeck();
        DealCards();

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
        if(m.To < FoundationIndex)
        {
            Card ret = MyTableHandler.MoveCardToTableauColumn(m.MovedCard, m.To, m.IsForced);
            if (ret != null)
            {
                MyInterfaceController.MoveCardToTableau(m.MovedCard, ret, m.To);
            }
        }
        else if (m.To < DeckIndex)
        { 

        }
        else if (m.To < WasteIndex)
        {

        }
        else
        {

        }
        MyMoveLog.LogMove(m);
    }

    public Card[] GetTableauLastCards()
    {
        return MyTableHandler.GetTableauLastCards();
    }

    
}
