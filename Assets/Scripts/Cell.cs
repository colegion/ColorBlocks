using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public void ConfigureSelf(Vector2 position)
    {
        transform.position = new Vector3(position.x, 0, position.y);
    }
}
