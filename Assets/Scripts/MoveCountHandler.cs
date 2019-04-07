using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCountHandler : MonoBehaviour
{
    public UnityEngine.UI.Text MyText;
    private int moveCount;

    public void AddMove()
    {
        moveCount++;
        MyText.text = moveCount.ToString();
    }
}
