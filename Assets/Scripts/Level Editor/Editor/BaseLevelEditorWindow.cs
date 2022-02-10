using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BaseLevelEditorWindow : EditorWindow
{
    protected static SceneGUISettings settings;

    protected SerializedObject serializedObject;
    protected SerializedProperty currentProperty;

    protected MapObjectType currentMapObjectType = MapObjectType.None;
    protected MapObject currentMapObject = null;
    protected LevelEditorGrid grid;

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

        for (int i = 0; i < panel.buttons.Length; i++) {
            GUILayout.Space(panel.buttonOffset);

            if (panel.buttons[i].selected) {
                GUI.backgroundColor = Color.white;
                currentMapObjectType = (MapObjectType)i;
            }
            else {
                GUI.backgroundColor = Color.gray;
            }

            if (GUILayout.Button(panel.buttons[i].name, GUILayout.Height(panel.buttons[i].height), GUILayout.Width(panel.buttons[i].width))) {
                CategoryButtonPress(panel.buttons[i], (MapObjectType)i);
                DisableObjectsInCollection(panel.buttons[i], panel.buttons);
            }
            GUILayout.Space(panel.buttonOffset);
        }

        GUILayout.EndVertical();
        EditorGUILayout.EndVertical();
        GUILayout.EndArea();
    }

    protected void DrawSelectionPanel(MapObject mapObject) {
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

                        if (GUILayout.Button(mapObject.content, GUILayout.Height(btn.height),
                            GUILayout.Width(btn.width))) {
                            SelectionButtonPress(btn, mapObject);
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
            grid.SaveGameObjects();
        }
        GUILayout.EndArea();
    }

    protected void DrawResetButton() {
        var pixelRect = SceneView.currentDrawingSceneView.camera.pixelRect;
        GUILayout.BeginArea(new Rect(pixelRect.width - 165, pixelRect.height - 80, 80, 80));
        if (GUILayout.Button("Reset", GUILayout.Height(80), GUILayout.Width(80))) {
            ResetGUI();
        }
        GUILayout.EndArea();
    }

    protected void DrawHandles(Vector3 cellCenter, Vector3 cellSize) {
        Vector3 topLeft = cellCenter + Vector3.Scale(Vector3.left, cellSize) * 0.5f + Vector3.Scale(Vector3.forward, cellSize) * 0.5f;
        Vector3 topRight = cellCenter - Vector3.Scale(Vector3.left, cellSize) * 0.5f + Vector3.Scale(Vector3.forward, cellSize) * 0.5f;
        Vector3 bottomLeft = cellCenter + Vector3.Scale(Vector3.left, cellSize) * 0.5f - Vector3.Scale(Vector3.forward, cellSize) * 0.5f;
        Vector3 bottomRight = cellCenter - Vector3.Scale(Vector3.left, cellSize) * 0.5f - Vector3.Scale(Vector3.forward, cellSize) * 0.5f;

        Handles.color = Color.green;
        Vector3[] lines = { topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft, topLeft };
        Handles.DrawLines(lines);
    }

    protected void DrawMeshPreview(Mesh mesh, Vector3 position) {
        Color color = new Color(104, 223, 248, 213);
        var material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        material.SetColor("_BaseColor", color); // Doesn't work for some reason
        Graphics.DrawMesh(mesh, position, Quaternion.identity, material, 0);
    }

    #endregion

    #region Other methods

    private void CategoryButtonPress(Button_ btn, MapObjectType type) {
        btn.selected = !btn.selected;
        if (btn.selected == false) {
            currentMapObjectType = MapObjectType.None;
        }
        else {
            currentMapObjectType = type;
        }
    }

    private void SelectionButtonPress(Button_ btn, MapObject obj) {
        btn.selected = !btn.selected;
        serializedObject.FindProperty("drawObjects").boolValue ^= true;
        if(currentMapObject == obj) {
            currentMapObject = null;
        }
        else {
            currentMapObject = obj;
        }
    }

    private void DisableObjectsInCollection(Button_ obj, Button_[] collection) {
        foreach(var item in collection) {
            if(item != obj) {
                item.selected = false;
            }
        }
    }

    private void ResetGUI() {
        foreach (var btn in settings.leftPanel.buttons) {
            btn.selected = false;
        }
        foreach (var btn in settings.topPanel.buttons) {
            btn.selected = false;
        }
        serializedObject.FindProperty("drawObjects").boolValue = false;
        currentMapObject = null;
        currentMapObjectType = MapObjectType.None;
        grid.ClearGameObjects();
    }

    #endregion
}
