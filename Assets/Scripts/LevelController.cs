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
    
    /*
    public void InstantiateGrids(int row, int column)
    {
        _cellGrid = new Cell[row, column];
        _blockGrid = new Block[row, column];
    }*/

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

    public List<CellAttributes> GetPath(CellAttributes current, Direction direction)
    {
        List<CellAttributes> path = new List<CellAttributes>();
        var directionVector = DirectionVectors[direction];
        var nextPos = new CellAttributes(current.row + directionVector.x, current.column + directionVector.y);
        while (IsValidCoordinate(nextPos))
        {
            if (_blockGrid[nextPos.row, nextPos.column] == null)
            {
                path.Add(nextPos);
            }
            nextPos = new CellAttributes(nextPos.row + directionVector.x, nextPos.column + directionVector.y);
        }
        
        return path;
    }

    public void UpdateBlockPositionOnGrid(Block block, CellAttributes initialPosition)
    {
        var position = block.transform.position;
        CellAttributes cell = new CellAttributes((int)position.x, (int)-position.z);
        if (IsValidCoordinate(cell) && IsValidCoordinate(initialPosition))
        {
            _blockGrid[cell.row, cell.column] = block;
            _blockGrid[initialPosition.row, initialPosition.column] = null;
        }
        else
        {
            Debug.LogError("There might be a calculation error");
        }
    }

    private bool IsValidCoordinate(CellAttributes coordinate)
    {
        return _cellGrid.GetLength(0) > coordinate.row && coordinate.row >= 0 && _cellGrid.GetLength(1) > coordinate.column && coordinate.column >= 0;
    }
}
