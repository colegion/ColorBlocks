using System;
using Interfaces;
using UnityEngine;
using static Utilities.CommonFields;

namespace Utilities
{
    public class SwipeHandler : MonoBehaviour
    {
        private Vector2 _startTouchPosition;
        private Vector2 _endTouchPosition;
        private bool _isSwiping = false;

        [SerializeField] private float swipeThreshold = 50f;
        [SerializeField] private LayerMask blockLayer;

        private IMovable _movable;
        private Direction _detectedDirection;
        void Update()
        {
            HandleSwipe();
        }

        void HandleSwipe()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, blockLayer))
                    {
                        IMovable movable = hit.collider.GetComponent<IMovable>();
                        if (movable != null)
                        {
                            _movable = movable;
                            _startTouchPosition = touch.position;
                            _isSwiping = true;
                        }
                    }
                }

                if (touch.phase == TouchPhase.Moved && _isSwiping)
                {
                    _endTouchPosition = touch.position;
                    Vector2 swipeDirection = _endTouchPosition - _startTouchPosition;

                    if (swipeDirection.magnitude >= swipeThreshold)
                    {
                        DetectSwipeDirection(swipeDirection);
                        _isSwiping = false;
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    _isSwiping = false;
                }
            }
        }

        void DetectSwipeDirection(Vector2 swipeDirection)
        {
            swipeDirection.Normalize();

            if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
            {
                _detectedDirection = swipeDirection.x > 0 ? Direction.Right : Direction.Left;
            }
            else
            {
                _detectedDirection = swipeDirection.y > 0 ? Direction.Up : Direction.Down;
            }
            
            Debug.Log("Direction: " + _detectedDirection);
            _movable.MoveSelf();
        }
    }
}
