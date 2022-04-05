using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathObject : MonoBehaviour
{
    [SerializeField] private PathMapping pathMapping;

    [SerializeField] private float refreshRate = 30f;
    private int pathMapResolution;
    [SerializeField] private float pathMakingStrength = 0.2f;
    float[,] pathValues;

    private int previousX = -1;
    private int previousZ = -1;


    // Start is called before the first frame update
    void Start()
    {
        pathMapResolution = PathMapping.pathMapResolution;
        pathValues = new float[pathMapResolution, pathMapResolution];

        //StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath() {
        float time = Time.time;
        while (true) {
            pathMapping.GetXZ(transform.position, out int x, out int z);

            float v = pathValues[x, z];
            if (previousX != x || previousZ != z) {
                v += pathMakingStrength;

            }
            Debug.Log(pathValues[x, z]);
            pathValues[x, z] = Mathf.Clamp01(v);
            previousX = x;
            previousZ = z;

            if (Time.time - time > 0.25f) {
                Debug.Log("Updating Paths");

                PathMapping.UpdatePathMask(pathValues);
                pathValues = new float[pathMapResolution, pathMapResolution];
                time = Time.time;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
