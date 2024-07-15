using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using GameObjects;
using Interfaces;
using Pool;
using Scriptables;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;
using Utilities;
using static Utilities.CommonFields;

public class LevelLoader : MonoBehaviour
{
    private LevelController _levelController;
    private LevelInfo _currentLevel;
    private int _levelIndex = 4;

    [SerializeField] private PoolController poolController;
    [SerializeField] private GameConfig gameConfig;
    
    private void Awake()
    {
        AddListeners();
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    private void InitiateGame(PoolReadyEvent eventData)
    {
        LoadLevel();
    }

    private void LoadLevel()
    {
        _currentLevel = JsonReader.ReadJSon($"Level{_levelIndex}");
        _levelController = new LevelController(_currentLevel.rowCount, _currentLevel.columnCount);
        _levelController.SetMoveLimit(_currentLevel.moveLimit);
        
        EventBus.Instance.Trigger(new ControllerReadyEvent(_levelController));
        ConfigureCells();
        ConfigureBlocks();
        ConfigureExits();
        EventBus.Instance.Trigger(new LevelIndexEvent(_levelIndex));
    }

    private void ConfigureCells()
    {
        var cells = _currentLevel.cellInfo;
        foreach (var element in cells)
        {
            var tempCell = poolController.GetCell();
            poolController.AppendActiveObjects(tempCell);
            tempCell.InjectCellData(new CellAttributes(element.row, element.column));
        }
    }

    private void ConfigureBlocks()
    {
        var movables = _currentLevel.movableInfo;
        foreach (var element in movables)
        {
            var tempBlock = poolController.GetBlock(element.length);
            poolController.AppendActiveObjects(tempBlock);
            tempBlock.InjectBlockData(element, gameConfig);
        }
    }

    private void ConfigureExits()
    {
        var exits = _currentLevel.exitInfo;
        foreach (var element in exits)
        {
            var tempExit = poolController.GetExit();
            poolController.AppendActiveObjects(tempExit);
            tempExit.ConfigureSelf(element);
        }
    }

    private void HandleOnLevelComplete(LevelFinishedEvent eventData)
    {
        if (eventData.IsSuccessful)
        {
            _levelIndex = _levelIndex % 4 + 1;
        }

        DOVirtual.DelayedCall(1.5f, () =>
        {
            poolController.ClearActiveObjects();
            LoadLevel();
        });

    }

    private void AddListeners()
    {
        EventBus.Instance.Register<LevelFinishedEvent>(HandleOnLevelComplete);
        EventBus.Instance.Register<PoolReadyEvent>(InitiateGame);
    }
 
    private void RemoveListeners()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Unregister<LevelFinishedEvent>(HandleOnLevelComplete);
            EventBus.Instance.Unregister<PoolReadyEvent>(InitiateGame);
        }
    }
}
