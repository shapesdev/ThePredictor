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

    protected void DrawCategoriesPanel() {
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

    protected void DrawSelectionPanel() {
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
                GameObjectUtils.AddGameObject(btn.name + " GameObject");
            }
            GUILayout.Space(panel.buttonOffset);
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(panel.buttonOffset);
        EditorGUILayout.EndVertical();
        GUILayout.EndArea();
    }

    protected void DrawSaveButton() {
        var pixelRect = SceneView.currentDrawingSceneView.camera.pixelRect;
        GUILayout.BeginArea(new Rect(pixelRect.width - 80, pixelRect.height - 80, 80, 80));
        if (GUILayout.Button("Save", GUILayout.Height(80), GUILayout.Width(80))) {
            GameObjectUtils.Save();
        }
        GUILayout.EndArea();
    }

    private Vector3 cellSize = new Vector3(2f, 0f, 2f);

    protected void DisplayHandlesInScene() {
        // Get the mouse position in world space such as y = 0
        Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.y / guiRay.direction.y);

        // Get the corresponding cell on our virtual grid
        Vector3Int cell = new Vector3Int(Mathf.RoundToInt(mousePosition.x / cellSize.x), 0,
            Mathf.RoundToInt(mousePosition.z / cellSize.z));
        Vector3 cellCenter = Vector3.Scale(cell, cellSize);

        // Vertices of our square
        Vector3 topLeft = cellCenter + Vector3.Scale(Vector3.left, cellSize) * 0.5f + Vector3.Scale(Vector3.forward, cellSize) * 0.5f;
        Vector3 topRight = cellCenter - Vector3.Scale(Vector3.left, cellSize) * 0.5f + Vector3.Scale(Vector3.forward, cellSize) * 0.5f;
        Vector3 bottomLeft = cellCenter + Vector3.Scale(Vector3.left, cellSize) * 0.5f - Vector3.Scale(Vector3.forward, cellSize) * 0.5f;
        Vector3 bottomRight = cellCenter - Vector3.Scale(Vector3.left, cellSize) * 0.5f - Vector3.Scale(Vector3.forward, cellSize) * 0.5f;

        // Rendering
        Handles.color = Color.green;
        Vector3[] lines = { topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft, topLeft };
        Handles.DrawLines(lines);
    }

    #endregion
}
