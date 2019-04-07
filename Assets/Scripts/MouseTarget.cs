using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTarget : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {

        Vector3 newPos;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        newPos = new Vector3(mousePos.x, mousePos.y, -190);
        transform.position = newPos;
    }
}
