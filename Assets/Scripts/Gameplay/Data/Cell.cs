using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour, ICell
{
    private ICellCommand cellCommand;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        cellCommand = null;
    }

    public void Select()
    {
        meshRenderer.material.color = Color.green;
    }

    public void Deselect()
    {
        meshRenderer.material.color = Color.white;
    }
    public bool IsSelected()
    {
        return meshRenderer.material.color == Color.green ? true : false;
    }

    public void SetCommand(ICellCommand cellCommand) {
        this.cellCommand = cellCommand;
    }

    public void ExecuteCommand() {
        if(cellCommand != null) {
            cellCommand.Execute();
        }
    }
}
