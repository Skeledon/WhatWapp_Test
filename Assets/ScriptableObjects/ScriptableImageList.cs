using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableImageList : ScriptableObject
{
    [SerializeField]
    private Sprite[] Images;

    public Sprite GetImage(int index)
    {
        return Images[index];
    }
}
