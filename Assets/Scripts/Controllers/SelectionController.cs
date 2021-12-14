using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController
{
    private List<ICellView> selectedCells;

    private Vector2 startPoint;
    private Vector2 endPoint;

    public SelectionController() {
        selectedCells = new List<ICellView>(0);
        startPoint = Vector2.zero;
    }

    public void HandleSelectionInputs(RectTransform selectionBox) {
        if (Input.GetMouseButtonDown(0)) {
            startPoint = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 20)) {
                SelectCell(hit);
            }
            else {
                DeselectAll();
            }
        }
        else if (Input.GetMouseButton(0)) {
            endPoint = Input.mousePosition;
            if((startPoint - endPoint).magnitude > 20) {
                UpdateSelectionBox(Input.mousePosition, selectionBox);
            }
        }
        else if(Input.GetMouseButtonUp(0)) {
            ReleaseSelectionBox(selectionBox);
        }
    }

    private void UpdateSelectionBox(Vector2 curMousePos, RectTransform selectionBox) {
        if (!selectionBox.gameObject.activeInHierarchy) {
            selectionBox.gameObject.SetActive(true);
        }
        float width = curMousePos.x - startPoint.x;
        float height = curMousePos.y - startPoint.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPoint + new Vector2(width / 2, height / 2);
    }

    private void ReleaseSelectionBox(RectTransform selectionBox) {
        selectionBox.gameObject.SetActive(false);

        var mid = (startPoint + endPoint) / 2;
        float width = Mathf.Abs(endPoint.x - startPoint.x) / 2;
        float height = Mathf.Abs(endPoint.y - startPoint.y) / 2;
        var extents = new Vector3(width, height, 0);

        var hits = Physics.BoxCastAll(mid, extents, Vector3.one, Quaternion.identity, 20);

        Debug.Log(hits.Length);
    }

    private void SelectCell(RaycastHit hit) {
        var cell = hit.transform.gameObject.GetComponent<ICellView>();
        if (!selectedCells.Contains(cell)) {
            cell.Select();
            selectedCells.Add(cell);
        }
        else {
            cell.Deselect();
            selectedCells.Remove(cell);
        }
    }

    private void DeselectAll() {
        foreach (var cell in selectedCells) {
            cell.Deselect();
        }
        selectedCells.Clear();
    }
}
