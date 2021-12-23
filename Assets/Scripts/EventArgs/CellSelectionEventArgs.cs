using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CellSelectionEventArgs : EventArgs
{
    public IEnumerable<ICellView> SelectedCells { get; }

    public CellSelectionEventArgs(IEnumerable<ICellView> selectedCells) {
        SelectedCells = selectedCells;
    }
}
