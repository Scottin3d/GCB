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
    protected int isntanceCount = 0;
    /// <summary>
    /// Returns instance of prefab.
    /// </summary>
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
            isntanceCount++;
        }
        // set reference to pool
        inst.Orgin = this;
        // and prepare instance for use
        inst.PrepareToUse();
        return inst;
    }

    public T GetPrefabInstance()
    {
        return GetPrefabInstance(null);
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
