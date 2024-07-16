using System;
using System.Collections;
using System.Collections.Generic;
using GameObjects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using static Utilities.CommonFields;
using EventBus = Utilities.EventBus;

public class LevelController
{
    private Cell[,] _cellGrid;
    private Block[,] _blockGrid;
    
    private int _moveLimit;
    private int _blockCount;
    private bool _moveLimitEnabled;

    public LevelController(int row, int column)
    {
        _cellGrid = new Cell[row, column];
        _blockGrid = new Block[row, column];
        _levelFinishedEventTriggered = false;
        _blockCount = 0;
    }

    public void SetMoveLimit(int count)
    {
        _moveLimit = count;
        _moveLimitEnabled = _moveLimit > 0;
        EventBus.Instance.Trigger(new MovementEvent(_moveLimit));
    }
    
    public void AssignCell(Cell cell)
    {
        var coordinate = cell.GetCellAttribute();
        _cellGrid[coordinate.row, coordinate.column] = cell;
    }

    public void AssignBlock(Block block)
    {
        _blockCount++;
        var blockList = block.GetCellAttributesList();
        foreach (var coordinate in blockList)
        {
            _blockGrid[coordinate.row, coordinate.column] = block;
        }
    }

    public void MoveBlock(Block block, Direction direction)
    {
        var initialPos = block.GetCellAttribute(direction);
        var directionVector = DirectionVectors[direction];
        var nextPos = new CellAttributes(initialPos.row + directionVector.x, initialPos.column + directionVector.y);
        CellAttributes finalPos = initialPos;
        while (!IsCellBlocked(nextPos))
        {
            finalPos = nextPos;
            nextPos = new CellAttributes(nextPos.row + directionVector.x, nextPos.column + directionVector.y);
        }

        var finalCell = TryGetCell(finalPos);
        var exit = finalCell != null ? finalCell.GetExitByDirection(direction) : null;
        var isSuccessful = IsSuccessfulMovement(block, exit);
        
        if (isSuccessful)
        {
            RemoveBlockFromGrid(block);
        }
        else
        {
            UpdateBlockPositionOnGrid(block, finalPos, direction);
        }
        
        block.MoveBlock(finalPos, direction, () =>
        {
            if (exit != null) exit.AnimateObject(isSuccessful);
            block.AnimateObject(isSuccessful);
        });
        
        DecreaseMoveLimit();
    }

    public bool IsCellBlocked(CellAttributes nextPos)
    {
        if (!IsValidCoordinate(nextPos)) return true;
        if (_blockGrid[nextPos.row, nextPos.column] != null)
        {
            return true;
        }

        return false;
    }

    private bool IsSuccessfulMovement(Block block, Exit exit)
    {
        return exit != null && exit.GetColor() == block.GetColor();
    }
    
    private void UpdateBlockPositionOnGrid(Block block, CellAttributes finalPosition, Direction direction)
    {
        var currentPos = block.GetCellAttributesList();
        foreach (var coordinate in currentPos)
        {
            _blockGrid[coordinate.row, coordinate.column] = null;
        }
        
        var oppositeDirection = block.GetOppositeDirection(direction);
        List<CellAttributes> updatedCells = new List<CellAttributes>();
        block.SetOrientation(direction, finalPosition);
        if (block.GetLength() == 1)
        {
            updatedCells.Add(finalPosition);
        }
        var iterations = block.GetLength();
        for (int i = 0; i < iterations; i++)
        {
            var coordinate = new CellAttributes(finalPosition.row + (DirectionVectors[oppositeDirection].x * i),
                finalPosition.column + (DirectionVectors[oppositeDirection].y * i));
            updatedCells.Add(coordinate);
            block.SetOrientation(oppositeDirection, coordinate);
            _blockGrid[coordinate.row, coordinate.column] = block;
        }
        
        block.SetCellAttributes(updatedCells);
    }

    private void RemoveBlockFromGrid(Block block)
    {
        var positions = block.GetCellAttributesList();
        foreach (var coordinate in positions)
        {
            if (IsValidCoordinate(coordinate))
            {
                _blockGrid[coordinate.row, coordinate.column] = null;
            }
        }
        DecreaseRemainingBlockCount();
    }

    public void SetCellExit(Exit exit)
    {
        var coordinates = exit.GetCellAttributes();
        var cell = TryGetCell(coordinates);
        cell.SetExitByDirection(exit);
    }
    
    private void DecreaseMoveLimit()
    {
        if (_moveLimitEnabled)
        {
            _moveLimit--;
            EventBus.Instance.Trigger(new MovementEvent(_moveLimit));
            CheckLevelCompletion();
        }
    }

    private void DecreaseRemainingBlockCount()
    {
        _blockCount--;
        CheckLevelCompletion();
    }
    
    private bool _levelFinishedEventTriggered = false;
    private void CheckLevelCompletion()
    {
        if (_levelFinishedEventTriggered)
        {
            return;
        }

        bool isLevelComplete = _blockCount == 0 || (_moveLimitEnabled && _moveLimit == 0);

        if (isLevelComplete)
        {
            _levelFinishedEventTriggered = true;
            bool isSuccess = _blockCount == 0;
            EventBus.Instance.Trigger(new LevelFinishedEvent(isSuccess));
        }
    }

    public int GetHeight()
    {
        return _cellGrid.GetLength(1);
    }

    public int GetWidth()
    {
        return _cellGrid.GetLength(0);
    }

    private Cell TryGetCell(CellAttributes coordinates)
    {
        return IsValidCoordinate(coordinates) ? _cellGrid[coordinates.row, coordinates.column] : null;
    }

    private bool IsValidCoordinate(CellAttributes coordinate)
    {
        return _cellGrid.GetLength(0) > coordinate.row && coordinate.row >= 0 && _cellGrid.GetLength(1) > coordinate.column && coordinate.column >= 0;
    }
}
