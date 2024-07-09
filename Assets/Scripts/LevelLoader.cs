using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class LevelLoader : MonoBehaviour
{
    private int _levelIndex = 1;
    void Start()
    {
        JsonReader.ReadJSon("Level1");
    }
}
