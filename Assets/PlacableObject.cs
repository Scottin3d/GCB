using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacableObject : MonoBehaviour
{
    [SerializeField] List<HeightAdjustOnPlace> heightAdjustChildren;
    [SerializeField] Transform visual;
    [SerializeField] Material selectionMaterial;
    
    // Start is called before the first frame update
    void Awake()
    {
        heightAdjustChildren = new List<HeightAdjustOnPlace>();
        for (int i = 0; i < visual.childCount; i++)
        {
            visual.GetChild(i).TryGetComponent<HeightAdjustOnPlace>(out HeightAdjustOnPlace child);
            if (child != null) {
                heightAdjustChildren.Add(child);
            }
        }
    }

    public void AdjustChildren() {
        foreach (var child in heightAdjustChildren)
        {
            child.AdjustHeight();
        }
    }

    public void SetSelectionColor(Color color) {
        selectionMaterial.SetColor("_Color", color);
    }
}
