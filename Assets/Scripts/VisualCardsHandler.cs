using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCardsHandler : MonoBehaviour
{
    public GameObject CardPrefab;

    private List<GameObject> MyCards = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            tmp.GetComponent<CardImage>().SetupCardVisually(c.Number, c.Suit);
            MyCards.Add(tmp);
        }

        //DEBUG
        MoveCardsDebug();
    }

    public void FlipCard(int id)
    {
        MyCards[id].GetComponent<CardImage>().Flip();
    }

    private void MoveCardsDebug()
    {
        for(int i = 0; i< MyCards.Count; i++)
        {
            MyCards[i].transform.position = new Vector2((i % 13) * 2, (i / 13) * 2);
        }
    }

}
