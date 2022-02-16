using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelEditorGrid
{
    private Vector3 cellSize;
    private Vector2 gridSize;

    public LevelEditorGrid(Vector2 gridSize, Vector3 cellSize) {
        this.gridSize = gridSize;
        this.cellSize = cellSize;
    }

    public Vector2 GetGridSize() {
        return gridSize;
    }

    public Vector3 GetCellSize() {
        return cellSize;
    }

    public Vector3 GetCellCenter() {
        Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.y / guiRay.direction.y);
        Vector3Int cell = new Vector3Int(Mathf.RoundToInt(mousePosition.x / cellSize.x), 0,
            Mathf.RoundToInt(mousePosition.z / cellSize.z));
        Vector3 cellCenter = Vector3.Scale(cell, cellSize);

        return cellCenter;
    }
}
