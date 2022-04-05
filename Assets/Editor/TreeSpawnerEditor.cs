using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TreeSpawner))]
public class TreeSpawnerEditor : Editor
{
    public bool cachedValues = false;
    public override void OnInspectorGUI()
    {
        TreeSpawner myScript = (TreeSpawner)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Generate")) {
            myScript.Spawn(cachedValues);
            cachedValues = true;
        }
        //EditorGUILayout.HelpBox("This is a help box", MessageType.Info);
    }
}
