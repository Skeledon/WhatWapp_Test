using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableHandler : MonoBehaviour
{

    private const int TABLEAU_SLOTS = 7;
    private const int FOUNDATION_SLOTS = 4;
    private TableSlot[] Tableau = new TableSlot[TABLEAU_SLOTS];
    private TableSlot[] Foundation = new TableSlot[FOUNDATION_SLOTS];
    private TableSlot Waste = new TableSlot();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i< TABLEAU_SLOTS; i++)
        {
            Tableau[i] = new TableSlot();
        }

        for (int i = 0; i < FOUNDATION_SLOTS; i++)
        {
            Foundation[i] = new TableSlot();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AddCardToTableauColumn(Card c, int column)
    {
        if (column >= TABLEAU_SLOTS)
            return false;
        if (!CheckValidityOfMove(c, column))
            return false;
        AddCard(c, column);
        return true;

    }

    private bool CheckValidityOfMove(Card c, int column)
    {
        return true;
    }

    private void AddCard(Card c, int column)
    {

    }
}
