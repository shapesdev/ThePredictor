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

    private const int MAX_SELECTIONS = 4;

    #region Editor Window GUI

    protected void DrawProperty(SerializedProperty prop, bool drawChildren, bool boldLabel) {
        if(prop != null) {
            var origFontStyle = EditorStyles.label.fontStyle;
            if (boldLabel) {
                EditorStyles.label.fontStyle = FontStyle.Bold;
            }
            EditorGUILayout.PropertyField(prop, drawChildren);
            EditorStyles.label.fontStyle = origFontStyle;
            EditorGUILayout.Space();
        }
    }

    protected void DrawLabel(string label, bool bold) {
        if (bold) EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        else EditorGUILayout.LabelField(label);
    }

    protected void DrawGeneralButtons() {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save", GUILayout.Height(50), GUILayout.Width(50))) {
            mapCatalog.Save();
        }
        if (GUILayout.Button("Reset", GUILayout.Height(50), GUILayout.Width(50))) {
            ResetGUI();
        }
        GUILayout.EndHorizontal();
    }

    #endregion

    #region Scene GUI

    protected void DrawGrid(Vector2 gridSize, Vector3 cellSize) {
        if (mapCatalog.parent == null) {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Quad);
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y - 0.5f,
                obj.transform.position.z);
            obj.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            obj.tag = "LevelGrid";
            obj.name = "Level";
            mapCatalog.parent = obj;
        }
        mapCatalog.parent.transform.localScale = new Vector3(gridSize.x * cellSize.x, gridSize.y * cellSize.z, 1);
    }

    protected void DrawCategoriesPanel() {
        var pixelRect = SceneView.currentDrawingSceneView.camera.pixelRect;
        var panel = settings.leftPanel;

        GUILayout.BeginArea(new Rect(panel.rect.x, pixelRect.height / 2 - (panel.rect.height / 2),
            panel.rect.width, panel.rect.height));

            var rect = EditorGUILayout.BeginVertical();
                GUI.Box(rect, GUIContent.none);

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
            panel.rect.y, panel.rect.width + 50, panel.rect.height));

        var rect = EditorGUILayout.BeginVertical();
            GUI.Box(rect, GUIContent.none);

                GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Selections", EditorStyles.boldLabel);
                    GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.Space(panel.buttonOffset);

                GUILayout.BeginHorizontal();
                    var selectionPage = serializedObject.FindProperty("selectionPage");
                    if (GUILayout.Button("<", GUILayout.Width(20), GUILayout.Height(20))) {
                        if(selectionPage.intValue > 0) {
                            selectionPage.intValue -= 1;
                        }
                    }
                    for (int i = selectionPage.intValue * MAX_SELECTIONS, x = 0; i < mapObjects.Count && x < 4; i++, x++) {
                        GUILayout.Space(panel.buttonOffset);
                        var selectionProp = serializedObject.FindProperty("selectionIndex");

                        if (selectionProp.intValue == i) {
                            GUI.backgroundColor = Color.white;
                            currentMapObject = mapObjects[i];
                        }
                        else {
                            GUI.backgroundColor = Color.gray;
                        }

                        if (GUILayout.Button(mapObjects[i].content, GUILayout.Height(panel.buttons[x].height),
                            GUILayout.Width(panel.buttons[x].width))) {
                            SelectionButtonPress(i, mapObjects[i]);
                        }
                    GUILayout.Space(panel.buttonOffset);
                    }
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(">", GUILayout.Width(20), GUILayout.Height(20))) {
                        if(selectionPage.intValue + 1 <= mapObjects.Count / MAX_SELECTIONS) {
                            selectionPage.intValue += 1;
                        }
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
            if (hit.transform.tag == "LevelGrid" || hit.transform.gameObject.layer == 8) {
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
        serializedObject.FindProperty("selectionPage").intValue = 0;
        currentMapObject = null;
        currentMapObjectType = MapObjectType.None;
        mapCatalog.Clear();
    }

    #endregion
}
