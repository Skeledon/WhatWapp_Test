using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInLayerSetByZ : MonoBehaviour
{
    public int StepPerYUnit = 10;
    public int AdditionalStep = 0;

    private SpriteRenderer myRenderer;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        myRenderer.sortingOrder = (int)(-transform.position.z * StepPerYUnit) + AdditionalStep;
    }
}
