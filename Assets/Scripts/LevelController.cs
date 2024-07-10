using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private Cell[,] _cellGrid;
    private Block[,] _blockGrid;

    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    private void AddListeners()
    {
        
    }

    private void RemoveListeners()
    {
        
    }
}
