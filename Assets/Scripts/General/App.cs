using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour
{
    [SerializeField]
    private GameObject gamePrefab;

    private GameFactory gameFactory;

    private void Awake() {
        Init();
    }

    private void Init() {
        gameFactory = new GameFactory();
    }
}
