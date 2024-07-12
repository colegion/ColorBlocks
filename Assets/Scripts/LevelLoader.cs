using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Scriptables;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;
using static Utilities.CommonFields;

public class LevelLoader : MonoBehaviour
{
    private LevelController _levelController;
    private LevelInfo _currentLevel;
    private int _levelIndex = 1;

    [SerializeField] private Cell cell;
    [SerializeField] private Block block;
    [SerializeField] private Exit exit;
    
    [SerializeField] private GameConfig gameConfig;

    private void Start()
    {
        _currentLevel = JsonReader.ReadJSon("Level4");
        _levelController = new LevelController(_currentLevel.rowCount, _currentLevel.columnCount);
        
        SpawnCells();
        SpawnBlocks();
        SpawnExits();
        EventBus.Instance.Trigger(new ControllerReadyEvent(_levelController));
    }

    private void SpawnCells()
    {
        var cells = _currentLevel.cellInfo;
        foreach (var element in cells)
        {
            var tempCell = Instantiate(cell, transform);
            _levelController.AssignObjectByType(new CellAttributes(element.row, element.column), tempCell.gameObject);
            tempCell.ConfigureSelf(new CellAttributes(element.row, element.column));
        }
    }

    private void SpawnBlocks()
    {
        var movables = _currentLevel.movableInfo;
        foreach (var element in movables)
        {
            var tempBlock = Instantiate(block, transform);
            _levelController.AssignObjectByType(new CellAttributes(element.row, element.column), tempBlock.gameObject);
            tempBlock.ConfigureSelf(gameConfig.GetConfigByColor(element.color), element, gameConfig.GetMeshByLength(element.length));
        }
    }

    private void SpawnExits()
    {
        var exits = _currentLevel.exitInfo;
        foreach (var element in exits)
        {
            var tempGate = Instantiate(exit, transform);
            tempGate.ConfigureSelf(element);
        }
    }
}
