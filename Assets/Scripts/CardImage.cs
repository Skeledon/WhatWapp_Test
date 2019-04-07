using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardImage : MonoBehaviour
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

    private Animator MyAnimator;
    private VisualCardsHandler MyHandler;

    private int currentTableSlot;

    public Card MyLinkedCard { get; private set; }
    public Transform NextCardPosition;
    public Transform[] NextPositions;

    public Transform TargetTransform;

    private Vector3 velocity;
    private float smoothTime = .05f;

    private bool isSlot = false;
    public bool IsOnTableau;
    public bool IsPickedUpByMouse = false;

    private bool doubleClickReady = false;
    private const float TIME_FOR_DOUBLECLICK = .2f;

    // Start is called before the first frame update
    void Awake()
    {
        MyAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Keep the card on the correct position
        if (MyLinkedCard.IsSlot || IsPickedUpByMouse)
        {
            transform.position = TargetTransform.position;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, TargetTransform.position, ref velocity, smoothTime);
        }

        //Set the correct position for the next card
        if (isSlot || !IsOnTableau)
        {
            NextCardPosition.position = NextPositions[1].position;
        }
        else
        {
            NextCardPosition.position = NextPositions[0].position;
        }
    }

    public void CardSelected(Transform mouseTarget)
    {
        if (isSlot)
            return;
        TargetTransform = mouseTarget;
        if (!doubleClickReady)
            MyHandler.CardSelected(MyLinkedCard, currentTableSlot);
        else
            MyHandler.CardDoubleClicked(MyLinkedCard, currentTableSlot);
        StartCoroutine(DoubleClickTimer());
    }

    private IEnumerator DoubleClickTimer()
    {
        doubleClickReady = true;
        yield return new WaitForSeconds(TIME_FOR_DOUBLECLICK);
        doubleClickReady = false;
    }

    public int ReleaseCard()
    {
        return currentTableSlot;
    }

    public void RevertToOldTarget(Transform t)
    {
        TargetTransform = t;
    }

    public void SetupCardVisually(int number, int suit, Card c, GameObject handler)
    {
        if (number == 14 || number <= 0)
        {
            CardBack.sprite = BackGrounds.GetImage(2);
            CardFront.sprite = BackGrounds.GetImage(2);
            isSlot = true;
            if(number == 0)
            {
                MainSuit.sprite = Suits.GetImage(suit);
            }
        }
        else
        {
            //Initializes the background of both sides
            CardBack.sprite = BackGrounds.GetImage(1);
            CardFront.sprite = BackGrounds.GetImage(0);

            //Initializes the correct suit image
            MainSuit.sprite = Suits.GetImage(suit);
            SmallSuit.sprite = Suits.GetImage(suit);

            //Initializes the correct number (it needs the -1 because card numbers go from 1 to 13, arrays start at 0)
            Number.sprite = CardNumbers.GetImage(number - 1);
            //suit < 2 means clubs or diamonds, hence the number whould be red. Otherwise it will be black
            if (suit < 2)
                Number.color = Red;
            else
                Number.color = Black;
        }
        MyLinkedCard = c;
        MyHandler = handler.GetComponent<VisualCardsHandler>();
    }

    public void Flip()
    {
        MyAnimator.SetTrigger("Flip");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("TableSlot"))
        {
            currentTableSlot = other.GetComponent<TableSlot>().MyIndex;
        }
    }
}
