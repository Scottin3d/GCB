using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColor : MonoBehaviour
{
    public ColorPaletteSO colorPallete;
    [SerializeField] private Material mat;
    [SerializeField] Transform visualRoot;

    private void OnEnable()
    {
        mat = Instantiate(mat);
        mat.SetColor("_Color", colorPallete.Palette[Random.Range(0, colorPallete.Count)]);

        for (int i = 0; i < visualRoot.childCount; i++)
        {
            visualRoot.GetChild(i).GetComponent<MeshRenderer>().material = mat;
        }
    }

    
}
