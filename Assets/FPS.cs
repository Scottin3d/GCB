using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPS : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    [SerializeField] private float hudRefreshRate = 1f;

    private float timer;

    private void Update()
    {
        if (Time.unscaledTime > timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            text.text = "FPS: " + fps;
            timer = Time.unscaledTime + hudRefreshRate;
        }
    }
}
