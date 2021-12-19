using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour, IApp
{
    [SerializeField]
    private GameObject gamePrefab;

    private IFactory gameFactory;

    private void Awake() {
        Init();
    }

    private void Init() {
        gameFactory = new GameFactory();
    }

    public void LoadGame() {
        gameFactory.Load(this, gamePrefab);
    }

    public void LoadMainMenu() {
        gameFactory.Unload();
    }
}
