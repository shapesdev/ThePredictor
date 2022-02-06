using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelEditorData : ScriptableObject
{
    public SceneGUISettings sceneGUISettings;

    [HideInInspector]
    public bool drawObjects = false;
}
