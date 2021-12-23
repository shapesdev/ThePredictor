using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCommand : ICellCommand
{
    public void Execute() {
        Debug.Log("Stopping");
    }

    public string GetCommandName() {
        return "Stop";
    }
}
