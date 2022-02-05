using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelEditorData : ScriptableObject
{
    public SceneGUISettings sceneGUISettings;

    public Vector3 cellSize = new Vector3(2f, 0f, 2f);
    public bool paintMode = false;
}

#region Classes

[System.Serializable]
public class SceneGUISettings
{
    public Panel leftPanel;
    public Panel topPanel;
    public Panel botPanel;
}

[System.Serializable]
public class Panel
{
    public Rect rect;
    public Button_[] buttons;
    public int buttonOffset;
}

[System.Serializable]
public class Button_
{
    public string name;
    public bool selected;
    public int width;
    public int height;
}

#endregion
