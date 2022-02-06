using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelEditorData))]
public class LevelEditorInspector : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if(GUILayout.Button("Open Level Editor")) {
            EditorApplication.ExecuteMenuItem("Shapes/Level Editor"); 
        }
    }
}
