using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSelection
{
    private List<ICell> selectedCells;
    private ICell currentCell = null;
    private Vector3 lastPoint;

    public CellSelection() {
        selectedCells = new List<ICell>();
        currentCell = null;
        lastPoint = Vector3.zero;
    }

    public List<ICell> GetSelectedCells() {
        if (Input.GetMouseButton(0) && lastPoint != Input.mousePosition) {
            lastPoint = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(lastPoint);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 20, 1<<8)) {
                SelectCell(hit);
            }
            else if(currentCell != null) {
                currentCell = null;
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            DeselectAll();
        }
        if(Input.GetMouseButtonUp(0)) {
            return selectedCells;
        }
        return null;
    }

    private void SelectCell(RaycastHit hit) {
        var cell = hit.transform.gameObject.GetComponent<ICell>();
        if(cell != currentCell) {
            if(!selectedCells.Contains(cell)) {
                selectedCells.Add(cell);
                cell.Select();
            }
            else if(!cell.IsSelected()) {
                cell.Select();
            }
            currentCell = cell;
        }
    }

    public void DeselectAll() {
        foreach(var cell in selectedCells) {
            cell.Deselect();
        }
        selectedCells.Clear();
        currentCell = null;
    }
}
