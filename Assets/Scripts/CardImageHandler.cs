using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardImageHandler : MonoBehaviour
{
    [Header("Scriptables")]
    public ScriptableImageList Suits;
    public ScriptableImageList CardNumbers;
    public ScriptableImageList BackGrounds;

    [Header("Main Card Images")]
    public SpriteRenderer CardFront;
    public SpriteRenderer CardBack;

    [Header("Card Details")]
    public SpriteRenderer MainSuit;
    public SpriteRenderer SmallSuit;
    public SpriteRenderer Number;

    [Header("Colors")]
    public Color Red;
    public Color Black;

    // Start is called before the first frame update
    void Start()
    {
        SetupCardVisually(Random.Range(0,14), Random.Range(0,4));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupCardVisually(int number, int suit)
    {
        //Initializes the background of both sides
        CardBack.sprite = BackGrounds.GetImage(1);
        CardFront.sprite = BackGrounds.GetImage(0);

        //Initializes the correct suit image
        MainSuit.sprite = Suits.GetImage(suit);
        SmallSuit.sprite = Suits.GetImage(suit);

        //Initializes the correct number
        Number.sprite = CardNumbers.GetImage(number);
        //suit < 2 means clubs or diamonds, hence the number whould be red. Otherwise it will be black
        if (suit < 2)
            Number.color = Red;
        else
            Number.color = Black;
    }
}
