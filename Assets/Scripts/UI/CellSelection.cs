using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSelection
{
    private List<ICell> selectedCells;

    public CellSelection() {
        selectedCells = new List<ICell>();
    }

    public List<ICell> GetSelectedCells() {
        if (Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 20, 1<<8)) {
                SelectCell(hit);
            }
        }
        if(Input.GetMouseButtonUp(0)) {
            return selectedCells;
        }
        return null;
    }

    private void SelectCell(RaycastHit hit) {
        var cell = hit.transform.gameObject.GetComponent<ICell>();
        if (!selectedCells.Contains(cell)) {
            selectedCells.Add(cell);
            cell.Select();
        }
        else if (!cell.IsSelected()) {
            cell.Select();
        }
    }

    public void DeselectAll() {
        foreach(var cell in selectedCells) {
            cell.Deselect();
        }
        selectedCells.Clear();
    }
}
