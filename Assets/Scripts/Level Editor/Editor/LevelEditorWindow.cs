using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public struct SceneViewSettings {
    public bool skyboxUpdated;
    public bool gridUpdated;
}

public class LevelEditorWindow : BaseLevelEditorWindow
{
    private LevelEditorGrid grid;
    private SceneViewSettings originalSceneView = new SceneViewSettings();

    [MenuItem(itemName: "Shapes/Level Editor")]
    public static void Init() {
        LoadSerializedObject();
    }

    #region Loading methods

    private static void LoadSerializedObject() {
        LevelEditorWindow window = GetWindow<LevelEditorWindow>("Level Editor");
        var data = AssetUtils.GetInstance<LevelEditorData>();
        settings = data.sceneGUISettings;
        window.serializedObject = new SerializedObject(data);
    }

    private void LoadLevelEditorGrid() {        
        grid = new LevelEditorGrid(new Vector2(10, 10), new Vector3(2.0f, 0.0f, 2.0f));
        mapCatalog = new EditorMapCatalog();
        string resourcesPath = "Assets/Editor Default Resources/";

        for (MapObjectType e = MapObjectType.Static; e <= MapObjectType.Other; e++) {
            var path = resourcesPath + e.ToString();
            string[] prefabFiles = System.IO.Directory.GetFiles(path, "*.prefab");

            if(prefabFiles.Length != 0) {
                foreach (var file in prefabFiles) {
                    var go = AssetDatabase.LoadAssetAtPath(file, typeof(GameObject)) as GameObject;
                    var content = new GUIContent(AssetPreview.GetAssetPreview(go));

                    if(grid.mapObjects.ContainsKey(e)) {
                        grid.mapObjects[e].Add(new MapObject(go, content));
                    }
                    else {
                        grid.mapObjects.Add(e, new List<MapObject> { new MapObject(go, content) });
                    }
                }
            }
        }
    }
    #endregion

    private void OnEnable() {
        SceneView.duringSceneGui += OnSceneGUI;
        LoadLevelEditorGrid();
        UpdateSceneViewSettings(GetWindow<SceneView>());
    }

    private void OnDisable() {
        SceneView.duringSceneGui -= OnSceneGUI;
        mapCatalog.Clear();
        UpdateSceneViewSettings(GetWindow<SceneView>(), true);
    }

    private void OnGUI() {
        if(serializedObject == null) {
            LoadSerializedObject();
        }

        DrawSaveButton();
        DrawResetButton();

        //currentProperty = serializedObject.FindProperty("sceneGUISettings");
        //DrawProperties(currentProperty, true);

        //serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI(SceneView sceneView) {
        DrawSceneGUI(sceneView);

        sceneView.Repaint();
    }

    #region SceneGUI methods

    private void DrawSceneGUI(SceneView sceneView) {
        DrawCategoriesPanel();

        if (currentMapObjectType != MapObjectType.None) {
            if (grid.mapObjects.TryGetValue(currentMapObjectType, out _) == true) {
                DrawSelectionPanel(grid.mapObjects[currentMapObjectType]);
                if (serializedObject.FindProperty("drawObjects").boolValue) {
                    var center = grid.GetCellCenter();
                    var cellSize = grid.GetCellSize();
                    var mesh = currentMapObject._object.GetComponent<MeshFilter>().sharedMesh; // currentMapObject gets reset after close, need to fix this

                    DrawMeshPreview(mesh, center);
                    DrawHandles(center, cellSize);
                    HandleMouseEvents(center, sceneView);
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
                        sceneView.ShowNotification(new GUIContent("Cannot add GameObject here"), 0.1f);
                    }
                }
                else {
                    mapCatalog.Add(currentMapObject._object, center);
                }
            }
        }
        Selection.activeObject = null;
    }

    private void UpdateSceneViewSettings(SceneView sceneView, bool reset = false) {
        if(reset) {
            if (originalSceneView.gridUpdated) {
                sceneView.showGrid = !sceneView.showGrid;
            }
            if (originalSceneView.skyboxUpdated) {
                sceneView.sceneViewState.showSkybox = !sceneView.sceneViewState.showSkybox;
            }
        }
        else {
            if (!sceneView.showGrid) {
                sceneView.showGrid = true;
                originalSceneView.gridUpdated = true;
            }
            if (sceneView.sceneViewState.showSkybox) {
                sceneView.sceneViewState.showSkybox = false;
                originalSceneView.skyboxUpdated = true;
            }
        }
    }

    #endregion
}
