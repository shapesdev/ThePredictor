using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelEditorData : ScriptableObject
{
    public SceneGUISettings sceneGUISettings;

    [SerializeField]
    private Vector3 cellSize;
    [SerializeField]
    private Vector2 gridSize;
    [SerializeField]
    private int categoryIndex;
    [SerializeField]
    private int selectionIndex;
    [SerializeField]
    private bool drawObjects = false;
}
