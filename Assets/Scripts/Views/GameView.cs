using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour, IGameView
{
    private SelectionController selectionController;

    public void Init() {
        selectionController = new SelectionController();
    }

    private void Update() {
        selectionController.HandleSelectionInputs();
    }
}
