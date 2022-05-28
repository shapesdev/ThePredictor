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
    [Range(0, 5)]
    [SerializeField]
    private int upLayer;
    [HideInInspector]
    [SerializeField]
    private int categoryIndex;
    [HideInInspector]
    [SerializeField]
    private int selectionIndex;
    [HideInInspector]
    [SerializeField]
    private int selectionPage;
    [HideInInspector]
    [SerializeField]
    private bool drawObjects = false;
}
