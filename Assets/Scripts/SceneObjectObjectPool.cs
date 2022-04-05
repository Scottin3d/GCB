using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// resources
// https://www.reddit.com/r/Unity3D/comments/6b1rx9/why_isnt_my_custom_editor_saving_values/


/// <summary>
/// This script needs to be added to an empty <c>GameObject</c> in the hierarchy 
/// and have a prefab assigned.
/// </summary>
public class SceneObjectObjectPool : GenericObjectPool<SceneObject>
{
    [HideInInspector]
    public int cacheCount;
    [HideInInspector]
    public Transform parent;
}