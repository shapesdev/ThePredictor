using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelEditorData : ScriptableObject
{
    public SceneGUISettings sceneGUISettings;

    [SerializeField]
    private int categoryIndex;

    [SerializeField]
    private int selectionIndex;

    [HideInInspector]
    [SerializeField]
    private bool drawObjects = false;
}
