using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanningPhaseUI : MonoBehaviour
{
    [SerializeField]
    private Button[] commandButtons;

    public event Action<ICellCommand> OnGetPressedCommand;

    public void Init(IEnumerable<ICellCommand> cellCommands)
    {
        int current = 0;

        foreach (var cmd in cellCommands)
        {
            commandButtons[current].onClick.AddListener(() => UpdateCurrentCommand(cmd));
            commandButtons[current].GetComponentInChildren<Text>().text = cmd.GetCommandName();
            current++;
        }
    }

    private void UpdateCurrentCommand(ICellCommand cellCommand) {
        OnGetPressedCommand(cellCommand);
    }
}
