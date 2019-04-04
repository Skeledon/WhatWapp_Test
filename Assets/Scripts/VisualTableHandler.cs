using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualTableHandler : MonoBehaviour
{
    public GameObject[] TableauColumns;
    public GameObject[] Foundations;
    public GameObject Deck;

    public VisualCardsHandler MyVisualCardHandler;
    public InterfaceController MyInterfaceController;

    private Vector2[] TableauNextPositions = new Vector2[TableHandler.TABLEAU_SLOTS];
    private Vector2[] FoundationNextPositions = new Vector2[TableHandler.FOUNDATION_SLOTS];

    // Start is called before the first frame update
    void Start()
    {
        UpdateNextPositions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateNextPositions()
    {
        Card[] lasts = MyInterfaceController.GetTableauLastCards();

        for(int i = 0; i < lasts.Length; i++)
        {
            if(lasts[i].ID < 0)
            {
                TableauNextPositions[i] = TableauColumns[i].transform.position;
            }
            else
            {
                TableauNextPositions[i] = MyVisualCardHandler.GetNextCardPosition(lasts[i].ID);
            }
        }

    }

    public void MoveCardToTableau(Card c, int column)
    {
        MyVisualCardHandler.MoveCard(c.ID, TableauNextPositions[column]);
        UpdateNextPositions();
    }
}
