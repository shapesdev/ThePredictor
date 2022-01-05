using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlanningSequence : MonoBehaviour, ISequence
{
    [SerializeField]
    private PlanningPhaseMenu planningPhaseMenu;

    private CellSelection cellSelection;
    private List<ICellCommand> cellCommands;

    public void Start()
    {
        gameObject.SetActive(true);
        cellCommands = new List<ICellCommand>();
        cellSelection = new CellSelection();

        cellCommands.Add(new JumpCommand());
        cellCommands.Add(new SlideCommand());
        cellCommands.Add(new StopCommand());
        cellCommands.Add(new WalkCommand());
        cellCommands.Add(new RunCommand());

        planningPhaseMenu.Init(cellCommands);
    }

    public void Stop()
    {
        gameObject.SetActive(false);
    }

    public void Update()
    {
        var selectedCells = cellSelection.GetSelectedCells();
        if (selectedCells != null && selectedCells.Count > 0)
        {
            EnableCommandButtons();
        }
    }

    private void EnableCommandButtons()
    {
        planningPhaseMenu.gameObject.SetActive(true);
    }

    public void DisableCommandButtons()
    {
        cellSelection.DeselectAll();
        planningPhaseMenu.gameObject.SetActive(false);
    }
}
