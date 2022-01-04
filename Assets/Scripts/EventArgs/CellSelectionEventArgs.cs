using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CellSelectionEventArgs : EventArgs
{
    public IEnumerable<ICell> SelectedCells { get; }

    public CellSelectionEventArgs(IEnumerable<ICell> selectedCells) {
        SelectedCells = selectedCells;
    }
}
