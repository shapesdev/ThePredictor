using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelEditorData : ScriptableObject
{
    public SceneGUISettings sceneGUISettings;

    [HideInInspector]
    [SerializeField]
    private MapObject currentMapObject;
    [HideInInspector]
    [SerializeField]
    private int categoryIndex;
    [HideInInspector]
    [SerializeField]
    private int selectionIndex;
    [HideInInspector]
    [SerializeField]
    private bool drawObjects = false;
}
