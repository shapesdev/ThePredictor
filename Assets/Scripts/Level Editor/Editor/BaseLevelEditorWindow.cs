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
    protected EditorMapCatalog mapCatalog;

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

    protected void DrawSaveButton() {
        if (GUILayout.Button("Save", GUILayout.Height(80))) {
            mapCatalog.Save();
        }
    }

    protected void DrawResetButton() {
        if (GUILayout.Button("Reset", GUILayout.Height(80))) {
            ResetGUI();
        }
    }

    #endregion

    #region Scene GUI

    protected void DrawTest() {
        var pixelRect = SceneView.currentDrawingSceneView.camera.pixelRect;
        var panel = settings.leftPanel;

        GUILayout.BeginArea(new Rect(panel.rect.x, pixelRect.height / 2 - (panel.rect.height / 2), 80 * 4, 80 * 4));

        string[] names = { "Static", "Active", "Other", "Moving" };

        int categoryIndex = GUILayout.SelectionGrid(serializedObject.FindProperty("categoryIndex").intValue, names, 1, GUILayout.Height(80),
            GUILayout.Width(80));
        if(GUILayout.Button("Static")) {
            Debug.Log("Pressed Static");
        }
        serializedObject.FindProperty("categoryIndex").intValue = categoryIndex;

        GUILayout.EndArea();
    }

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

    protected void DrawSelectionPanel(List<MapObject> mapObjects) {
        var pixelRect = SceneView.currentDrawingSceneView.camera.pixelRect;
        var panel = settings.topPanel;

        Handles.BeginGUI();

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
                    
                    for(int i = 0; i < mapObjects.Count; i++) {
                        GUILayout.Space(panel.buttonOffset);

                        if (panel.buttons[i].selected) GUI.backgroundColor = Color.white;
                        else GUI.backgroundColor = Color.gray;

                        if (GUILayout.Button(mapObjects[i].content, GUILayout.Height(panel.buttons[i].height),
                            GUILayout.Width(panel.buttons[i].width))) {
                            SelectionButtonPress(panel.buttons[i], mapObjects[i]);
                        }
                    GUILayout.Space(panel.buttonOffset);
                    }

                GUILayout.EndHorizontal();
                GUILayout.Space(panel.buttonOffset);
            EditorGUILayout.EndVertical();
        GUILayout.EndArea();

        Handles.EndGUI();
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
        var material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        if(mapCatalog.Exists(position)) {
            material.SetColor("_BaseColor", new Color32(214, 24, 0, 203));
        }
        else {
            material.SetColor("_BaseColor", new Color32(48, 204, 214, 203));
        }
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
        mapCatalog.Clear();
    }

    #endregion
}
