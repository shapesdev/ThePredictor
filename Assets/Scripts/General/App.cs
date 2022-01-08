using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour
{
    [SerializeField]
    private GameSystem gameSystem;

    void Awake() {
        gameSystem.Init();
    }
}
