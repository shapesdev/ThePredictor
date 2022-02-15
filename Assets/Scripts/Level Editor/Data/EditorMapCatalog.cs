using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorMapCatalog
{
    private List<MapObject> activeGameObjects;
    private GameObject parent = null;

    public EditorMapCatalog() {
        activeGameObjects = new List<MapObject>();
    }

    public void Add(GameObject obj, Vector3 pos) {
        if(parent == null) {
            parent = new GameObject("Level");
        }

        GameObject go = PrefabUtility.InstantiatePrefab(obj) as GameObject;
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
