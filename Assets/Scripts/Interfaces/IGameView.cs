using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IGameView
{
    void Init();
    void DisplayCommands();
    event EventHandler<CellSelectionEventArgs> OnCellSelection;
}
