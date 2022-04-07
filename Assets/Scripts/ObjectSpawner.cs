using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] protected Transform parent;
    [SerializeField] protected SceneObjectObjectPool objectPool;
    [Range(0f, 0.99f)]
    [SerializeField] private float density;
    protected float Density;
    [Range(0f, 90f)]
    [SerializeField] protected float slopeBias;
    [Range(0f, 1f)]
    [SerializeField] private float jitter;
    protected float Jitter = 0f;

    //[SerializeField] protected GameObject[] prefabsVariants;
    protected SceneObject[,] spawnedObjects;
    protected PathMapping pathMapping;

    private void Awake()
    {
        Density = Mathf.Clamp01(density);
    }
    private void Start()
    {
        Init(false);
        Spawn(false);
    }

    private void Init(bool init)
    {
        pathMapping = GetComponent<PathMapping>();
        if (!init) { pathMapping.CacheValues(); }
        Density = Mathf.Clamp01(density);
        Jitter = jitter * pathMapping.PathMapCell;
    }

    public virtual void Spawn(bool init) {
        Init(init);
        if (spawnedObjects != null) { RecycleObjects(); }

        float[,] map = PathMapping.PathOpacityMap;
        MapCell[,] cachedMapCells = pathMapping.CachedMapCells;
        spawnedObjects = new SceneObject[pathMapping.mapResolution, pathMapping.mapResolution];

        
        for (int x = 0; x < pathMapping.mapResolution; x++)
        {
            for (int z = 0; z < pathMapping.mapResolution; z++)
            {
                if (cachedMapCells[x, z].Center == Vector3.zero) { continue; }
                if (map[x, z] >= 0.1f) { continue; }
                if (Random.Range(0f, 1f) > Density) { continue; }

                float chance = Density;

                // denser in hills
                if (cachedMapCells[x, z].Slope < slopeBias) {
                    chance /= 3f;
                }

                // limits number of trees on flat land, ref build area
                float n = Mathf.PerlinNoise(x / (float)pathMapping.mapResolution * 20f, z / (float)pathMapping.mapResolution * 20f);
                if (cachedMapCells[x, z].Slope <= 5 
                    &&  n < 0.28f) { chance = Mathf.Clamp01(chance - 0.5f); }

                if (Random.Range(0f, 1f) > chance) { continue; }

                // jitter offset
                Vector3 jitterOffest = new Vector3(Random.Range(-Jitter, Jitter), 
                                                    0f, 
                                                    Random.Range(-Jitter, Jitter)) *2f;

                // vertical slope offset
                Vector3 slopeOffset = new Vector3(0f, cachedMapCells[x, z].Slope / 180f, 0f);

                SceneObject inst = objectPool.GetPrefabInstance(parent);

                inst.transform.position = pathMapping.GetWorldPositionCellCenter(x, z) + jitterOffest - slopeOffset;
                inst.transform.rotation = RandomRotation();
                inst.name = string.Format("Tree {0}: ({1}, {2}", x + z * map.GetLength(0), inst.transform.position.x, inst.transform.position.z);
                if (Random.Range(0, 2) == 0)
                {
                    inst.transform.localScale = new Vector3(-1f, inst.transform.localScale.y, -1f);
                }
                float yScale = Random.Range(0.85f, 1.5f);
                inst.transform.localScale = new Vector3(inst.transform.localScale.x, yScale, inst.transform.localScale.z) * Random.Range(1f, 1.95f);

                spawnedObjects[x, z] = inst;
                
            }
        }

    }

    public virtual void RecycleObjects() {
        for (int x = 0; x < spawnedObjects.GetLength(0); x++)
        {
            for (int z = 0; z < spawnedObjects.GetLength(1); z++)
            {
                if (spawnedObjects[x, z] != null)
                {
                    spawnedObjects[x, z].ReturnToPool();
                    spawnedObjects[x, z] = null;
                }
            }
        }
    }

    protected IEnumerator DestroyEffect(SceneObject obj)
    {
        yield return DestroyEffectShrink(obj);
        obj.ReturnToPool();
    }
    protected IEnumerator DestroyEffectShrink(SceneObject obj)
    {
        obj.transform.DOScale(0, 2f);
        yield return new WaitForEndOfFrame();
    }

    public static Quaternion RandomRotation()
    {
        Quaternion q = Quaternion.identity;
        q.eulerAngles = new Vector3(q.x, Random.rotation.eulerAngles.y, q.z);
        return q;
    }
}
