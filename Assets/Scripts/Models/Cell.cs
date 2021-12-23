using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private ICellCommand cellCommand;

    public Cell() {

    }

    public void SetCommand(ICellCommand cellCommand) {
        this.cellCommand = cellCommand;
    }

    public void ExecuteCommandOnCell() {
        if(cellCommand != null) {
            cellCommand.Execute();
        }
    }
}
