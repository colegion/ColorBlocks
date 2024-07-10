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
    [SerializeField] private LevelController levelController;
    private Cell[,] _cellGrid;
    private Block[,] _blockGrid;
    private LevelInfo _currentLevel;
    private int _levelIndex = 1;

    [SerializeField] private Cell cell;
    [SerializeField] private Block block;
    [SerializeField] private Gate gate;

    [SerializeField] private List<BlockConfig> blockConfigs;
    private void Start()
    {
        _currentLevel = JsonReader.ReadJSon("Level1");
        _cellGrid = new Cell[_currentLevel.rowCount, _currentLevel.columnCount];
        
        SpawnCells();
        SpawnBlocks();
        SpawnExits();
        EventBus.Instance.Trigger(new ControllerReadyEvent(levelController));
    }

    private void SpawnCells()
    {
        var cells = _currentLevel.cellInfo;

        foreach (var element in cells)
        {
            var tempCell = Instantiate(cell, transform);
            _cellGrid[element.row, element.column] = tempCell;
            tempCell.ConfigureSelf(new Vector2(element.row, element.column));
        }
    }

    private void SpawnBlocks()
    {
        var movables = _currentLevel.movableInfo;

        foreach (var element in movables)
        {
            var tempBlock = Instantiate(block, transform);
            _blockGrid[element.row, element.column] = tempBlock;
            tempBlock.ConfigureSelf(blockConfigs[element.color], element);
        }
    }

    private void SpawnExits()
    {
        var exits = _currentLevel.exitInfo;

        foreach (var element in exits)
        {
            var tempGate = Instantiate(gate, transform);
            tempGate.ConfigureSelf(element);
        }
    }
}
