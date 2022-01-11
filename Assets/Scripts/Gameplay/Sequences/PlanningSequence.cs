using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlanningSequence : MonoBehaviour, ISequence
{
    [SerializeField]
    private PlanningPhaseUI planningPhaseUI;
    private CellSelection cellSelection;

    public void Init()
    {
        gameObject.SetActive(true);

        cellSelection = new CellSelection();

        StartCoroutine(CheckCellSelections());
        planningPhaseUI.Init(new List<ICellCommand>() {
        new JumpCommand(),
        new SlideCommand(),
        new StopCommand(),
        new WalkCommand(),
        new RunCommand()
        });

        planningPhaseUI.OnGetPressedCommand += PlanningPhaseUI_OnGetPressedCommand;
    }

    public void Terminate()
    {
        gameObject.SetActive(false);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            DisableCommandButtons();
        }
    }

    private IEnumerator CheckCellSelections() {
        while(true) {
            yield return null;
            var selectedCells = cellSelection.GetSelectedCells();
            if (selectedCells != null && selectedCells.Count > 0) {
                EnableCommandButtons();
                break;
            }
        }
    }

    private void PlanningPhaseUI_OnGetPressedCommand(ICellCommand cmd) {
        var selectedCells = cellSelection.GetSelectedCells();
        foreach(var cell in selectedCells) {
            cell.SetCommand(cmd);
        }
        DisableCommandButtons();
    }

    private void EnableCommandButtons() {
        planningPhaseUI.gameObject.SetActive(true);
    }

    public void DisableCommandButtons() {
        cellSelection.DeselectAll();
        planningPhaseUI.gameObject.SetActive(false);
        StartCoroutine(CheckCellSelections());
    }
}
