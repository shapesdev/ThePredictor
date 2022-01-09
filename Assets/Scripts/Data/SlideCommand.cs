using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideCommand : ICellCommand
{
    public void Execute() {
        Debug.Log("Sliding");
    }

    public string GetCommandName() {
        return "Slide";
    }
}
