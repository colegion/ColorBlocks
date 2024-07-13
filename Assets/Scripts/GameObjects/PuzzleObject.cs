using System;
using UnityEngine;
using Utilities;
using static Utilities.CommonFields;

namespace GameObjects
{
    public class PuzzleObject : MonoBehaviour
    {
        protected LevelController controller;

        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
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

        public virtual void ReturnToPool()
        {
            
        }

        private void AddListeners()
        {
            EventBus.Instance.Register<ControllerReadyEvent>(InjectLevelController);
        }

        private void RemoveListeners()
        {
            EventBus.Instance.Register<ControllerReadyEvent>(InjectLevelController);
        }
    }
}
