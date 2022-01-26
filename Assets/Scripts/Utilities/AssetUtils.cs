using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class AssetUtils
{
    public static T[] GetAllInstances<T>() where T : ScriptableObject {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        T[] assets = new T[guids.Length];

        for(int i = 0; i < assets.Length; i++) {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            assets[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }
        return assets;
    }

    public static T GetInstance<T>() where T : ScriptableObject {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        T asset = AssetDatabase.LoadAssetAtPath<T>(path);
        return asset;
    }
}
