using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectUtils
{
    private static List<GameObject> activeGameObjects = new List<GameObject>();

    public static void AddGameObject(string name) {
        GameObject go = new GameObject(name);
        go.AddComponent<GameObjectProperties>();
        activeGameObjects.Add(go);
    }

    public static void Clear() {
        foreach(var go in activeGameObjects) {
            if(!go.GetComponent<GameObjectProperties>().Saved) {
                GameObject.DestroyImmediate(go);
            }
        }
        activeGameObjects.Clear();
    }

    public static void Save() {
        foreach (var go in activeGameObjects) {
            go.GetComponent<GameObjectProperties>().Saved = true;
        }
    }
}
