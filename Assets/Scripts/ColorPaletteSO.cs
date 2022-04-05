using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ColorPaletteSO : ScriptableObject
{
    public Color[] Palette;

    public int Count => Palette.Length;
}
