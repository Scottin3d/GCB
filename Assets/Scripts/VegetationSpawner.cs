using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class VegetationSpawner : ObjectSpawner
{
    private static bool updateGrass = true;
    [SerializeField] private float updateGrassInterval = 2f;
    protected override void Spawn() {
        float[,] map = PathMapping.PathOpacityMap;
        MapCell[,] cachedMapCells = pathMapping.CachedMapCells;
        spawnedObjects = new SceneObject[map.GetLength(0), map.GetLength(1)];
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int z = 0; z < map.GetLength(1); z++)
            {
                if (cachedMapCells[x, z].Center != Vector3.zero && map[x,z] <= 0.1f && Random.Range(0f, 1f) < Density && cachedMapCells[x, z].Slope < slopeBias) {

                    SceneObject inst = objectPool.GetPrefabInstance(parent);
                    /*
                    GameObject g = Instantiate(prefabsVariants[Random.Range(0, prefabsVariants.Length)],
                                               cachedMapCells[x,z].Center, 
                                               RandomRotation(), 
                                               parent);
                    */
                    inst.transform.position = cachedMapCells[x, z].Center;
                    inst.transform.rotation = RandomRotation();
                    inst.transform.rotation = Quaternion.LookRotation(transform.forward, cachedMapCells[x, z].Normal);

                    if (Random.Range(0,2) == 0) {
                        inst.transform.localScale = new Vector3(-1f, 1f, -1f);
                    }

                    spawnedObjects[x, z] = inst;
                }
            }
        }

        StartCoroutine(UpdateGrass());
    }

    IEnumerator UpdateGrass() {
        while (updateGrass) {
            float[,] map = PathMapping.PathOpacityMap;
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int z = 0; z < map.GetLength(1); z++)
                {
                    if (map[x, z] >= 0.9f)
                    {
                        if (spawnedObjects[x, z] == null) { continue; }
                        StartCoroutine(DestroyEffect(spawnedObjects[x, z]));
                        spawnedObjects[x, z] = null;
                    }
                }
            }

            yield return new WaitForSeconds(updateGrassInterval);
        }
    }
}
