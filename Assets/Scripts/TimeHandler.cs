using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    public UnityEngine.UI.Text MyText;
    private float time;
    private int seconds;
    private int minutes;

    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        string str;
        time += Time.deltaTime;
        seconds = (int)(time % 60);
        minutes = (int)(time / 60);
        str = string.Format("{0:00} : {1:00}", minutes, seconds);
        MyText.text = str;
    }
}
