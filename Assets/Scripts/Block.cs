using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Interfaces;
using Scriptables;
using UnityEngine;
using Utilities;
using static Utilities.CommonFields;

public class Block : MonoBehaviour, IMovable
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;
    
    private LevelController _controller;
    private BlockConfig _config;
    private GameConfig _gameConfig;
    private MovableAttributes _movableAttributes;
    private Direction[] _directions;
    private Direction _mainDirection;

    private List<CellAttributes> _cellAttributes = new List<CellAttributes>();
    private Dictionary<Direction, CellAttributes> _orientation;

    private void OnValidate()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
    }

    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    public void InjectBlockData(MovableAttributes attributes, GameConfig gameData)
    {
        _config = gameData.GetConfigByColor(attributes.color);
        _movableAttributes = attributes;
        _gameConfig = gameData;
    }

    private void ConfigureDirectionArray()
    {
        _directions = new [] 
        {
            (Direction)_movableAttributes.directions[0],
            (Direction)_movableAttributes.directions[1]
        };

        _mainDirection = _directions[0];
    }
    
    private void ConfigureMesh(GameConfig gameData)
    {
        meshRenderer.material.mainTexture = _config.GetTextureByLength(_movableAttributes.length, _directions[0]);
        meshFilter.mesh = gameData.GetMeshByLength(_movableAttributes.length);
    }
      
    private void ConfigureTransform()
    {
        _orientation = new Dictionary<Direction, CellAttributes>();
        CellAttributes position = new CellAttributes(_movableAttributes.row, _movableAttributes.column);
        transform.position = new Vector3(_movableAttributes.row, 0, _movableAttributes.column);
        _cellAttributes.Add(position);
        for (int i = _directions.Length-1; i >= 0; i--)
        {
            position = new CellAttributes(_movableAttributes.row + DirectionVectors[_directions[i]].x * (_movableAttributes.length -1),
                _movableAttributes.column + DirectionVectors[_directions[i]].y * (_movableAttributes.length -1));
            if (!_controller.IsCellBlocked(position))
            {
                SetOrientation(_directions[i], position);
                SetOrientation(GetOppositeDirection(_directions[i]), new CellAttributes(_movableAttributes.row, _movableAttributes.column));
                _cellAttributes.Add(position);
                break;
            }
        }
        
        transform.rotation = Quaternion.Euler(RotationVectors[(Direction)_movableAttributes.directions[0]]);
        _controller.AssignBlock(this);
    }

    public void SetOrientation(Direction direction, CellAttributes coordinate)
    {
        if (!_orientation.TryAdd(direction, coordinate))
        {
            _orientation[direction] = coordinate;
        }
    }
    

    public Direction GetOppositeDirection(Direction given)
    {
        foreach (var direction in _directions)
        {
            if (given != direction) return direction;
        }

        return _directions[0];
    }

    public void TriggerMovement(Direction direction)
    {
        if (IsDesiredDirectionValid(direction))
        {
            _controller.MoveBlock(this, direction);
        }
        else
        {
            Debug.Log("Not movable in that direction");
        }
    }

    public void MoveBlock(CellAttributes target, Direction direction, Action onComplete)
    {
        Vector3 finalTarget = new Vector3(target.row, 0, target.column);
        if (GetLength() > 1)
        {
            finalTarget = UpdateFinalTargetByDirection(direction, finalTarget);
        }

        transform.DOMove(finalTarget, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    private Vector3 UpdateFinalTargetByDirection(Direction direction, Vector3 finalTarget)
    {
        switch (direction)
        {
            case Direction.Up:
                break;
            case Direction.Down:
                finalTarget += Vector3.left;
                break;
            case Direction.Right:
                finalTarget += Vector3.back;
                break;
            case Direction.Left:
                break;
        }

        return finalTarget;
    }

    public void AnimateBlock(bool isSuccessful)
    {
        
    }

    private bool IsDesiredDirectionValid(Direction desired)
    {
        return _directions.Contains(desired);
    }

    public BlockColors GetColor()
    {
        return _config.Color;
    }

    public int GetLength()
    {
        return _movableAttributes.length;
    }

    public CellAttributes GetCellAttributes(Direction direction)
    {
        if (_orientation.TryGetValue(direction, out CellAttributes coordinate)) return coordinate;
        return new CellAttributes((int)transform.position.x, (int)transform.position.z);
    }

    public List<CellAttributes> GetCellAttributes_2()
    {
        return _cellAttributes;
    }

    public void SetCellAttributes(List<CellAttributes> cellList)
    {
        _cellAttributes = cellList;
    }

    private void InjectLevelController(ControllerReadyEvent eventData)
    {
        _controller = eventData.Controller;
        ConfigureDirectionArray();
        ConfigureMesh(_gameConfig);
        ConfigureTransform();
    }

    private void AddListeners()
    {
        EventBus.Instance.Register<ControllerReadyEvent>(InjectLevelController);
    }

    private void RemoveListeners()
    {
        EventBus.Instance.Unregister<ControllerReadyEvent>(InjectLevelController);
    }
}
