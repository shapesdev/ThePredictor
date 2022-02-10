using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject
{
    public Vector3 _position;
    public GameObject _object;
    public GUIContent content;
    public bool saved;

    public MapObject(GameObject _object, GUIContent content) {
        this._object = _object;
        this.content = content;
        saved = false;
    }

    public MapObject(GameObject _object) {
        this._object = _object;
        saved = false;
    }
}
