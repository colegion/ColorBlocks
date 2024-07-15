using System.Collections.Generic;
using GameObjects;
using Interfaces;
using UnityEngine;
using Utilities;

namespace Pool
{
    public class PoolController : MonoBehaviour
    {
        [SerializeField] private GameObject smallBlockPrefab;
        [SerializeField] private GameObject largeBlockPrefab;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GameObject exitPrefab;

        [SerializeField] private int poolSize;

        private BlockPool _blockPool;
        private CellPool _cellPool;
        private ExitPool _exitPool;
        
        private List<IPoolable> _activeObjects = new List<IPoolable>();

        private void Start()
        {
            var poolTransform = transform;
            _blockPool = new BlockPool(smallBlockPrefab, largeBlockPrefab, poolSize, poolTransform);
            _cellPool = new CellPool(cellPrefab, poolSize, poolTransform);
            _exitPool = new ExitPool(exitPrefab, poolSize, poolTransform);
            
            EventBus.Instance.Trigger(new CommonFields.PoolReadyEvent());
        }

        public void AppendActiveObjects(IPoolable poolable)
        {
            _activeObjects.Add(poolable);
        }

        public Block GetBlock(int length)
        {
            return _blockPool.GetObject(length);
        }

        private void ReturnBlock(Block block)
        {
            _blockPool.ReturnObject(block);
        }

        public Cell GetCell()
        {
            return _cellPool.GetObject();
        }

        private void ReturnCell(Cell cell)
        {
            _cellPool.ReturnObject(cell);
        }

        public Exit GetExit()
        {
            return _exitPool.GetObject();
        }

        private void ReturnExit(Exit exit)
        {
            _exitPool.ReturnObject(exit);
        }
        
        public void ClearActiveObjects()
        {
            foreach (var obj in _activeObjects)
            {
                if (obj is Cell cell)
                {
                    ReturnCell(cell);
                }
                else if (obj is Block block)
                {
                    ReturnBlock(block);
                }
                else if (obj is Exit exit)
                {
                    ReturnExit(exit);
                }
            }
            _activeObjects.Clear();
        }
    }
}
