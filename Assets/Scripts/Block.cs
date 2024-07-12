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

    private LevelController _controller;
    private BlockConfig _config;
    private MovableAttributes _movableAttributes;
    private Direction[] _directions;

    private void OnValidate()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    public void ConfigureSelf(BlockConfig config, MovableAttributes attributes)
    {
        _config = config;
        _movableAttributes = attributes;
        meshRenderer.material.mainTexture = config.TextureMap;
        _directions = new [] 
        {
            (Direction)_movableAttributes.directions[0],
            (Direction)_movableAttributes.directions[1]
        };
        transform.position = new Vector3(attributes.row, 0, attributes.column);
        transform.rotation = Quaternion.Euler(RotationVectors[(Direction)attributes.directions[0]]);
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
        Vector2 position;
            position = new Vector2(transform.position.x + (DirectionVectors[_directions[0]].x * (_movableAttributes.length -1)),
                transform.position.z + (DirectionVectors[_directions[0]].y * (_movableAttributes.length -1)));
        return new CellAttributes((int)position.x, (int)position.y);
    }

    private void InjectLevelController(ControllerReadyEvent eventData)
    {
        _controller = eventData.Controller;
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
