using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using static Utilities.CommonFields;

public class Cell : MonoBehaviour
{
    private Dictionary<Direction, Exit> _exits = new Dictionary<Direction, Exit>();
    public void ConfigureSelf(CellAttributes position)
    {
        transform.position = new Vector3(position.row, 0, position.column);
    }

    public void SetExitByDirection(Exit exit)
    {
        _exits[(Direction)exit.GetDirection()] = exit;
    }

    public Exit GetExitByDirection(Direction direction)
    {
        return _exits[direction] != null ? _exits[direction] : throw new Exception("No exit in that direction");
    }
}
