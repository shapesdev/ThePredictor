using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour, IGameView
{
    private SelectionController selectionController;

    public RectTransform selectionBox;

    private void Start() {
        selectionController = new SelectionController();
    }

    public void Init() {
    }

    private void Update() {
        selectionController.HandleSelectionInputs(selectionBox);
    }
}
