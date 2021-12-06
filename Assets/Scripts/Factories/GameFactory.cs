using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFactory : MonoBehaviour
{
    private GameController controller;
    private GameModel model;
    private GameView view;

    private GameObject instance;

    public void Load(GameObject gamePrefab) {
        instance = GameObject.Instantiate(gamePrefab);
        view = instance.GetComponent<GameView>();
        model = new GameModel();
        controller = new GameController(model, view);
    }
}
