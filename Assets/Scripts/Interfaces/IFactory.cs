using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFactory
{
    void Load(IApp app, GameObject go);
    void Unload();
}
