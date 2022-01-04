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

    public void Init(IEnumerable<ICellCommand> possibleCommands) {
        selectionController = new SelectionController();
        int current = 0;

        foreach(var cmd in possibleCommands) {
            commandButtons[current].onClick.AddListener(() => cmd.Execute());
            commandButtons[current].onClick.AddListener(() => DisableCommandButtons());
            commandButtons[current].GetComponentInChildren<Text>().text = cmd.GetCommandName();
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

    public void EnableCommandButtons() {
        commandButtons[0].transform.parent.gameObject.SetActive(true);
    }

    private void DisableCommandButtons() {
        selectionController.DeselectAll();
        commandButtons[0].transform.parent.gameObject.SetActive(false);
    }
}
