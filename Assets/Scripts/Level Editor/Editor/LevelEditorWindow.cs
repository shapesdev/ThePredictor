using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class LevelEditorWindow : BaseLevelEditorWindow
{
    private SceneViewSettings originalSceneView = new SceneViewSettings();

    [MenuItem(itemName: "Shapes/Level Editor")]
    public static void Init() {
        LoadSerializedObject();
    }

    #region Loading methods

    private static void LoadSerializedObject() {
        var inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
        LevelEditorWindow window = GetWindow<LevelEditorWindow>("Level Editor", inspectorType);
        var data = AssetUtils.GetInstance<LevelEditorData>();
        settings = data.sceneGUISettings;
        window.serializedObject = new SerializedObject(data);
    }

    private void LoadLevelEditorData() {
        mapCatalog = new EditorMapCatalog();
        string resourcesPath = "Assets/Editor Default Resources/";

        if(!System.IO.Directory.Exists(resourcesPath)) {
            Debug.LogError($"Make sure you have {resourcesPath} folder");
        }
        else {
            for (MapObjectType e = MapObjectType.Static; e <= MapObjectType.Other; e++) {
                var path = resourcesPath + e.ToString();
                string[] prefabFiles = System.IO.Directory.GetFiles(path, "*.prefab");

                if (prefabFiles.Length != 0) {
                    foreach (var file in prefabFiles) {
                        var go = AssetDatabase.LoadAssetAtPath(file, typeof(GameObject)) as GameObject;
                        var content = new GUIContent(AssetPreview.GetAssetPreview(go));

                        if (mapCatalog.mapObjects.ContainsKey(e)) {
                            mapCatalog.mapObjects[e].Add(new MapObject(go, content));
                        }
                        else {
                            mapCatalog.mapObjects.Add(e, new List<MapObject> { new MapObject(go, content) });
                        }
                    }
                }
            }
        }
    }
    #endregion

    private void OnEnable() {
        SceneView.duringSceneGui += OnSceneGUI;
        LoadLevelEditorData();
        UpdateSceneViewSettings();
    }

    private void OnDisable() {
        SceneView.duringSceneGui -= OnSceneGUI;
        mapCatalog.Clear();
        UpdateSceneViewSettings(true);
    }

    private void OnGUI() {
        if(serializedObject == null) {
            LoadSerializedObject();
        }

        DrawProperty(serializedObject.FindProperty("gridSize"), false, true);
        DrawProperty(serializedObject.FindProperty("cellSize"), false, true);
        DrawProperty(serializedObject.FindProperty("upLayer"), false, true);

        DrawGeneralButtons();

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI(SceneView sceneView) {
        DrawSceneGUI(sceneView);

        serializedObject.ApplyModifiedProperties();
        sceneView.Repaint();
    }

    #region SceneGUI methods

    private void DrawSceneGUI(SceneView sceneView) {
        var cellSize = serializedObject.FindProperty("cellSize").vector3Value;
        var gridSize = serializedObject.FindProperty("gridSize").vector2Value;
        DrawGrid(gridSize, cellSize);
        DrawCategoriesPanel();

        if (currentMapObjectType != MapObjectType.None) {
            if (mapCatalog.mapObjects.TryGetValue(currentMapObjectType, out _) == true) {
                DrawSelectionPanel(mapCatalog.mapObjects[currentMapObjectType]);
                if (serializedObject.FindProperty("drawObjects").boolValue) {
                    var center = GetCellCenter();
                    var mesh = currentMapObject._object.GetComponent<MeshFilter>().sharedMesh;

                    DrawMeshPreview(mesh, center);
                    DrawHandles(center, cellSize);
                    HandleMouseEvents(center, sceneView);
                    Selection.activeObject = null;
                }
            }
            else {
                sceneView.ShowNotification(new GUIContent("There are no objects of this type"), 0.1f);
            }
        }
    }

    private void HandleMouseEvents(Vector3 center, SceneView sceneView) {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        Event e = Event.current;
        if(e.type == EventType.MouseDown || e.type == EventType.MouseDrag) {
            if (e.button == 0) {
                if (mapCatalog.Exists(center)) {
                    if(e.type == EventType.MouseDown) {
                        sceneView.ShowNotification(new GUIContent("An GameObject already exists here"), 0.1f);
                    }
                }
                else if(!IsWithinGrid(center)) {
                    sceneView.ShowNotification(new GUIContent("GameObject can only be placed within grid bounds"), 0.1f);
                }
                else {
                    mapCatalog.Add(currentMapObject._object, center);
                }
            }
        }
    }

    private void UpdateSceneViewSettings(bool reset = false) {
        SceneView sceneView = GetWindow<SceneView>(); 

        if (reset) {
            sceneView.showGrid = originalSceneView.gridEnabled;
            sceneView.sceneViewState.showSkybox = originalSceneView.skyboxEnabled;
        }
        else {
            originalSceneView.gridEnabled = sceneView.showGrid;
            sceneView.showGrid = true;
            originalSceneView.skyboxEnabled = sceneView.sceneViewState.showSkybox;
            sceneView.sceneViewState.showSkybox = false;
        }
    }

    private Vector3 GetCellCenter() {
        var cellSize = serializedObject.FindProperty("cellSize").vector3Value;
        var upLayer = serializedObject.FindProperty("upLayer").intValue;
        Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.y / guiRay.direction.y);
        Vector3Int cell = new Vector3Int(Mathf.RoundToInt(mousePosition.x / cellSize.x), 0,
            Mathf.RoundToInt(mousePosition.z / cellSize.z));
        Vector3 cellCenter = Vector3.Scale(cell, cellSize);
        cellCenter.y = upLayer;

        return cellCenter;
    }

    #endregion
}
