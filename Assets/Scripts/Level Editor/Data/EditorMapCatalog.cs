using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorMapCatalog
{
    private List<MapObject> activeGameObjects;
    private GameObject parent = null;

    public Dictionary<MapObjectType, List<MapObject>> mapObjects;

    public EditorMapCatalog() {
        activeGameObjects = new List<MapObject>();
        mapObjects = new Dictionary<MapObjectType, List<MapObject>>();
    }

    public void Add(GameObject obj, Vector3 pos) {
        if(parent == null) {
            parent = new GameObject("Level");
        }
        GameObject go = GameObject.Instantiate(obj);
        go.name = $"{go.name}-Preview";
        var mapObject = new MapObject(go);
        go.transform.position = mapObject._position = pos;
        go.transform.SetParent(parent.transform);
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
        GameObject.DestroyImmediate(parent);
        parent = null;
    }

    public void Save() {
        foreach(var go in activeGameObjects) {
            if(go._object != null) {
                go.saved = true;
                go._object.name = go._object.name.Substring(0, go._object.name.Length - 8);
            }
        }
    }

    public bool Exists(Vector3 pos) {
        foreach(var go in activeGameObjects) {
            if(pos == go._position) {
                return true;
            }
        }
        return false;
    }
}
