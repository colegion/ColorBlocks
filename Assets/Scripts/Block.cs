using System;
using System.Collections;
using System.Collections.Generic;
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
    
    public void MoveSelf()
    {
        throw new NotImplementedException();
    }

    public BlockColors GetColor()
    {
        return _config.Color;
    }

    public Direction[] GetDirections()
    {
        return _directions;
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
