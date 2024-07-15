using GameObjects;
using UnityEngine;

namespace Pool
{
    public class ExitPool : ObjectPool<Exit>
    {
        private bool _hasInitialized;
        public ExitPool(GameObject prefab, int initialSize, Transform parent = null) : base(prefab, initialSize, parent)
        {
            Initialize();
        }
    }
}
