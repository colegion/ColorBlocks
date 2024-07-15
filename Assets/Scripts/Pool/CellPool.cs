using GameObjects;
using UnityEngine;

namespace Pool
{
    public class CellPool : ObjectPool<Cell>
    {
        private bool _hasInitialized;
        public CellPool(GameObject prefab, int initialSize, Transform parent = null) : base(prefab, initialSize, parent)
        {
            Initialize();
        }

    }
}
