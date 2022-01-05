using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCommand : ICellCommand
{
    public void Execute() {
        Debug.Log("Running");
    }

    public string GetCommandName() {
        return "Run";
    }
}
