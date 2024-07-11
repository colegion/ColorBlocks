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

    public List<CellAttributes> GetPath(Block block, Direction direction)
    {
        List<CellAttributes> path = new List<CellAttributes>();
        var initialPos = block.GetCellAttributes();
        var directionVector = DirectionVectors[direction];
        var nextPos = new CellAttributes(initialPos.row + directionVector.x, initialPos.column + directionVector.y);
        CellAttributes currentPos = null;
        while (IsValidCoordinate(nextPos))
        {
            if (_blockGrid[nextPos.row, nextPos.column] == null)
            {
                path.Add(nextPos);
            }

            currentPos = nextPos;
            nextPos = new CellAttributes(nextPos.row + directionVector.x, nextPos.column + directionVector.y);
        }

        var finalCell = TryGetCell(currentPos);
        var exit = finalCell.GetExitByDirection(direction);
        if (exit != null && exit.GetColor() == block.GetColor())
        {
            path.Add(exit.GetExitPosition());
        }
        
        UpdateBlockPositionOnGrid(block, currentPos, initialPos);
        return path;
    }

    private void UpdateBlockPositionOnGrid(Block block, CellAttributes finalPosition, CellAttributes initialPosition)
    {
        if (IsValidCoordinate(initialPosition))
        {
            _blockGrid[initialPosition.row, initialPosition.column] = null;
        }
        
        if (IsValidCoordinate(finalPosition))
        {
            _blockGrid[finalPosition.row, finalPosition.column] = block;
        }
        else
        {
            //@todo: means block got out of the grid using exit. hard mind mapping. need to find better way.
            Debug.LogError("There might be a calculation error");
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
