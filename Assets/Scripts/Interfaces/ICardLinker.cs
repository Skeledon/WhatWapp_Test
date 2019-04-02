using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardLinker 
{
    void AddCard(Card c);
    Card NextCard();
}
