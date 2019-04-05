using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualTableHandler : MonoBehaviour
{
    public GameObject CardPrefab;

    public GameObject[] TableauColumns;
    public GameObject[] Foundations;
    public GameObject Deck;

    public List<GameObject> TableauSlots = new List<GameObject>();
    public List<GameObject> FoundationSlots = new List<GameObject>();

    public VisualCardsHandler MyVisualCardHandler;
    public InterfaceController MyInterfaceController;

    private Transform[] TableauNextPositions = new Transform[TableHandler.TABLEAU_SLOTS];
    private Transform[] FoundationNextPositions = new Transform[TableHandler.FOUNDATION_SLOTS];

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateNextTableauPositions()
    {
       
        Card[] lasts = MyInterfaceController.GetTableauLastCards();

        for (int i = 0; i < lasts.Length; i++)
        {
            if (lasts[i].ID < 0)
            {
                TableauNextPositions[i] = TableauSlots[i].GetComponent<CardImage>().NextCardPosition;
            }
            else
            {
                TableauNextPositions[i] = MyVisualCardHandler.GetNextCardPosition(lasts[i].ID);
            }
        }
    }
     private void UpdateNextFoundationPositions()
    {
        Card[] lasts = MyInterfaceController.GetFoundationLastCards();

        for (int i = 0; i < lasts.Length; i++)
        {
            if (lasts[i].ID < 0)
            {
                FoundationNextPositions[i] = FoundationSlots[i].GetComponent<CardImage>().NextCardPosition;
            }
            else
            {
                FoundationNextPositions[i] = MyVisualCardHandler.GetNextCardPosition(lasts[i].ID);
            }
        }

    }

    public void MoveCardToTableau(Card c, int column)
    {
        MyVisualCardHandler.MoveCard(c.ID, TableauNextPositions[column], true);
        UpdateNextTableauPositions();
        UpdateNextFoundationPositions();
    }

    public void MoveCardToFoundation(Card c, int column)
    {
        MyVisualCardHandler.MoveCard(c.ID, FoundationNextPositions[column], false);
        UpdateNextFoundationPositions();
        UpdateNextTableauPositions();
    }

    public void GenerateTableauSlotsCard(Card[] slots)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            GameObject tmp = Instantiate(CardPrefab, transform);
            tmp.GetComponent<CardImage>().SetupCardVisually(slots[i].Number, slots[i].Suit, slots[i], MyVisualCardHandler.gameObject);
            tmp.GetComponent<CardImage>().TargetTransform = TableauColumns[i].transform;
            TableauSlots.Add(tmp);
        }
        UpdateNextTableauPositions();
    }

    public void GenerateFoundationSlotsCard(Card[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            GameObject tmp = Instantiate(CardPrefab, transform);
            tmp.GetComponent<CardImage>().SetupCardVisually(slots[i].Number, slots[i].Suit, slots[i], MyVisualCardHandler.gameObject);
            tmp.GetComponent<CardImage>().TargetTransform = Foundations[i].transform;
            FoundationSlots.Add(tmp);
            tmp.GetComponent<CardImage>().Flip();
        }
        UpdateNextFoundationPositions();
    }
}
