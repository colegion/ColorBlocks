using System;
using System.Collections.Generic;
using GameObjects;
using UnityEngine;

namespace Utilities
{
    public class PuzzleObjectPool : MonoBehaviour
    {
        [SerializeField] private Transform poolParent;
        [SerializeField] private PuzzleObject objectToPool;
        [SerializeField] private int poolAmount;
        
        private static PuzzleObjectPool _instance;
        public static PuzzleObjectPool Instance
        {
            get
            {
                if (_instance == null)
                {
                    var eventBusGameObject = new GameObject("PuzzlePool");
                    _instance = eventBusGameObject.AddComponent<PuzzleObjectPool>();
                    DontDestroyOnLoad(eventBusGameObject);
                }
                return _instance;
            }
        }

        private List<PuzzleObject> _pooledPuzzleObjects = new List<PuzzleObject>();

        private void Awake()
        {
            PoolPuzzleObjects();
        }

        private void PoolPuzzleObjects()
        {
            for (int i = 0; i < poolAmount; i++)
            {
                var temp = Instantiate(objectToPool, poolParent);
                _pooledPuzzleObjects.Add(temp);
            }
        }

        public PuzzleObject GetAvailableObject()
        {
            foreach (var element in _pooledPuzzleObjects)
            {
                if (!element.gameObject.activeSelf)
                    return element;
            }
            
            PoolPuzzleObjects();
            return GetAvailableObject();
        }
    }
}
