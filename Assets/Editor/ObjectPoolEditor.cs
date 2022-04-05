using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneObjectObjectPool))]
[System.Serializable]
public class ObjectPoolEditor : Editor
{
    int cacheCount = 15000;
    Transform parent;
    public override void OnInspectorGUI()
    {
        SceneObjectObjectPool myScript = (SceneObjectObjectPool)target;
        DrawDefaultInspector();

        EditorGUILayout.IntField("Cache Count", cacheCount);
        EditorGUILayout.ObjectField("Parent", parent, typeof(Transform));
        if (GUILayout.Button("Cache Pool"))
        {
            myScript.PreCachePool(cacheCount, parent);
        }

        if (GUILayout.Button("Recycle Pool"))
        {
            myScript.RecycleCache(cacheCount);
        }

        //EditorGUI.ObjectField(parent);
        //EditorGUILayout.HelpBox("This is a help box", MessageType.Info);
    }
}