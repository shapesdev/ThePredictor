using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(LevelEditor))]
public class LevelEditorGUI : Editor
{
    private void OnSceneGUI() {
        var data = (LevelEditor)target;

        //var pixelRect = SceneView.currentDrawingSceneView.camera.pixelRect;
        // have it dynamically adjust to Scene View window's size? Food for thought

        Handles.BeginGUI();

        DrawLeftPanel(data);

        Handles.EndGUI();
    }

    private void DrawLeftPanel(LevelEditor data) {

        var leftPanel = data.GetLeftPanel();

        GUILayout.BeginArea(new Rect(leftPanel.xOffset, leftPanel.yOffset, leftPanel.width, leftPanel.height));

        var rect = EditorGUILayout.BeginVertical();
        GUI.color = Color.yellow;
        GUI.Box(rect, GUIContent.none);

        GUI.color = Color.white;

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Object Categories", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        GUI.backgroundColor = Color.gray;

        GUILayout.Space(leftPanel.offsetBetweenButtons);

        if (GUILayout.Button("Static", GUILayout.Height(leftPanel.buttonHeight), GUILayout.Width(leftPanel.buttonWidth))) {
            // Method
        }

        GUILayout.Space(leftPanel.offsetBetweenButtons);

        if (GUILayout.Button("Moving", GUILayout.Height(leftPanel.buttonHeight),
            GUILayout.Width(leftPanel.buttonWidth))) {
            // Method
        }

        GUILayout.Space(leftPanel.offsetBetweenButtons);

        if (GUILayout.Button("Active", GUILayout.Height(leftPanel.buttonHeight),
            GUILayout.Width(leftPanel.buttonWidth))) {
            // Method
        }

        GUILayout.Space(leftPanel.offsetBetweenButtons);

        if (GUILayout.Button("Other", GUILayout.Height(leftPanel.buttonHeight),
            GUILayout.Width(leftPanel.buttonWidth))) {
            // Method
        }

        GUILayout.Space(leftPanel.offsetBetweenButtons);

        GUILayout.EndVertical();
        EditorGUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
