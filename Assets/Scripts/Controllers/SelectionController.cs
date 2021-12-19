using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController
{
    private List<ICellView> selectedCells;
    private ICellView currentCell = null;
    private Vector3 lastPoint;

    public SelectionController() {
        selectedCells = new List<ICellView>();
        currentCell = null;
        lastPoint = Vector3.zero;
    }

    public List<ICellView> HandleSelectionInputs() {
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
            Debug.Log("Releasing");
            return selectedCells;
        }
        return null;
    }

    private void SelectCell(RaycastHit hit) {
        var cell = hit.transform.gameObject.GetComponent<ICellView>();
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

    private void DeselectAll() {
        foreach(var cell in selectedCells) {
            cell.Deselect();
        }
        currentCell = null;
    }
}
