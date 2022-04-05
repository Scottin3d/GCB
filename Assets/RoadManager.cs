using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    Vector3 startPosition = default;
    Vector3 endPosition = default;
    public bool isBuilding = false;
    public bool isActive = false;

    [SerializeField] LineRenderer road;
    [SerializeField] PathMapping pathMapping;
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R)) {
            isActive = !isActive;
            //road.enabled = isActive;

            if (!isActive) {
                return; 
            }
            road.positionCount = 2;

            startPosition = pathMapping.GetWorldPositionCellCenter(Utility.MouseToTerrainPosition());
            road.SetPosition(0, startPosition);
            road.SetPosition(1, startPosition);
        }

        if (isActive && Input.GetMouseButton(0)) {
            isBuilding = !isBuilding;
            if (isBuilding)
            {
                startPosition = pathMapping.GetWorldPositionCellCenter(Utility.MouseToTerrainPosition());
                road.SetPosition(0, startPosition);
            }
            else {
                isActive = false;
                endPosition = pathMapping.GetWorldPositionCellCenter(Utility.MouseToTerrainPosition());
                road.SetPosition(1, endPosition);
            }
        }

        if (isActive && isBuilding)
        {
            endPosition = pathMapping.GetWorldPositionCellCenter(Utility.MouseToTerrainPosition());
            road.SetPosition(1, endPosition);

        }
    }
}
