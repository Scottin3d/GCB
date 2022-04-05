using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceVolume : MonoBehaviour
{
    [SerializeField] Collider volume;
    public Collider Volume => volume;
    [SerializeField]
    public int Radius = 5;
}
