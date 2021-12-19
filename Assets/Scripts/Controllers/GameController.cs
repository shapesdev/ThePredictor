using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController
{
    private readonly IGameModel model;
    private readonly IGameView view;
    private readonly IApp app;

    public GameController(IApp app, IGameModel model, IGameView view) {
        this.app = app;
        this.model = model;
        this.view = view;
        Init();
    }

    private void Init() {
        model.Init();
        view.Init();
    }
}
