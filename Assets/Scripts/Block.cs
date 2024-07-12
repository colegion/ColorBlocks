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

    private List<CellAttributes> _cellAttributes = new List<CellAttributes>(); 

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
        /*ConfigureDirectionArray();
        ConfigureMesh(gameData);
        ConfigureTransform();*/
    }

    private void ConfigureDirectionArray()
    {
        _directions = new [] 
        {
            (Direction)_movableAttributes.directions[0],
            (Direction)_movableAttributes.directions[1]
        };
    }
    
    private void ConfigureMesh(GameConfig gameData)
    {
        meshRenderer.material.mainTexture = _config.GetTextureByLength(_movableAttributes.length, _directions[0]);
        meshFilter.mesh = gameData.GetMeshByLength(_movableAttributes.length);
    }

    private void ConfigureTransform()
    {
        
        CellAttributes position = new CellAttributes(_movableAttributes.row, _movableAttributes.column);
        transform.position = new Vector3(_movableAttributes.row, 0, _movableAttributes.column);
        _cellAttributes.Add(position);
        for (int i = 0; i < _directions.Length; i++)
        {
            position = new CellAttributes(_movableAttributes.row + DirectionVectors[_directions[i]].x * (_movableAttributes.length -1),
                _movableAttributes.column + DirectionVectors[_directions[i]].y * (_movableAttributes.length -1));
            if (!_controller.IsCellBlocked(position))
            {
                _cellAttributes.Add(position);
                break;
            }
        }
        
        transform.rotation = Quaternion.Euler(RotationVectors[(Direction)_movableAttributes.directions[0]]);
        _controller.AssignBlock(this);
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

    public void MoveBlock(CellAttributes target, Action onComplete)
    {
        transform.DOMove(new Vector3(target.row, 0, target.column), 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
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

    public Direction[] GetDirections()
    {
        return _directions;
    }

    public CellAttributes GetCellAttributes()
    {
        return new CellAttributes((int)transform.position.x, (int)transform.position.z);
    }

    public List<CellAttributes> GetCellAttributes_2()
    {
        return _cellAttributes;
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
