using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pool
{
    public abstract class ObjectPool<T> where T : IPoolable
    {
        protected Queue<T> pool = new Queue<T>();
        protected GameObject prefab;
        protected Transform parent;
        protected int initialSize;

        public ObjectPool(GameObject prefab, int initialSize, Transform parent = null)
        {
            this.prefab = prefab;
            this.parent = parent;
            this.initialSize = initialSize;
        }

        protected void Initialize()
        {
            InitializePool(initialSize);
        }

        protected virtual void InitializePool(int initialSize)
        {
            for (int i = 0; i < initialSize; i++)
            {
                T obj = CreateNewObject();
                obj.ReturnToPool();
                pool.Enqueue(obj);
            }
        }

        protected virtual T CreateNewObject()
        {
            GameObject obj = Object.Instantiate(prefab, parent);
            return obj.GetComponent<T>();
        }
        
        protected virtual T CreateNewObject(int length)
        {
            throw new NotImplementedException("CreateNewObject(int length) not implemented in base class.");
        }

        public T GetObject()
        {
            if (pool.Count > 0)
            {
                T obj = pool.Dequeue();
                obj.EnableObject();
                return obj;
            }
            else
            {
                T obj = CreateNewObject();
                obj.EnableObject();
                return obj;
            }
        }

        public virtual void ReturnObject(T obj)
        {
            obj.ReturnToPool();
            pool.Enqueue(obj);
        }
    }

}
