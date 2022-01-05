using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkCommand : ICellCommand
{
    public void Execute() {
        Debug.Log("Walking");
    }

    public string GetCommandName() {
        return "Walk";
    }
}
