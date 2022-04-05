using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
using CodeMonkey;
public class SelectionManager : MonoBehaviour
{
    TextMesh text;


    public PlacableObject testPrefab;
    public PathMapping pathMapping;

    [SerializeField] Transform selectionOutline = default;
    bool dragging;
    Vector3 startDrag;
    Vector3 endDrag;
    Vector3 dragCenter;
    Vector3 dragSize;

    private bool isBuilding = false;
    // Start is called before the first frame update
    void Start()
    {
            selectionOutline.gameObject.SetActive(isBuilding);

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.B)) {
            isBuilding = !isBuilding;
            //selectionOutline.gameObject.SetActive(isBuilding);
        }

        if (isBuilding)
        {
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
                            out RaycastHit hit, 999f, LayerMask.GetMask("Ground"));
            //selectionOutline.transform.position = hit.point;

            testPrefab.transform.position = pathMapping.GetWorldPosition(hit.point, true);
            pathMapping.GetXZ(testPrefab.transform.position, out int x, out int z);
            //text = CodeMonkey.Utils.UtilsClass.CreateWorldText(string.Format("Selection({0}, {1})", x, z), null, pathMapping.CachedMapCells[x, z].Center + new Vector3(0f, 5f, 0f), 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
            
            if (pathMapping.CachedMapCells[x, z].Slope > 5)
            {
                testPrefab.SetSelectionColor(Color.red);
            }
            else { 
                testPrefab.SetSelectionColor(Color.green);

            }
            testPrefab.AdjustChildren();
        }
    }
}
