using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneObjectObjectPool))]
public class ObjectPoolEditor : Editor
{
    public int cacheCount = 15000;
    public override void OnInspectorGUI()
    {
        SceneObjectObjectPool myScript = (SceneObjectObjectPool)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Cache Pool"))
        {
            myScript.PreCachePool(cacheCount);
        }
        //EditorGUILayout.HelpBox("This is a help box", MessageType.Info);
    }
}
