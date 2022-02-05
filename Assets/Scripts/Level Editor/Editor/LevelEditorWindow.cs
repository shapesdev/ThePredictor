using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEditorWindow : ExtendedEditorWindow
{
    [SerializeField]
    private List<GameObject> palette = new List<GameObject>();
    List<GUIContent> paletteIcons = new List<GUIContent>();
    [SerializeField]
    private int paletteIndex;

    private string path = "Assets/Editor Default Resources/";

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

    private void OnSceneGUI(SceneView target) {
        if (paintMode) {
            Event e = Event.current;
            if(e.type == EventType.MouseMove) {
                HandleUtility.Repaint();
            }
            DisplayHandlesInScene(palette[0].GetComponent<MeshFilter>().sharedMesh, palette[0].GetComponent<MeshRenderer>().sharedMaterial);
            EditorUtility.SetDirty(target);
        }

        DrawCategoriesPanel();
        DrawSelectionPanel(paletteIcons);

        DrawSaveButton();
        DrawResetButton();
    }

    private void LoadSelectionPrefabs() {
        palette.Clear();

        string[] prefabFiles = System.IO.Directory.GetFiles(path, "*.prefab");
        foreach(var file in prefabFiles) {
            var go = AssetDatabase.LoadAssetAtPath(file, typeof(GameObject)) as GameObject;
            palette.Add(go);

            var texture = AssetPreview.GetAssetPreview(go);
            paletteIcons.Add(new GUIContent(texture));
        }
    }
}
