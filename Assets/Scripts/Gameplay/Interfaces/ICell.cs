using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICell 
{
    void Select();
    void Deselect();
    bool IsSelected();
    void SetCommand(ICellCommand cellCommand);
    void ExecuteCommand();
}
