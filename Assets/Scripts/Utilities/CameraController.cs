using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Utilities
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private int borderGap;
        private LevelController _controller;
        private void Awake()
        {
            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void InjectLevelController(CommonFields.ControllerReadyEvent eventData)
        {
            _controller = eventData.Controller;
            SetupCamera();
        }

        private void SetupCamera()
        {
            var width = _controller.GetWidth();
            var height = _controller.GetHeight();
            camera.transform.position = new Vector3(width / 2f, 2, height / 4f);

            var aspectRatio = (float)Screen.width / Screen.height;

            var horizontalSize = (width / 2f + borderGap) / aspectRatio;
            var verticalSize = height / 2f + borderGap;
            camera.orthographicSize = verticalSize > horizontalSize ? verticalSize : horizontalSize;
        }

        private void AddListeners()
        {
            EventBus.Instance.Register<CommonFields.ControllerReadyEvent>(InjectLevelController);
        }

        private void RemoveListeners()
        {
            EventBus.Instance.Unregister<CommonFields.ControllerReadyEvent>(InjectLevelController);
        }
    }
}
