using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : ICellCommand
{
    public void Execute() {
        Debug.Log("Jumping");
    }

    public string GetCommandName() {
        return "Jump";
    }
}
