using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExtendedEditorWindow : EditorWindow
{
    protected static SceneGUISettings settings;
    protected SerializedObject serializedObject;
    protected SerializedProperty currentProperty;

    #region Editor Window GUI

    protected void DrawProperties(SerializedProperty prop, bool drawChildren) {
        if(prop != null) {
            EditorGUILayout.PropertyField(prop, drawChildren);
        }
    }

    protected void DrawLabel(string label, bool bold) {
        if (bold) EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        else EditorGUILayout.LabelField(label);
    }

    #endregion

    #region Scene GUI

    protected void DrawLeftPanel() {
        var pixelRect = SceneView.currentDrawingSceneView.camera.pixelRect;
        var panel = settings.leftPanel;

        GUILayout.BeginArea(new Rect(panel.rect.x, pixelRect.height / 2 - (panel.rect.height / 2),
            panel.rect.width, panel.rect.height));

        var rect = EditorGUILayout.BeginVertical();
        GUI.Box(rect, GUIContent.none);

        GUI.color = Color.white;

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Categories", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();

        foreach (var btn in panel.buttons) {
            GUILayout.Space(panel.buttonOffset);

            if (btn.selected) GUI.backgroundColor = Color.white;
            else GUI.backgroundColor = Color.gray;

            if (GUILayout.Button(btn.name, GUILayout.Height(btn.height), GUILayout.Width(btn.width))) {
                CategoryButtonPress(btn);
            }
            GUILayout.Space(panel.buttonOffset);
        }

        GUILayout.EndVertical();
        EditorGUILayout.EndVertical();
        GUILayout.EndArea();
    }

    private void CategoryButtonPress(Button_ btn) {
        btn.selected = !btn.selected;
    }

    protected void DrawTopPanel() {
        var pixelRect = SceneView.currentDrawingSceneView.camera.pixelRect;
        var panel = settings.topPanel;

        GUILayout.BeginArea(new Rect(pixelRect.width / 2 - (panel.rect.width / 2),
            panel.rect.y, panel.rect.width, panel.rect.height));

        var rect = EditorGUILayout.BeginVertical();

        GUI.Box(rect, GUIContent.none);
        GUI.color = Color.white;

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Selections", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(panel.buttonOffset);

        GUILayout.BeginHorizontal();
        GUI.backgroundColor = Color.red;

        foreach (var btn in panel.buttons) {
            GUILayout.Space(panel.buttonOffset);

            if (btn.selected) GUI.backgroundColor = Color.white;
            else GUI.backgroundColor = Color.gray;

            if (GUILayout.Button(btn.name, GUILayout.Height(btn.height),
                GUILayout.Width(btn.width))) {
                // Method
            }
            GUILayout.Space(panel.buttonOffset);
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(panel.buttonOffset);
        EditorGUILayout.EndVertical();
        GUILayout.EndArea();
    }

    #endregion
}
