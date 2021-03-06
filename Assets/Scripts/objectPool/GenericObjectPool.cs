using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic object pool implementation with generic twist
/// </summary>
public class GenericObjectPool<T> : MonoBehaviour, IObjectPool<T> where T : MonoBehaviour, IPoolable
{
    // Reference to prefab.
    [SerializeField]
    protected T[] prefabs;
    // References to reusable instances
    private Stack<T> reusableInstances = new Stack<T>();
    [SerializeField]
    protected int instanceCount = 0;
    /// <summary>
    /// Returns instance of prefab with parent.
    /// </summary>
    /// <param name="p">Parent <c>Transform</c>.</param>
    /// <returns>Instance of prefab.</returns>
    public T GetPrefabInstance(Transform p = null)
    {
        T inst;
        // if we have object in our pool we can use them
        if (reusableInstances.Count > 0)
        {
            // get object from pool
            inst = reusableInstances.Pop();
            // remove parent
            inst.transform.SetParent(p);
            // reset position
            inst.transform.localPosition = Vector3.zero;
            inst.transform.localScale = Vector3.one;
            inst.transform.localEulerAngles = Vector3.one;
            // activate object
            inst.gameObject.SetActive(true);
        }
        // otherwise create new instance of prefab
        else
        {
            inst = Instantiate(prefabs[Random.Range(0, prefabs.Length)], p);
            instanceCount++;
        }
        // set reference to pool
        inst.Orgin = this;
        // and prepare instance for use
        inst.PrepareToUse();
        return inst;
    }

    /// <summary>
    /// Returns instance of prefab without parent.
    /// </summary>
    /// <returns></returns>
    public T GetPrefabInstance()
    {
        return GetPrefabInstance(null);
    }

    public void PreCachePool(int n)
    {
        PreCachePool(n, null);
    }
    public void PreCachePool(int n, Transform p)
    {
        if (n < reusableInstances.Count) { return; }

        for (int i = 0; i < n; i++)
        {
            T inst = Instantiate(prefabs[Random.Range(0, prefabs.Length)], Vector3.zero, Quaternion.identity, p);
            inst.transform.localScale = Vector3.one;
            inst.gameObject.SetActive(false);
            inst.name = instanceCount.ToString();
            reusableInstances.Push(inst);
            instanceCount++;

        }
    }

    public void RecycleCache(int n) {
        if (n > reusableInstances.Count) { n = reusableInstances.Count; }

        for (int i = 0; i < n; i++)
        {
            T inst = reusableInstances.Pop();
            inst.gameObject.SetActive(true);

            if (Application.isEditor)
                Object.DestroyImmediate(inst.gameObject);
            else
                Object.Destroy(inst.gameObject);
        }
        instanceCount -= n;
        reusableInstances = new Stack<T>();
    }

    /// <summary>
    /// Returns instance to the pool.
    /// </summary>
    /// <param name="instance">Prefab instance.</param>
    public void ReturnToPool(T instance)
    {
        // disable object
        instance.gameObject.SetActive(false);
        // set parent as this object
        instance.transform.SetParent(transform);
        // reset position
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localScale = Vector3.one;
        instance.transform.localEulerAngles = Vector3.one;
        // add to pool
        reusableInstances.Push(instance);
    }
    /// <summary>
    /// Returns instance to the pool.
    /// Additional check is this is correct type.
    /// </summary>
    /// <param name="instance">Instance.</param>
    public void ReturnToPool(object instance)
    {
        // if instance is of our generic type we can return it to our pool
        if (instance is T)
        {
            ReturnToPool(instance as T);
        }
    }
}
