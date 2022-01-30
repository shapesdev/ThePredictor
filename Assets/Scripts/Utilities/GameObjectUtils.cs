using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class GameObjectUtils
{
    private static List<GameObject> activeGameObjects = new List<GameObject>();

    public static void AddGameObject(string name) {
/*        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.AddComponent<GameObjectProperties>();
        activeGameObjects.Add(go);*/
        ShowPreview();
        //Selection.activeObject = go;
    }

    public static void ShowPreview() {
        var myMat = Resources.Load("Blue.mat");
        var mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        Vector3 position = Input.mousePosition;
        Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity);
    }

    public static void Clear() {
        foreach(var go in activeGameObjects) {
            if(go != null) {
                if (!go.GetComponent<GameObjectProperties>().Saved) {
                    GameObject.DestroyImmediate(go);
                }
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
