using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using TMPro;
public class PathMapping : MonoBehaviour
{
    [SerializeField] private float decayAmount = 0.0001f;
    public static int pathMapResolution = 243;
    public int mapResolution = pathMapResolution;
    private float pathMapCell;
    public float PathMapCell => pathMapCell;
    private static float[,] pathOpactityMap;
    public static float[,] PathOpacityMap => pathOpactityMap;

    static Material mat;
    static bool decayPaths = true;

    private MapCell[,] cachedMapCells;
    public MapCell[,] CachedMapCells => cachedMapCells;
    private Vector3[,] mapHeightCache;
    private Vector3[,] mapVertexCache;

    // Start is called before the first frame update
    void Awake()
    {
        cachedMapCells = new MapCell[pathMapResolution, pathMapResolution];
        mapHeightCache = new Vector3[pathMapResolution, pathMapResolution];
        //mapVertexCache = GetComponent<MeshFilter>().mesh.vertices;
        mapVertexCache = new Vector3[pathMapResolution, pathMapResolution];
        
        mat = GetComponent<MeshRenderer>().material;
        pathOpactityMap = new float[pathMapResolution, pathMapResolution];
        pathMapCell = 32f / pathMapResolution;

        CacheMapValues();
        //StartCoroutine(CacheMapValueCoroutine());
    }

    public void CacheValues() {
            cachedMapCells = new MapCell[pathMapResolution, pathMapResolution];
            mapHeightCache = new Vector3[pathMapResolution, pathMapResolution];
            //mapVertexCache = GetComponent<MeshFilter>().mesh.vertices;
            mapVertexCache = new Vector3[pathMapResolution, pathMapResolution];

            mat = GetComponent<MeshRenderer>().sharedMaterial;
            pathOpactityMap = new float[pathMapResolution, pathMapResolution];
            pathMapCell = 243f / pathMapResolution;

            CacheMapValues();
    }

    private void Start()
    {
        StartCoroutine(FadeMap());

        DrawDebug();
    }

    private void CacheMapValues() {
        for (int x = 0; x < pathMapResolution; x++)
        {
            for (int z = 0; z < pathMapResolution; z++)
            {
                Vector3 v = GetWorldPositionCellCenter(x, z);
                Physics.Raycast(v + new Vector3(x, 100f, z), -Vector3.up, out RaycastHit hitCenter, 999f, LayerMask.GetMask("Ground"));
                mapHeightCache[x, z] = hitCenter.point;

                Physics.Raycast(new Vector3(x, 100f, z), -Vector3.up, out RaycastHit hitCorner, 999f, LayerMask.GetMask("Ground"));
                mapVertexCache[x, z] = hitCorner.point;

                cachedMapCells[x, z] = new MapCell(hitCenter.point, hitCorner.point, hitCenter.normal, x, z);
            }
        }
    }

    IEnumerator CacheMapValueCoroutine() {
        for (int x = 0; x < pathMapResolution; x++)
        {
            for (int z = 0; z < pathMapResolution; z++)
            {
                Vector3 v = GetWorldPositionCellCenter(x, z);
                Physics.Raycast(v + new Vector3(x, 100f, z), -Vector3.up, out RaycastHit hitCenter, 999f, LayerMask.GetMask("Ground"));
                mapHeightCache[x, z] = hitCenter.point;

                Physics.Raycast(new Vector3(x, 100f, z), -Vector3.up, out RaycastHit hitCorner, 999f, LayerMask.GetMask("Ground"));
                mapVertexCache[x, z] = hitCorner.point;
                Debug.DrawLine(hitCenter.point, v + new Vector3(x, 100f, z), Color.blue, 99f);
                cachedMapCells[x, z] = new MapCell(hitCenter.point, hitCorner.point, hitCenter.normal, x, z);

                yield return new WaitForEndOfFrame();
            }
        }
    }


    public static void UpdatePathMask(float[,] mask) {
        if (mask.GetLength(0) != pathOpactityMap.GetLength(0)) { return; }
        Color[] pixels = new Color[pathMapResolution * pathMapResolution];
        for (int i = 0; i < pathOpactityMap.GetLength(0); i++)
        {
            for (int j = 0; j < pathOpactityMap.GetLength(1); j++)
            {
                float grey = Mathf.Clamp01(pathOpactityMap[i, j] + mask[i, j]);
                pathOpactityMap[i, j] = grey;
                pixels[i + j * pathMapResolution] = new Color(grey, grey, grey);
            }
        }

        Texture2D pathMask = new Texture2D(pathMapResolution, pathMapResolution);
        pathMask.SetPixels(pixels);
        pathMask.filterMode = FilterMode.Bilinear;
        pathMask.Apply();
        mat.SetTexture("PathMask", pathMask);
    }

    IEnumerator FadeMap() {
        while (decayPaths) {
            for (int i = 0; i < pathOpactityMap.GetLength(0); i++)
            {
                for (int j = 0; j < pathOpactityMap.GetLength(1); j++)
                {
                    if (pathOpactityMap[i, j] < 1f) {
                        pathOpactityMap[i, j] = Mathf.Clamp01(pathOpactityMap[i, j] - decayAmount);
                    }
                }
            }

            yield return new WaitForSeconds(1 / 30f);
        }
    }

    public void GetXZ(Vector3 pos, out int x, out int z)
    {
        x = Mathf.FloorToInt(pos.x / pathMapCell);
        z = Mathf.FloorToInt(pos.z / pathMapCell);
    }

    public Vector3 GetWorldPositionCellCenter(int x, int z) {
        return cachedMapCells[x, z].Center;
    }
    public Vector3 GetWorldPositionCellCenter(Vector3 position)
    {
        GetXZ(position, out int x, out int z);
        return cachedMapCells[x, z].Center;
    }

    public Vector3 GetWorldPosition(int x, int z, bool height = false) {
        float posX = x * pathMapCell;
        float posZ = z * pathMapCell;
        Vector3 pos = Vector3.zero;

        if (height) {
            pos = mapVertexCache[x, z];
        }
        else { 
            pos = new Vector3(posX, 0f, posZ);
        }
        return pos;
    }

    public Vector3 GetWorldPosition(Vector3 position, bool height = false)
    {
        
        GetXZ(position, out int x, out int z);
        Vector3 pos = Vector3.zero;

        if (height)
        {
            pos = mapVertexCache[x, z];
        }
        else
        {
            pos = cachedMapCells[x, z].Corner;
        }
        return pos;
    }

    public void DrawDebug()
    {

        for (int x = 1; x < pathMapResolution; x++)
        {
            for (int z = 1; z < pathMapResolution; z++)
            {
                
                Debug.DrawLine(cachedMapCells[x, z].Corner, cachedMapCells[x, z - 1].Corner, Color.white, 999f);
                Debug.DrawLine(cachedMapCells[x, z].Corner, cachedMapCells[x - 1, z].Corner, Color.white, 999f);
            }
        }
    }

    
}

public struct MapCell
{
    public MapCell(Vector3 _center, Vector3 _corner, Vector3 _normal, int _x, int _z)
    {
        Center = _center; Corner = _corner; Normal = _normal; Slope = Vector3.Angle(Vector3.up, Normal); X = _x; Z = _z;
    }

    public Vector3 Center;
    public Vector3 Corner;
    public Vector3 Normal;
    public float Slope;
    public int X;
    public int Z;
};
