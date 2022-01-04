using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModel : IGameModel
{
    private List<ICellCommand> possibleCellCommands;
    private List<Cell> cells;

    public GameModel(List<Cell> cells)
    {
        this.cells = cells;
    }
         
    public void Init()
    {
        possibleCellCommands = new List<ICellCommand>();
        cells = new List<Cell>();
        possibleCellCommands.Add(new JumpCommand());
        possibleCellCommands.Add(new SlideCommand());
        possibleCellCommands.Add(new StopCommand());
        possibleCellCommands.Add(new WalkCommand());
        possibleCellCommands.Add(new RunCommand());
    }

    public IEnumerable<ICellCommand> GetPossibleCommands() {
        return possibleCellCommands;
    }
}
