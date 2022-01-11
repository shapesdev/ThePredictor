using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICellCommand 
{
    void Execute();
    string GetCommandName();
}
