using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using static Utilities.CommonFields;

public class Gate : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] wallMeshes;

    private ExitAttributes _config;
    
    public void ConfigureSelf(ExitAttributes config)
    {
        _config = config;
        var xPos = config.column + DirectionVectors[(Direction)config.direction].x;
        var zPos = -config.row + DirectionVectors[(Direction)config.direction].y;
        transform.position = new Vector3(xPos, 0, zPos);
        transform.rotation = Quaternion.Euler(RotationVectors[(Direction)config.direction]);
        SetColor();
    }

    private void SetColor()
    {
        wallMeshes[0].material.color = ColorDictionary[(BlockColors)_config.color];
        wallMeshes[1].material.color = ColorDictionary[(BlockColors)_config.color];
    }
}
