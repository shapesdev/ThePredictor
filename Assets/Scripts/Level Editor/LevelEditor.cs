using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    [Header("GUI Settings")]
    [SerializeField]
    private LeftPanel leftPanel;
    [SerializeField]
    private TopPanel topPanel;

    public LeftPanel GetLeftPanel() {
        return leftPanel;
    }

    public TopPanel GetTopPanel() {
        return topPanel;
    }
}

[System.Serializable]
public class TopPanel : Panel { }

[System.Serializable]
public class LeftPanel : Panel {}

public class Panel
{
    [Header("Panel Settings")]
    public int xOffset = 0;
    public int yOffset = 40;
    public int width = 100;
    public int height = 400;

    [Header("Button Settings")]
    public int buttonWidth = 100;
    public int buttonHeight = 100;
    public int offsetBetweenButtons = 5;
}
