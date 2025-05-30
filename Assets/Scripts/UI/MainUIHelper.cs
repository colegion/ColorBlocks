using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;
using static Utilities.CommonFields;
using EventBus = Utilities.EventBus;

namespace UI
{
    public class MainUIHelper : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moveField;
        [SerializeField] private TextMeshProUGUI moveCountField;
        [SerializeField] private TextMeshProUGUI levelIndexField;
        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void SetLevelField(LevelIndexEvent eventData)
        {
            levelIndexField.text = $"{eventData.LevelIndex}";
        }

        private void UpdateMoveCountField(MovementEvent eventData)
        {
            if (eventData.RemainingMoveCount == 0)
            {
                moveField.gameObject.SetActive(false);
            }
            else
            {
                moveField.gameObject.SetActive(true);
            }
            
            moveCountField.text = $"{eventData.RemainingMoveCount}";
        }

        private void AddListeners()
        {
            EventBus.Instance.Register<MovementEvent>(UpdateMoveCountField);
            EventBus.Instance.Register<LevelIndexEvent>(SetLevelField);
        }

        private void RemoveListeners()
        {
            if (EventBus.Instance != null)
            {
                EventBus.Instance.Unregister<MovementEvent>(UpdateMoveCountField);
                EventBus.Instance.Unregister<LevelIndexEvent>(SetLevelField);
            }
        }
    }
}
