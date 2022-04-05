using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherEffected : MonoBehaviour
{
    [SerializeField] WeatherSO weather;
    [SerializeField] Material mat;
    [SerializeField] Transform visualRoot;
    private void OnEnable()
    {
        Debug.Assert(weather != null);
        for (int i = 0; i < visualRoot.childCount; i++)
        {
            mat = visualRoot.GetChild(i).GetComponent<MeshRenderer>().material;
            mat.SetTexture("_WindMap", weather.WindMap);
            mat.SetFloat("_WindStrength", weather.WindStrenght);
            mat.SetVector("_WindDirection", weather.WindDirection);
        }
    }
}
