using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCardsHandler : MonoBehaviour
{
    public GameObject CardPrefab;
    public InterfaceController MyInterfaceController;

    private MoveGenerator MyMoveGenerator = new MoveGenerator();

    private List<GameObject> MyCards = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 300f))
            {
                if(hit.transform.CompareTag("Card"))
                    hit.transform.GetComponent<CardImage>().CardSelected();
                else if (hit.transform.CompareTag("Deck"))
                {
                    MyInterfaceController.DrawFromDeck();
                }

            }
        }
    }

    /// <summary>
    /// Generates the cards visually. Call this BEFORE shuffling the deck. This way the indexes in the lists are aligned
    /// </summary>
    /// <param name="cards"></param>
    public void GenerateCards(List<Card> cards)
    {
        foreach (Card c in cards)
        {
            GameObject tmp = Instantiate(CardPrefab,transform);
            tmp.GetComponent<CardImage>().SetupCardVisually(c.Number, c.Suit, c, gameObject);
            //tmp.GetComponent<CardImage>().TargetPosition = transform.position;
            tmp.GetComponent<CardImage>().TargetTransform = transform;
            MyCards.Add(tmp);
        }

    }

    public void FlipCard(int id)
    {
        MyCards[id].GetComponent<CardImage>().Flip();
    }

    public void MoveCard(int id, Transform destination, bool isOnTableau)
    {
        MyCards[id].GetComponent<CardImage>().TargetTransform = destination;
        MyCards[id].GetComponent<CardImage>().IsOnTableau = isOnTableau;
    }

    public Transform GetNextCardPosition(int id)
    {
        return MyCards[id].GetComponent<CardImage>().NextCardPosition;
    }

    public void CardSelected(Card c, int tableSlot)
    {
        if (!c.IsFaceDown)
            if (MyMoveGenerator.AddCardToMove(c, tableSlot))
            {
                Move m = MyMoveGenerator.GetMove();
                if (m.From == m.To)
                {
                    MyInterfaceController.TryFoundationMove(c, m.From);
                }
                else
                {
                    MyInterfaceController.ExecuteMove(m);
                }
            }
            
    }

}
