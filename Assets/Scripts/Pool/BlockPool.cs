using System;
using System.Collections.Generic;
using System.Linq;
using GameObjects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pool
{
    public class BlockPool : ObjectPool<Block>
    {
        private readonly GameObject _smallBlockPrefab;
        private readonly GameObject _largeBlockPrefab;

        private Queue<Block>[] lengthBasedPools;

        public BlockPool(GameObject smallBlockPrefab, GameObject largeBlockPrefab, int initialSize,
            Transform parent = null)
            : base(null, initialSize, parent)
        {
            _smallBlockPrefab = smallBlockPrefab;
            _largeBlockPrefab = largeBlockPrefab;
            InitializeLengthBasedPools();
        }

        private void InitializeLengthBasedPools()
        {
            lengthBasedPools = new Queue<Block>[2];

            for (int i = 0; i < lengthBasedPools.Length; i++)
            {
                lengthBasedPools[i] = new Queue<Block>();
            }

            int initialSizePerLength = initialSize / 2;

            for (int i = 0; i < initialSizePerLength; i++)
            {
                CreateNewObject(1).ReturnToPool();
                CreateNewObject(2).ReturnToPool();
            }
        }


        protected override Block CreateNewObject()
        {
            return CreateNewObject(1);
        }

        protected override Block CreateNewObject(int length)
        {
            GameObject tempPrefab = length == 1 ? _smallBlockPrefab : _largeBlockPrefab;
            GameObject obj = Object.Instantiate(tempPrefab, parent);
            Block blockComponent = obj.GetComponent<Block>();
            
            int lengthIndex = length - 1;
            if (lengthIndex >= 0 && lengthIndex < lengthBasedPools.Length)
            {
                lengthBasedPools[lengthIndex].Enqueue(blockComponent);
            }
            else
            {
                Debug.LogWarning("Trying to add block to an invalid length-based pool.");
            }

            return blockComponent;
        }


        public override void ReturnObject(Block block)
        {
            block.ReturnToPool();

            int lengthIndex = block.GetLength() - 1;
            if (lengthIndex >= 0 && lengthIndex < lengthBasedPools.Length)
            {
                lengthBasedPools[lengthIndex].Enqueue(block);
            }
            else
            {
                Debug.LogWarning("Invalid block length returned to pool.");
            }
        }

        public Block GetObject(int length)
        {
            int lengthIndex = length - 1;

            if (lengthIndex >= 0 && lengthIndex < lengthBasedPools.Length && lengthBasedPools[lengthIndex].Count > 0)
            {
                Block obj = lengthBasedPools[lengthIndex].Dequeue();
                obj.EnableObject();
                return obj;
            }
            else
            {
                Block obj = CreateNewObject(length);
                obj.EnableObject();
                return obj;
            }
        }
    }
}