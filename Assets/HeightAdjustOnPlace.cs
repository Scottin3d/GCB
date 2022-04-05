using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightAdjustOnPlace : MonoBehaviour
{
    public void AdjustHeight() {

        float yPos = Utility.RaycastDown(transform.position, LayerMask.GetMask("Ground")).y;
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}
