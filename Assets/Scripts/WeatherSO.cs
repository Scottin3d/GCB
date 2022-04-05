using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class WeatherSO : ScriptableObject
{
    [SerializeField] Vector2 windDirection;
    public Vector2 WindDirection => windDirection;
    [Range(0f, 2f)]
    [SerializeField] float windStrength = 0.5f;
    public float WindStrenght => Mathf.Clamp(windStrength, 0f, 2f);
    [SerializeField] Texture2D windMap;
    public Texture2D WindMap => windMap;
}
