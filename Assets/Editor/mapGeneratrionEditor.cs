using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof (mapGeneration))]

public class mapGeneratrionEditor : Editor {
    public override void OnInspectorGUI()
    {
        mapGeneration mapGen = (mapGeneration)target;

        if (DrawDefaultInspector()){
            if (mapGen.autoUpdate)
            {
             mapGen.DrawMapInEditor();
            }
        }

        if (GUILayout.Button ("Generate"))
        {
            mapGen.DrawMapInEditor();
        }

    }
}
