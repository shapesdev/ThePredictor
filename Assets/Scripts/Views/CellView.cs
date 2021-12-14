using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellView : MonoBehaviour, ICellView
{
    private MeshRenderer meshRenderer;

    private void Start() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Select() {
        meshRenderer.material.color = Color.green;
    }

    public void Deselect() {
        meshRenderer.material.color = Color.white;
    }
}
