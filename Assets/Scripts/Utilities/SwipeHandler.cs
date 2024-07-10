using System;
using UnityEngine;
using static Utilities.CommonFields;

namespace Utilities
{
    public class SwipeHandler : MonoBehaviour
    {
        private Vector2 startTouchPosition;
        private Vector2 endTouchPosition;
        private bool isSwiping = false;

        [SerializeField] private float swipeThreshold = 50f;
        [SerializeField] private LayerMask blockLayer;

        private Block selectedBlock;
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
                        Block block = hit.collider.GetComponent<Block>();
                        if (block != null)
                        {
                            selectedBlock = block;
                            startTouchPosition = touch.position;
                            isSwiping = true;
                        }
                    }
                }

                if (touch.phase == TouchPhase.Moved && isSwiping)
                {
                    endTouchPosition = touch.position;
                    Vector2 swipeDirection = endTouchPosition - startTouchPosition;

                    if (swipeDirection.magnitude >= swipeThreshold)
                    {
                        DetectSwipeDirection(swipeDirection);
                        isSwiping = false;
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    isSwiping = false;
                }
            }
        }

        void DetectSwipeDirection(Vector2 swipeDirection)
        {
            swipeDirection.Normalize();

            if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
            {
                if (swipeDirection.x > 0)
                {
                    _detectedDirection = Direction.Right;
                }
                else
                {
                    _detectedDirection = Direction.Left;
                }
            }
            else
            {
                if (swipeDirection.y > 0)
                {
                    _detectedDirection = Direction.Up;
                }
                else
                {
                    _detectedDirection = Direction.Down;
                }
            }
            
            Debug.Log("Direction: " + _detectedDirection);
        }
    }
}
