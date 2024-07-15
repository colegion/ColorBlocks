using System;
using UnityEngine;
using Utilities;
using static Utilities.CommonFields;

namespace GameObjects
{
    public class PuzzleObject : MonoBehaviour
    {
        protected LevelController controller;

        private void Awake()
        {
            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        public virtual void AnimateObject(bool isSuccessful)
        {
            
        }
        
        protected virtual void InjectLevelController(ControllerReadyEvent eventData)
        {
            controller = eventData.Controller;
        }

        private void AddListeners()
        {
            EventBus.Instance.Register<ControllerReadyEvent>(InjectLevelController);
        }

        private void RemoveListeners()
        {
            if (EventBus.Instance != null)
            {
                EventBus.Instance.Unregister<ControllerReadyEvent>(InjectLevelController);
            }
        }
    }
}
