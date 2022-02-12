using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorMapCatalog
{
    private List<MapObject> activeGameObjects;

    public EditorMapCatalog() {
        activeGameObjects = new List<MapObject>();
    }

    public void Add(GameObject obj, Vector3 pos) {
        GameObject go = PrefabUtility.InstantiatePrefab(obj) as GameObject;
        var mapObject = new MapObject(go);
        go.transform.position = mapObject._position = pos;
        activeGameObjects.Add(mapObject);
        Undo.RegisterCreatedObjectUndo(go, "");
    }

    public void Clear() {
        foreach(var go in activeGameObjects) {
            if(go.saved == false) {
                GameObject.DestroyImmediate(go._object);
            }
        }
        activeGameObjects.Clear();
    }

    public void Save() {
        foreach(var go in activeGameObjects) {
            go.saved = true;
        }
    }
}
