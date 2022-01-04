using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFactory : IFactory
{
    private IGameController controller;
    private IGameModel model;
    private IGameView view;

    private GameObject instance;
    private Cell cellPrefab;

    public GameFactory(Cell cellPrefab)
    {
        this.cellPrefab = cellPrefab;
    }

    public void Load(IApp app, GameObject prefab) {
        instance = GameObject.Instantiate(prefab);
        view = instance.GetComponentInChildren<GameView>();
        model = new GameModel(GenerateCells());
        controller = new GameController(app, model, view);

        controller.Init();
    }

    public void Unload() {
        GameObject.Destroy(instance);
    }

    private List<Cell> GenerateCells()
    {
        var cells = new List<Cell>();
        var gridObject = new GameObject("Grid");
        gridObject.transform.SetParent(instance.transform);

        for (int i = 0, x = 0; i < 10; i++, x+=1)
        {
            var cell = GameObject.Instantiate(cellPrefab, gridObject.transform);
            cell.transform.localPosition = new Vector3(x, 0, 0);
            cells.Add(cell);
        }
        return cells;
    }
}
