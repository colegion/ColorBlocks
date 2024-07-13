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
            moveCountField.text = $"{eventData.RemainingMoveCount}";
        }

        private void AddListeners()
        {
            EventBus.Instance.Register<MovementEvent>(UpdateMoveCountField);
            EventBus.Instance.Register<LevelIndexEvent>(SetLevelField);
        }

        private void RemoveListeners()
        {
            EventBus.Instance.Unregister<MovementEvent>(UpdateMoveCountField);
            EventBus.Instance.Unregister<LevelIndexEvent>(SetLevelField);
        }
    }
}
