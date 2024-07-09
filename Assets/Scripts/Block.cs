using System;
using System.Collections;
using System.Collections.Generic;
using Scriptables;
using UnityEngine;
using Utilities;
using static Utilities.CommonFields;

public class Block : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    private BlockConfig _config;

    private void OnValidate()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ConfigureSelf(BlockConfig config, MovableAttributes attributes)
    {
        _config = config;
        meshRenderer.material.mainTexture = config.TextureMap;
        transform.position = new Vector3(attributes.row, 0, attributes.column);
        transform.rotation = Quaternion.Euler(RotationVectors[(Direction)attributes.directions[0]]);
    }

    public BlockColors GetColor()
    {
        return _config.Color;
    }
}
