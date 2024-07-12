using System;
using System.Collections;
using System.Collections.Generic;
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

    public void AssignObjectByType(CellAttributes coordinates, GameObject obj)
    {
        if (obj.TryGetComponent(out Cell cell))
        {
            _cellGrid[coordinates.row, coordinates.column] = cell;
        }
        else if (obj.TryGetComponent(out Block block))
        {
            _blockGrid[coordinates.row, coordinates.column] = block;
        }
    }

    public void MoveBlock(Block block, Direction direction)
    {
        var initialPos = block.GetCellAttributes();
        var directionVector = DirectionVectors[direction];
        var nextPos = new CellAttributes(initialPos.row + directionVector.x, initialPos.column + directionVector.y);
        CellAttributes finalPos = null;
        while (IsValidCoordinate(nextPos))
        {
            if (_blockGrid[nextPos.row, nextPos.column] != null)
            {
                break;
            }
            
            finalPos = nextPos;
            nextPos = new CellAttributes(nextPos.row + directionVector.x, nextPos.column + directionVector.y);
        }

        var finalCell = TryGetCell(finalPos);
        var exit = finalCell.GetExitByDirection(direction);
        var isSuccessful = IsSuccessfulMovement(block, exit);
        
        if (isSuccessful)
        {
            RemoveBlockFromGrid(initialPos);
        }
        else
        {
            UpdateBlockPositionOnGrid(block, finalPos, initialPos);
        }
        
        block.MoveBlock(finalPos, () =>
        {
            exit.PlayParticle(isSuccessful);
            block.AnimateBlock(isSuccessful);
        });
    }

    private bool IsSuccessfulMovement(Block block, Exit exit)
    {
        return exit != null && exit.GetColor() == block.GetColor();
    }

    private void UpdateBlockPositionOnGrid(Block block, CellAttributes finalPosition, CellAttributes initialPosition)
    {
        _blockGrid[initialPosition.row, initialPosition.column] = null;
        _blockGrid[finalPosition.row, finalPosition.column] = block;
    }

    private void RemoveBlockFromGrid(CellAttributes position)
    {
        if (IsValidCoordinate(position))
        {
            _blockGrid[position.row, position.column] = null;
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
        return IsValidCoordinate(coordinates) ? _cellGrid[coordinates.row, coordinates.column] : throw new Exception("No suitable cell for this action");
    }

    private bool IsValidCoordinate(CellAttributes coordinate)
    {
        return _cellGrid.GetLength(0) > coordinate.row && coordinate.row >= 0 && _cellGrid.GetLength(1) > coordinate.column && coordinate.column >= 0;
    }
}
