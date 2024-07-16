using System;
using System.Collections.Generic;
using GameObjects;
using UnityEngine;
using Utilities;
using static Utilities.CommonFields;

public class LevelController
{
    private Cell[,] _cellGrid;
    private Block[,] _blockGrid;
    
    private int _moveLimit;
    private int _blockCount;
    private bool _moveLimitEnabled;
    private bool _levelFinishedEventTriggered;

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
        foreach (var coordinate in block.GetCellAttributesList())
        {
            if (!IsValidCoordinate(coordinate))
            {
                
            }
            _blockGrid[coordinate.row, coordinate.column] = block;
        }
    }

    public void MoveBlock(Block block, Direction direction)
    {
        var initialPos = block.GetCellAttribute(direction);
        var nextPos = GetNextPosition(initialPos, direction);
        var finalPos = initialPos;

        while (!IsCellBlocked(nextPos))
        {
            finalPos = nextPos;
            nextPos = GetNextPosition(finalPos, direction);
        }

        HandleBlockMovement(block, direction, finalPos);
        DecreaseMoveLimit();
    }

    private CellAttributes GetNextPosition(CellAttributes pos, Direction direction)
    {
        var directionVector = DirectionVectors[direction];
        return new CellAttributes(pos.row + directionVector.x, pos.column + directionVector.y);
    }

    private void HandleBlockMovement(Block block, Direction direction, CellAttributes finalPos)
    {
        var finalCell = TryGetCell(finalPos);
        var exit = finalCell?.GetExitByDirection(direction);
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
            exit?.AnimateObject(isSuccessful, direction);
            block.AnimateObject(isSuccessful, direction);
        });
    }

    public bool IsCellBlocked(CellAttributes pos)
    {
        return !IsValidCoordinate(pos) || _blockGrid[pos.row, pos.column] != null;
    }

    private bool IsSuccessfulMovement(Block block, Exit exit)
    {
        return exit != null && exit.GetColor() == block.GetColor();
    }

    private void UpdateBlockPositionOnGrid(Block block, CellAttributes finalPos, Direction direction)
    {
        ClearBlockGridPositions(block.GetCellAttributesList());
        var updatedCells = GetUpdatedBlockPositions(block, finalPos, direction);
        block.SetCellAttributes(updatedCells);
    }

    private void ClearBlockGridPositions(IEnumerable<CellAttributes> positions)
    {
        foreach (var coordinate in positions)
        {
            if (IsValidCoordinate(coordinate))
            {
                _blockGrid[coordinate.row, coordinate.column] = null;
            }
        }
    }

    private List<CellAttributes> GetUpdatedBlockPositions(Block block, CellAttributes finalPos, Direction direction)
    {
        var updatedCells = new List<CellAttributes>();
        var oppositeDirection = block.GetOppositeDirection(direction);

        block.SetOrientation(direction, finalPos);
        for (int i = 0; i < block.GetLength(); i++)
        {
            var coordinate = new CellAttributes(finalPos.row + DirectionVectors[oppositeDirection].x * i,
                                                finalPos.column + DirectionVectors[oppositeDirection].y * i);
            updatedCells.Add(coordinate);
            block.SetOrientation(oppositeDirection, coordinate);
            _blockGrid[coordinate.row, coordinate.column] = block;
        }

        return updatedCells;
    }

    private void RemoveBlockFromGrid(Block block)
    {
        ClearBlockGridPositions(block.GetCellAttributesList());
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

    private void CheckLevelCompletion()
    {
        if (_levelFinishedEventTriggered)
        {
            return;
        }

        if (IsLevelComplete())
        {
            _levelFinishedEventTriggered = true;
            EventBus.Instance.Trigger(new LevelFinishedEvent(_blockCount == 0));
        }
    }

    private bool IsLevelComplete()
    {
        return _blockCount == 0 || (_moveLimitEnabled && _moveLimit == 0);
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
        return coordinate.row >= 0 && coordinate.row < _cellGrid.GetLength(0) && 
               coordinate.column >= 0 && coordinate.column < _cellGrid.GetLength(1);
    }
}
