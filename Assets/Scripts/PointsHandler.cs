using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsHandler : MonoBehaviour
{
    public UnityEngine.UI.Text PointsText;

    public const int WASTE_TO_TABLEAU = 5;
    public const int WASTE_TO_FOUNDATION = 10;
    public const int TABLEAU_TO_FOUNDATION = 10;
    public const int FLIP_TABLEAU = 5;
    public const int FOUNDATION_TO_TABLEAU = -15;
    public const int RECYCLE = -100;
    private int points;
    
    public void ChangePoints(int n)
    {
        points += n;
        points = Mathf.Clamp(points, 0, int.MaxValue);
        PointsText.text = points.ToString();
    }
}
