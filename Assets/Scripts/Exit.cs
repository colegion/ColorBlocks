using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;
using static Utilities.CommonFields;
using EventBus = Utilities.EventBus;

public class Exit : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] wallMeshes;
    [SerializeField] private ParticleSystem _exitParticle;
    private LevelController _controller;
    private ExitAttributes _config;


    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    public void ConfigureSelf(ExitAttributes config)
    {
        _config = config;
        var xPos = config.row + DirectionVectors[(Direction)config.direction].x;
        var zPos = config.column + DirectionVectors[(Direction)config.direction].y;
        transform.position = new Vector3(xPos, 0, zPos);
        transform.rotation = Quaternion.Euler(RotationVectors[(Direction)config.direction]);
        SetColor();
    }

    private void SetColor()
    {
        wallMeshes[0].material.color = ColorDictionary[(BlockColors)_config.color];
        wallMeshes[1].material.color = ColorDictionary[(BlockColors)_config.color];
    }

    public void PlayParticle(bool isSuccessful)
    {
        //var main = _exitParticle.main.startColor;
        //main.color = ColorDictionary[GetColor()];
    }

    public BlockColors GetColor()
    {
        return (BlockColors)_config.color;
    }

    public int GetDirection()
    {
        return _config.direction;
    }

    public CellAttributes GetCellAttributes()
    {
        return new CellAttributes(_config.row, _config.column);
    }
    
    public CellAttributes GetExitPosition()
    {
        return new CellAttributes((int)transform.position.x, (int)transform.position.z);
    }
    
    private void InjectLevelController(ControllerReadyEvent eventData)
    {
        _controller = eventData.Controller;
        _controller.SetCellExit(this);
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
