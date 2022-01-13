using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(LevelEditor))]
public class LevelEditorGUI : Editor
{
    private void OnSceneGUI() {
        var data = (LevelEditor)target;

        //var pixelRect = SceneView.currentDrawingSceneView.camera.pixelRect;
        // have it dynamically adjust to Scene View window's size? Food for thought

        Handles.BeginGUI();

        DrawLeftPanel(data);
        DrawTopPanel(data);

        Handles.EndGUI();
    }

    private void DrawLeftPanel(LevelEditor data) {

        var leftPanel = data.GetLeftPanel();

        GUILayout.BeginArea(new Rect(leftPanel.xOffset, leftPanel.yOffset, leftPanel.width, leftPanel.height));

        var rect = EditorGUILayout.BeginVertical();
        GUI.Box(rect, GUIContent.none);

        GUI.color = Color.white;

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Object Categories", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();

        foreach (var btn in data.leftButtons) {
            GUILayout.Space(leftPanel.offsetBetweenButtons);

            if (btn.selected) GUI.backgroundColor = Color.white;
            else GUI.backgroundColor = Color.gray;

            if (GUILayout.Button(btn.name, GUILayout.Height(leftPanel.buttonHeight), GUILayout.Width(leftPanel.buttonWidth))) {
                CategoryButtonPress(btn);
            }

            GUILayout.Space(leftPanel.offsetBetweenButtons);
        }

        GUILayout.EndVertical();
        EditorGUILayout.EndVertical();
        GUILayout.EndArea();
    }

    private void CategoryButtonPress(Buttons btn) {
        btn.selected = !btn.selected;
        var data = (LevelEditor)target;
        if (GUI.changed) {
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(data.gameObject.scene);
        }
    }

    private void DrawTopPanel(LevelEditor data) {

        var top = data.GetTopPanel();
        var pixelRect = SceneView.currentDrawingSceneView.camera.pixelRect;

        GUILayout.BeginArea(new Rect(pixelRect.width / 2 - (top.width / 2), top.yOffset, top.width, top.height));

        var rect = EditorGUILayout.BeginVertical();
        GUI.color = Color.yellow;
        GUI.Box(rect, GUIContent.none);

        GUI.color = Color.white;

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Selections");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUI.backgroundColor = Color.red;

        GUILayout.Space(top.offsetBetweenButtons);

        if (GUILayout.Button("Object 1", GUILayout.Height(top.buttonHeight),
            GUILayout.Width(top.buttonWidth))) {
            // Method
        }

        GUILayout.Space(top.offsetBetweenButtons);

        if (GUILayout.Button("Object 2", GUILayout.Height(top.buttonHeight),
            GUILayout.Width(top.buttonWidth))) {
            // Method
        }

        GUILayout.Space(top.offsetBetweenButtons);

        if (GUILayout.Button("Object 3", GUILayout.Height(top.buttonHeight),
                GUILayout.Width(top.buttonWidth))) {
            // Method
        }

        if (GUILayout.Button("Object 4", GUILayout.Height(top.buttonHeight),
            GUILayout.Width(top.buttonWidth))) {
            // Method
        }

        GUILayout.Space(top.offsetBetweenButtons);

        GUILayout.EndHorizontal();

        GUILayout.Space(top.offsetBetweenButtons);

        EditorGUILayout.EndVertical();


        GUILayout.EndArea();
    }
}
