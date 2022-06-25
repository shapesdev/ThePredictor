using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private Vector2Int boardSize = new Vector2Int(11, 11);

    [SerializeField]
    private GameBoard board = default;

    [SerializeField]
    private GameTileContentFactory tileContentFactory = default;

    private Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    private void Awake() {
        board.Init(boardSize, tileContentFactory);
        board.ShowGrid = true;
    }

    private void OnValidate() {
        if(boardSize.x < 2) {
            boardSize.x = 2;
        }
        if(boardSize.y < 2) {
            boardSize.y = 2;
        }
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            HandleTouch();
        }
        else if(Input.GetMouseButtonDown(1)) {
            HandleAlternativeTouch();
        }

        if(Input.GetKeyDown(KeyCode.V)) {
            board.ShowPaths = !board.ShowPaths;
        }
        if(Input.GetKeyDown(KeyCode.G)) {
            board.ShowGrid = !board.ShowGrid;
        }
    }

    private void HandleAlternativeTouch() {
        GameTile tile = board.GetTile(TouchRay);
        if(tile != null) {
            board.ToggleDestination(tile);
        }
    }

    private void HandleTouch() {
        GameTile tile = board.GetTile(TouchRay);
        if(tile != null) {
            board.ToggleWall(tile);
        }
    }
}
