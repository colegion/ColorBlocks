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
                    Debug.LogError("PuzzleObjectPool is not initialized!");
                }
                return _instance;
            }
        }

        private List<PuzzleObject> _pooledPuzzleObjects = new List<PuzzleObject>();

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            PoolPuzzleObjects();
        }

        private void PoolPuzzleObjects()
        {
            for (int i = 0; i < poolAmount; i++)
            {
                var temp = Instantiate(objectToPool, poolParent);
                temp.gameObject.SetActive(false);
                _pooledPuzzleObjects.Add(temp);
            }

            EventBus.Instance.Trigger(new CommonFields.PoolReadyEvent());
        }

        public PuzzleObject GetAvailableObject()
        {
            foreach (var element in _pooledPuzzleObjects)
            {
                if (!element.gameObject.activeSelf)
                    return element;
            }

            return null;
        }
    }
}