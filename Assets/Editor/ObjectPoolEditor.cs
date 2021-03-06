using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneObjectObjectPool))]
public class ObjectPoolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SceneObjectObjectPool myScript = (SceneObjectObjectPool)target;
        DrawDefaultInspector();

        myScript.cacheCount = EditorGUILayout.IntField("Cache Count", myScript.cacheCount);
        myScript.parent = EditorGUILayout.ObjectField("Parent", myScript.parent, typeof(Transform), true) as Transform;

        if (GUILayout.Button("Cache Pool"))
        {
            myScript.PreCachePool(myScript.cacheCount, myScript.parent);
        }

        if (GUILayout.Button("Recycle Pool"))
        {
            myScript.RecycleCache(myScript.cacheCount);
        }

        if(GUILayout.Button("Force Delete"))
        {
            for (int i = 0; i < myScript.parent.childCount; i++)
            {
                GameObject t = myScript.parent.GetChild(i).gameObject;
                DestroyImmediate(t);
            }
        }

        //EditorGUI.ObjectField(parent);
        //EditorGUILayout.HelpBox("This is a help box", MessageType.Info);
    }
}