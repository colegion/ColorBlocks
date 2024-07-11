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

    public void AssignObjectByType(Vector2Int coordinates, GameObject obj)
    {
        if (obj.TryGetComponent(out Cell cell))
        {
            _cellGrid[coordinates.x, coordinates.y] = cell;
        }
        else if (obj.TryGetComponent(out Block block))
        {
            _blockGrid[coordinates.x, coordinates.y] = block;
        }
    }

    public List<Vector2Int> GetPath(Vector2Int current, Direction direction)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        var directionVector = DirectionVectors[direction];
        var nextPos = new Vector2Int(current.x + directionVector.x, current.y + directionVector.y);
        while (IsValidCoordinate(nextPos))
        {
            if (_blockGrid[nextPos.x, nextPos.y] == null)
            {
                path.Add(nextPos);
            }
            nextPos = new Vector2Int(nextPos.x + directionVector.x, nextPos.y + directionVector.y);
        }
        
        return path;
    }

    private bool IsValidCoordinate(Vector2Int coordinate)
    {
        return _cellGrid.GetLength(0) > coordinate.x && coordinate.x >= 0 && _cellGrid.GetLength(1) > coordinate.y && coordinate.y >= 0;
    }
}
