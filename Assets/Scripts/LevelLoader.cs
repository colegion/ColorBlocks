using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using GameObjects;
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
    [SerializeField] private Exit exit;
    
    [SerializeField] private GameConfig gameConfig;

    private void Start()
    {
        _currentLevel = JsonReader.ReadJSon("Level4");
        _levelController = new LevelController(_currentLevel.rowCount, _currentLevel.columnCount);
        _levelController.SetMoveLimit(_currentLevel.moveLimit);
        
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
            tempCell.InjectCellData(new CellAttributes(element.row, element.column));
        }
    }

    private void SpawnBlocks()
    {
        var movables = _currentLevel.movableInfo;
        foreach (var element in movables)
        {
            var pooledBlock = PuzzleObjectPool.Instance.GetAvailableBlock(element.length); 
            var tempBlock = Instantiate(gameConfig.GetPrefabByLength(element.length), transform);
            tempBlock.InjectBlockData(element, gameConfig);
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
