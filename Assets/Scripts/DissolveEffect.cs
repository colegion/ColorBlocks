using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utilities;

public class DissolveEffect
{
    private MeshRenderer _blockRenderer;
    private float _minCutoff;

    public DissolveEffect(MeshRenderer block, Color blockColor, float cutoff, float speed, MonoBehaviour gameObject, CommonFields.Direction direction)
    {
        _blockRenderer = block;
        _minCutoff = cutoff;
        _blockRenderer.material.SetColor("_EdgeColor", blockColor);
        SetDirection(direction);
        gameObject.StartCoroutine(DecreaseCutoff(speed));
    }

    private void SetDirection(CommonFields.Direction direction)
    {
        switch (direction)
        {
            case CommonFields.Direction.Up: case CommonFields.Direction.Left:
                _blockRenderer.material.SetFloat("_Direction", -1);
                break;
            case CommonFields.Direction.Down: case CommonFields.Direction.Right:
                _blockRenderer.material.SetFloat("_Direction", 1);
                break;
        }
    }

    private IEnumerator DecreaseCutoff(float speed)
    {
        while (_blockRenderer.material.GetFloat("_CutoffHeight") > _minCutoff)
        {
            var current = _blockRenderer.material.GetFloat("_CutoffHeight");
            current = Mathf.MoveTowards(current, _minCutoff, speed * Time.deltaTime);
            _blockRenderer.material.SetFloat("_CutoffHeight", current);

            yield return new WaitForEndOfFrame();
        }
    }
}
