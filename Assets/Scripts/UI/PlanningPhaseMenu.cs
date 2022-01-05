using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanningPhaseMenu : MonoBehaviour
{
    [SerializeField]
    private Button[] commandButtons;

    public void Init(List<ICellCommand> cellCommands)
    {
        int current = 0;

        foreach (var cmd in cellCommands)
        {
            commandButtons[current].onClick.AddListener(() => cmd.Execute());
            commandButtons[current].GetComponentInChildren<Text>().text = cmd.GetCommandName();
            current++;
        }
    }
}
