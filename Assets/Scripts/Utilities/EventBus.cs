using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Utilities
{
    public class EventBus : MonoBehaviour
    {
        private static EventBus _instance;
        private static bool _isInitialized;

        public static EventBus Instance
        {
            get
            {
                if (_instance == null && !_isInitialized)
                {
                    var eventBusGameObject = new GameObject("EventBus");
                    _instance = eventBusGameObject.AddComponent<EventBus>();
                    DontDestroyOnLoad(eventBusGameObject);
                    _isInitialized = true;
                }
                return _instance;
            }
        }

        private readonly Dictionary<Type, List<Delegate>> _eventListeners = new Dictionary<Type, List<Delegate>>();

        public void Register<T>(Action<T> listener) where T : class
        {
            Type eventType = typeof(T);
            if (!_eventListeners.ContainsKey(eventType))
            {
                _eventListeners[eventType] = new List<Delegate>();
            }
            _eventListeners[eventType].Add(listener);
        }

        public void Unregister<T>(Action<T> listener) where T : class
        {
            Type eventType = typeof(T);
            if (_eventListeners.TryGetValue(eventType, out var eventListeners))
            {
                eventListeners.Remove(listener);
            }
        }

        public void Trigger<T>(T eventData) where T : class
        {
            Type eventType = typeof(T);
            if (_eventListeners.TryGetValue(eventType, out var eventListeners))
            {
                foreach (var listener in new List<Delegate>(eventListeners))
                {
                    ((Action<T>)listener)(eventData);
                }
            }
        }
        
        private void OnDestroy()
        {
            _eventListeners.Clear();
            _instance = null;
        }
    }
}