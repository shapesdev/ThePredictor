using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEditorWindow : ExtendedEditorWindow
{
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
        currentProperty = serializedObject.FindProperty("sceneGUISettings");
        DrawProperties(currentProperty, true);

        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable() {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable() {
        SceneView.duringSceneGui -= OnSceneGUI;
        GameObjectUtils.Clear();
    }

    private void OnSceneGUI(SceneView obj) {
        Handles.BeginGUI();

        DrawCategoriesPanel();
        DrawSelectionPanel();
        DrawSaveButton();

        Handles.EndGUI();
    }
}
