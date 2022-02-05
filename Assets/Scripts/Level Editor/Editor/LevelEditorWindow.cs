using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum CategoryType
{
    None = -1, Static, Moving, Active, Other
}

public class MapObject
{
    public Vector3 _position;
    public GameObject _object;
    public GUIContent content;

    public MapObject(GameObject _object, GUIContent content) {
        this._object = _object;
        this.content = content;
    }
}

public class LevelEditorWindow : ExtendedEditorWindow
{
    [SerializeField]
    private Dictionary<CategoryType, MapObject> palette = new Dictionary<CategoryType, MapObject>();
    [SerializeField]
    private int paletteIndex;

    [MenuItem(itemName: "Shapes/Level Editor")]
    public static void Init() {
        LoadSerializedObject();
    }

    private static void LoadSerializedObject() {
        LevelEditorWindow window = GetWindow<LevelEditorWindow>("Level Editor");
        var data = AssetUtils.GetInstance<LevelEditorData>();
        settings = data.sceneGUISettings;
        window.serializedObject = new SerializedObject(data);
    }

    private void OnGUI() {
        if(serializedObject == null) {
            LoadSerializedObject();
        }
        //currentProperty = serializedObject.FindProperty("sceneGUISettings");
        //DrawProperties(currentProperty, true);

        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable() {
        SceneView.duringSceneGui += OnSceneGUI;
        LoadSelectionPrefabs();
    }

    private void OnDisable() {
        SceneView.duringSceneGui -= OnSceneGUI;
        GameObjectUtils.Clear();
    }

    private void OnSceneGUI(SceneView sceneView) {
        if (serializedObject.FindProperty("paintMode").boolValue) {
            var cellCenter = GetCellCenter();

            DrawHandles(cellCenter);
            //DrawMeshPreview(palette[0].GetComponent<MeshFilter>().sharedMesh, cellCenter);
            sceneView.Repaint();
        }
        DrawCategoriesPanel();
        if(currentCategoryType != CategoryType.None) {
            DrawSelectionPanel(palette[currentCategoryType]);
        }

        DrawSaveButton();
        DrawResetButton();
    }

    private void LoadSelectionPrefabs() {
        palette.Clear();
        string resourcesPath = "Assets/Editor Default Resources/";

        for(CategoryType e = CategoryType.Static; e <= CategoryType.Other; e++) {
            var path = resourcesPath + e.ToString();
            string[] prefabFiles = System.IO.Directory.GetFiles(path, "*.prefab");
            foreach(var file in prefabFiles) {
                var go = AssetDatabase.LoadAssetAtPath(file, typeof(GameObject)) as GameObject;
                var content = new GUIContent(AssetPreview.GetAssetPreview(go));
                palette.Add(e, new MapObject(go, content));
            }
        }
    }
}
