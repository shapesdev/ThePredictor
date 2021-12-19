using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFactory : IFactory
{
    private GameController controller;
    private GameModel model;
    private GameView view;

    private GameObject instance;

    public void Load(IApp app, GameObject prefab) {
        instance = GameObject.Instantiate(prefab);
        view = instance.GetComponent<GameView>();
        model = new GameModel();
        controller = new GameController(app, model, view);
    }

    public void Unload() {
        GameObject.Destroy(instance);
    }
}
