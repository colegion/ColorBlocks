using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using static Utilities.CommonFields;

public class Cell : MonoBehaviour
{
    private Dictionary<Direction, Exit> _exits = new Dictionary<Direction, Exit>();
    private LevelController _controller;

    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    public void InjectCellData(CellAttributes position)
    {
        transform.position = new Vector3(position.row, 0, position.column);
    }

    public void SetExitByDirection(Exit exit)
    {
        _exits[(Direction)exit.GetDirection()] = exit;
    }

    public CellAttributes GetCellAttribute()
    {
        return new CellAttributes((int)transform.position.x, (int)transform.position.z);
    }

    public Exit GetExitByDirection(Direction direction)
    {
        return _exits.TryGetValue(direction, out var exit) ? exit : null;
    }
    
    private void InjectLevelController(ControllerReadyEvent eventData)
    {
        _controller = eventData.Controller;
        _controller.AssignCell(this);
    }
    
    private void AddListeners()
    {
        EventBus.Instance.Register<ControllerReadyEvent>(InjectLevelController);
    }

    private void RemoveListeners()
    {
        EventBus.Instance.Unregister<ControllerReadyEvent>(InjectLevelController);
    }
}
