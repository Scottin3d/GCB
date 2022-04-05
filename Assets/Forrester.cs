using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Forrester : MonoBehaviour
{
    [SerializeField] PathMapping pathMapping;
    [SerializeField] Transform spawnParent;
    [SerializeField] private InfluenceVolume influenceVolume;
    [SerializeField] SceneObjectObjectPool objectPool;
    [SerializeField] int maxCount = 50;

    private int positionX;
    private int positionZ;
    [SerializeField] bool planting = false;
    [SerializeField] List<SceneObject> influencedTrees;
    public int treeCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        pathMapping.GetXZ(transform.position, out positionX, out positionZ);
        StartCoroutine(ScanVolume());
    }


    private void PlantTrees() {
        planting = true;
        while (treeCount <= maxCount) {
            treeCount++;
            StartCoroutine(PlantTree());
        }
        planting = false;
    }

    IEnumerator PlantTree()
    {
        SceneObject g = objectPool.GetPrefabInstance(spawnParent);

        Vector3 spawnPosition = Vector3.zero;
        Vector2 r = Random.insideUnitCircle * influenceVolume.Radius;
        spawnPosition = transform.position + new Vector3(r.x, 0f, r.y);
        spawnPosition = Utility.RaycastDown(spawnPosition, LayerMask.GetMask("Ground"));

        Collider[] colliders = Physics.OverlapSphere(spawnPosition, g.GetComponent<CapsuleCollider>().radius);
        foreach (var c in colliders)
        {
            if (c.CompareTag("tree")) 
            { 
                g.ReturnToPool();
                yield return new WaitForEndOfFrame();
            }
        }
        g.transform.position = spawnPosition;
        g.transform.localScale = Vector3.zero;
        g.transform.DOScale(1f, 15f);
        influencedTrees.Add(g);
        
        yield return new WaitForEndOfFrame();
    }

   
        IEnumerator ScanVolume()
    {
        while (true)
        {
            treeCount = 0;
            influencedTrees.Clear();
            influencedTrees = new List<SceneObject>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
            foreach (var c in colliders)
            {
                if (c.CompareTag("tree"))
                {
                    influencedTrees.Add(c.GetComponent<SceneObject>());
                }
            }

            treeCount = influencedTrees.Count;
            if (!planting && treeCount <= maxCount) {
                PlantTrees();
            }

            yield return new WaitForSeconds(5f);
        }
    }
}
