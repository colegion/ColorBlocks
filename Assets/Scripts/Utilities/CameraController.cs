using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Utilities
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera gameCamera;
        private LevelController _controller;
        private void Awake()
        {
            AddListeners();
        }

        private void OnDisable()
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
            
            float middleX = width / 2f;
            float middleZ = height / 4f;
            
            float fov = gameCamera.fieldOfView * Mathf.Deg2Rad;
            
            float aspectRatio = (float)Screen.width / Screen.height;
            float gridAspect = (float)width / height;

            float distance;
            if (gridAspect > aspectRatio)
            {
                distance = width / Mathf.Tan(fov / 4f);
            }
            else
            {
                distance = height / Mathf.Tan(fov / 4f);
            }
            
            gameCamera.transform.position = new Vector3(middleX, distance, middleZ);
            gameCamera.transform.LookAt(new Vector3(middleX, 0, middleZ));
            gameCamera.transform.rotation = Quaternion.Euler(80, -90, 0);
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
