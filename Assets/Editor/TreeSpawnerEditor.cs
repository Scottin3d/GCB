using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TreeSpawner))]
public class TreeSpawnerEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TreeSpawner myScript = (TreeSpawner)target;
        GUILayout.Label("Precaching of Trees.");

        if (GUILayout.Button("Generate")) {
            myScript.Spawn(false);
            myScript.cachedValues = true;
        }

        if (GUILayout.Button("Clear")) {
            myScript.RecycleObjects();
        }
    }
}
