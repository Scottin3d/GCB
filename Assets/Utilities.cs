using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static Vector3 MouseToTerrainPosition(LayerMask layerMask)
    {
        Vector3 position = Vector3.zero;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit info, 100, layerMask))
            position = info.point;
        return position;
    }
    public static Vector3 MouseToTerrainPosition()
    {
        Vector3 position = Vector3.zero;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit info, 100))
            position = info.point;
        return position;
    }
    public static RaycastHit CameraRay()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit info, 100))
            return info;
        return new RaycastHit();
    }

    public static Vector3 RaycastDown(Vector3 origin, LayerMask layerMask) {

        Physics.Raycast(origin + new Vector3(0f, 100f, 0f), -Vector3.up, out RaycastHit hit, 1000, layerMask);
        Debug.DrawLine(origin, hit.point, Color.red, 10f);
        return hit.point;
    }
}