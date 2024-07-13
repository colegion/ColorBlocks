using System;
using System.Collections;
using System.Collections.Generic;
using GameObjects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using static Utilities.CommonFields;

public class LevelController
{
    private Cell[,] _cellGrid;
    private Block[,] _blockGrid;

    public LevelController(int row, int column)
    {
        _cellGrid = new Cell[row, column];
        _blockGrid = new Block[row, column];
    }

    public void AssignCell(Cell cell)
    {
        var coordinate = cell.GetCellAttribute();
        _cellGrid[coordinate.row, coordinate.column] = cell;
    }

    public void AssignBlock(Block block)
    {
        var blockList = block.GetCellAttributes_2();
        foreach (var coordinate in blockList)
        {
            _blockGrid[coordinate.row, coordinate.column] = block;
        }
    }

    public void MoveBlock(Block block, Direction direction)
    {
        var initialPos = block.GetCellAttributes(direction);
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
        
        block.MoveBlock(finalPos, direction, () =>
        {
            if (exit != null) exit.AnimateObject(isSuccessful);
            block.AnimateObject(isSuccessful);
        });
        
        if (isSuccessful)
        {
            RemoveBlockFromGrid(block.GetCellAttributes_2());
        }
        else
        {
            UpdateBlockPositionOnGrid_2(block, finalPos, direction);
        }
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
    
    private void UpdateBlockPositionOnGrid_2(Block block, CellAttributes finalPosition, Direction direction)
    {
        var currentPos = block.GetCellAttributes_2();
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

    private void RemoveBlockFromGrid(List<CellAttributes> positions)
    {
        foreach (var coordinate in positions)
        {
            if (IsValidCoordinate(coordinate))
            {
                _blockGrid[coordinate.row, coordinate.column] = null;
            }
        }
    }

    public void SetCellExit(Exit exit)
    {
        var coordinates = exit.GetCellAttributes();
        var cell = TryGetCell(coordinates);
        cell.SetExitByDirection(exit);
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
