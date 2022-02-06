using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEditorWindow : BaseLevelEditorWindow
{
    [SerializeField]
    private int paletteIndex;

    private LevelEditorGrid grid;

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
        string resourcesPath = "Assets/Editor Default Resources/";

        for (MapObjectType e = MapObjectType.Static; e <= MapObjectType.Other; e++) {
            var path = resourcesPath + e.ToString();
            string[] prefabFiles = System.IO.Directory.GetFiles(path, "*.prefab");
            foreach (var file in prefabFiles) {
                var go = AssetDatabase.LoadAssetAtPath(file, typeof(GameObject)) as GameObject;
                var content = new GUIContent(AssetPreview.GetAssetPreview(go));
                grid.mapObjects.Add(e, new MapObject(go, content));
            }
        }
    }
    #endregion

    private void OnEnable() {
        SceneView.duringSceneGui += OnSceneGUI;
        LoadLevelEditorGrid();
    }

    private void OnDisable() {
        SceneView.duringSceneGui -= OnSceneGUI;
        GameObjectUtils.Clear();
    }

    private void OnGUI() {
        if(serializedObject == null) {
            LoadSerializedObject();
        }
        //currentProperty = serializedObject.FindProperty("sceneGUISettings");
        //DrawProperties(currentProperty, true);

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI(SceneView sceneView) {
        DrawCategoriesPanel();
        DrawSaveButton();
        DrawResetButton();

        if (currentMapObject != MapObjectType.None) {
            if (grid.mapObjects.TryGetValue(currentMapObject, out _) == true) {
                DrawSelectionPanel(grid.mapObjects[currentMapObject]);

                if (serializedObject.FindProperty("drawObjects").boolValue) {
                    Debug.Log("size: " + grid.GetCellSize());
                    DrawMeshPreview(grid.mapObjects[currentMapObject]._object.GetComponent<MeshFilter>().sharedMesh, grid.GetCellCenter());
                    DrawHandles(grid.GetCellCenter(), grid.GetCellSize());
                    // WTF DUDE
                }
            }
            else {
                sceneView.ShowNotification(new GUIContent("There are no objects of this type"));
            }
        }

        sceneView.Repaint();
    }
}
