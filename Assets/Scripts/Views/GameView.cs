using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour, IGameView
{
    [SerializeField]
    private Button[] commandButtons;

    public event EventHandler<CellSelectionEventArgs> OnCellSelection;

    private SelectionController selectionController;
    private GameController gameController;

    // FOR NOW HAVING START METHOD
    private void Start() {
        Init();
    }

    public void Init() {
        selectionController = new SelectionController();
        var model = new GameModel();
        gameController = new GameController(null, model, this);

        int current = 0;
        foreach(var cmd in model.GetPossibleCommands()) {
            commandButtons[current].onClick.AddListener(() => cmd.Execute());
            commandButtons[current].onClick.AddListener(() => ClearSelections());
            commandButtons[current].GetComponentInChildren<Text>().text = cmd.GetCommandName();
            commandButtons[current].gameObject.SetActive(false);
            current++;
        }
    }

    // Could be a coroutine for Planning Phase instead of an Update
    private void Update() {
        var selectedCells = selectionController.HandleSelectionInputs();
        if(selectedCells != null && selectedCells.Count > 0 ) {
            var eventArgs = new CellSelectionEventArgs(selectedCells);
            OnCellSelection.Invoke(this, eventArgs);
        }
    }

    public void DisplayCommands() {
        foreach(var btn in commandButtons) {
            btn.gameObject.SetActive(true);
        }
    }

    private void ClearSelections() {
        selectionController.DeselectAll();
        foreach (var btn in commandButtons) {
            btn.gameObject.SetActive(false);
        }
    }
}
