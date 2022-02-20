using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelEditorData : ScriptableObject
{
    [HideInInspector]
    public SceneGUISettings sceneGUISettings;

    [HideInInspector]
    [SerializeField]
    private Vector3 cellSize;
    [HideInInspector]
    [SerializeField]
    private Vector2 gridSize;
    [HideInInspector]
    [SerializeField]
    private int categoryIndex;
    [SerializeField]
    private int selectionIndex;
    [SerializeField]
    private int selectionPage;
    [HideInInspector]
    [SerializeField]
    private bool drawObjects = false;
}
