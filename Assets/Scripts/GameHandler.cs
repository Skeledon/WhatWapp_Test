using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public InterfaceController MyInterfaceController;

    private Deck MyDeck = new Deck();

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartGame()
    {
        MyDeck.CreateDeck();


        MyInterfaceController.GenerateCards(MyDeck.GetCardList());

        MyDeck.ShuffleDeck();
        StartCoroutine(DebugFlipTimer());
    }

    private IEnumerator DebugFlipTimer()
    {
        for (int i = 0; i < 52; i++)
        {
            yield return new WaitForSeconds(.1f);
            MyInterfaceController.FlipCard(i);
            Debug.Log("flip");
        }
    }
}
