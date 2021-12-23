using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModel : IGameModel
{
    private List<ICellCommand> possibleCellCommands;

    public void Init() {
        possibleCellCommands = new List<ICellCommand>();

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
