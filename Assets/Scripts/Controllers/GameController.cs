using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController
{
    private readonly IGameModel model;
    private readonly IGameView view;

    public GameController(IGameModel model, IGameView view) {
        this.model = model;
        this.view = view;
        Init();
    }

    private void Init() {
        model.Init();
        view.Init();
    }
}
