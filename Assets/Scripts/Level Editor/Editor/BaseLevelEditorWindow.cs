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

    private GameObject gridObj = null;

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

    protected void DrawGrid() {
        if(gridObj == null) {
            var gridSize = serializedObject.FindProperty("gridSize").vector2Value;
            var cellSize = serializedObject.FindProperty("cellSize").vector3Value;
            gridObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
            gridObj.transform.position = new Vector3(gridObj.transform.position.x, gridObj.transform.position.y - 0.5f,
                gridObj.transform.position.z);
            gridObj.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            gridObj.transform.localScale = new Vector3(gridSize.x * cellSize.x, gridSize.y * cellSize.z, 1);
            gridObj.tag = "LevelGrid";
        }
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

            if(serializedObject.FindProperty("categoryIndex").intValue == i) {
                GUI.backgroundColor = Color.white;
                currentMapObjectType = (MapObjectType)i;
            }
            else {
                GUI.backgroundColor = Color.gray;
            }

            if (GUILayout.Button(panel.buttons[i].name, GUILayout.Height(panel.buttons[i].height), GUILayout.Width(panel.buttons[i].width))) {
                CategoryButtonPress(i, (MapObjectType)i);
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

                        if (serializedObject.FindProperty("selectionIndex").intValue == i) {
                            GUI.backgroundColor = Color.white;
                            currentMapObject = mapObjects[i];
                        }
                        else {
                            GUI.backgroundColor = Color.gray;
                        }

                        if (GUILayout.Button(mapObjects[i].content, GUILayout.Height(panel.buttons[i].height),
                            GUILayout.Width(panel.buttons[i].width))) {
                            SelectionButtonPress(i, mapObjects[i]);
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
        if(!mapCatalog.Exists(position) && IsWithinGrid(position)) {
            material.SetColor("_BaseColor", new Color32(48, 204, 214, 203));
        }
        else {
            material.SetColor("_BaseColor", new Color32(214, 24, 0, 203));
        }
        Graphics.DrawMesh(mesh, position, Quaternion.identity, material, 0);
    }

    #endregion

    #region Other methods

    protected bool IsWithinGrid(Vector3 pos) {
        RaycastHit hit;
        if(Physics.Raycast(pos, -Vector3.up, out hit)) {
            if(hit.transform.tag == "LevelGrid") {
                return true;
            }
        }
        return false;
    }

    private void CategoryButtonPress(int index, MapObjectType type) {
        var prop = serializedObject.FindProperty("categoryIndex");
        if (prop.intValue == index) {
            currentMapObjectType = MapObjectType.None;
            prop.intValue = -1;
        }
        else {
            currentMapObjectType = type;
            prop.intValue = index;
        }
    }

    private void SelectionButtonPress(int index, MapObject obj) {
        var prop = serializedObject.FindProperty("selectionIndex");
        var drawProp = serializedObject.FindProperty("drawObjects");
        if (currentMapObject == obj) {
            currentMapObject = null;
            prop.intValue = -1;
            drawProp.boolValue = false;
        }
        else {
            currentMapObject = obj;
            prop.intValue = index;
            drawProp.boolValue = true;
        }
    }

    private void ResetGUI() {
        serializedObject.FindProperty("drawObjects").boolValue = false;
        serializedObject.FindProperty("categoryIndex").intValue = -1;
        serializedObject.FindProperty("selectionIndex").intValue = -1;
        currentMapObject = null;
        currentMapObjectType = MapObjectType.None;
        mapCatalog.Clear();
        GameObject.DestroyImmediate(gridObj);
        gridObj = null;
    }

    #endregion
}
