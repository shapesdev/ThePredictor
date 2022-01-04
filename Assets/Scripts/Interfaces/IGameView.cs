using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IGameView
{
    void Init(IEnumerable<ICellCommand> possibleCommands);
    void EnableCommandButtons();
    event EventHandler<CellSelectionEventArgs> OnCellSelection;
}
