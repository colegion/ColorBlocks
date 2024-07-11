using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Cell : MonoBehaviour
{
    public void ConfigureSelf(CellAttributes position)
    {
        transform.position = new Vector3(position.row, 0, position.column);
    }
}
