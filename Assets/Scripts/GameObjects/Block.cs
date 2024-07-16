using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Interfaces;
using Scriptables;
using UnityEngine;
using Utilities;
using static Utilities.CommonFields;

namespace GameObjects
{
    public class Block : PuzzleObject, IMovable, IPoolable
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private float cutOff;
        
        private DissolveEffect _dissolveEffect;
        
        
        private BlockConfig _config;
        private MovableAttributes _movableAttributes;
        private Direction[] _directions;
        private Direction _mainDirection;

        private List<CellAttributes> _cellAttributes = new List<CellAttributes>();
        private Dictionary<Direction, CellAttributes> _orientation;

        private void OnValidate()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void InjectBlockData(MovableAttributes attributes, GameConfig gameData)
        {
            _config = gameData.GetConfigByColor(attributes.color);
            _movableAttributes = attributes;
            ConfigureDirectionArray();
            ConfigureMesh();
            ConfigureTransform();
        }

        private void ConfigureDirectionArray()
        {
            _directions = new [] 
            {
                (Direction)_movableAttributes.directions[0],
                (Direction)_movableAttributes.directions[1]
            };

            _mainDirection = _directions[0];
        }
    
        private void ConfigureMesh()
        {
            meshRenderer.material.SetTexture("_MainTex", _config.GetTextureByLength(_movableAttributes.length, _directions[0]));
            var blockColor = ColorDictionary[_config.Color];
            blockColor.a = 130f / 255f;
            trailRenderer.startColor = blockColor;
            trailRenderer.endColor = blockColor;
        }
      
        private void ConfigureTransform()
        {
            _cellAttributes.Clear();
            _orientation = new Dictionary<Direction, CellAttributes>();
            CellAttributes position = new CellAttributes(_movableAttributes.row, _movableAttributes.column);
            transform.position = new Vector3(_movableAttributes.row, 0, _movableAttributes.column);
            _cellAttributes.Add(position);
            for (int i = _directions.Length-1 ; i >= 0; i--)
            {
                position = new CellAttributes(_movableAttributes.row + DirectionVectors[_directions[i]].x * (_movableAttributes.length -1),
                    _movableAttributes.column + DirectionVectors[_directions[i]].y * (_movableAttributes.length -1));
                if (!controller.IsCellBlocked(position))
                {
                    SetOrientation(_directions[i], position);
                    SetOrientation(GetOppositeDirection(_directions[i]), new CellAttributes(_movableAttributes.row, _movableAttributes.column));
                    _cellAttributes.Add(position);
                    break;
                }
            }
        
            transform.rotation = Quaternion.Euler(RotationVectors[(Direction)_movableAttributes.directions[0]]);
            controller.AssignBlock(this);
        }

        public void SetOrientation(Direction direction, CellAttributes coordinate)
        {
            if (!_orientation.TryAdd(direction, coordinate))
            {
                _orientation[direction] = coordinate;
            }
        }
        
        public Direction GetOppositeDirection(Direction given)
        {
            foreach (var direction in _directions)
            {
                if (given != direction) return direction;
            }

            return _directions[0];
        }

        public void TriggerMovement(Direction direction)
        {
            if (IsDirectionValid(direction))
            {
                controller.MoveBlock(this, direction);
            }
            else
            {
                AnimateStuck(direction);
                Debug.Log("Not movable in that direction");
            }
        }

        private void AnimateStuck(Direction direction)
        {
            var punch = direction is Direction.Up or Direction.Down ? new Vector3(0.1f, 0, 0) : new Vector3(0f, 0, 0.1f); 
            transform.DOPunchPosition(punch, 0.3f);
        }

        public void MoveBlock(CellAttributes target, Direction direction, Action onComplete)
        {
            var finalTarget = new Vector3(target.row, 0, target.column);
            if (GetLength() > 1)
            {
                finalTarget = UpdateFinalTargetByDirection(direction, finalTarget);
            }

            if (finalTarget == transform.position)
            {
                AnimateStuck(direction);
            }
            else
            {
                transform.DOMove(finalTarget, 9f).SetEase(Ease.Linear).SetSpeedBased().OnComplete(() =>
                {
                    onComplete?.Invoke();
                });
            }
        }

        private Vector3 UpdateFinalTargetByDirection(Direction direction, Vector3 finalTarget)
        {
            switch (direction)
            {
                case Direction.Up:
                    break;
                case Direction.Down:
                    finalTarget += Vector3.left;
                    break;
                case Direction.Right:
                    finalTarget += Vector3.back;
                    break;
                case Direction.Left:
                    break;
            }

            return finalTarget;
        }

        public override void AnimateObject(bool isSuccessful, Direction direction)
        {
            if (isSuccessful)
            {
                var position = transform.position;
                var vector = DirectionVectors[direction];
                var target = new Vector3(position.x + (vector.x * GetLength()), 0, position.z + (vector.y * GetLength()));
                _dissolveEffect = new DissolveEffect(meshRenderer, ColorDictionary[GetColor()], cutOff, 5f, this, direction);
                transform.DOMove(target, 1f).SetEase(Ease.Flash);
                transform.DOShakeScale(1f, 0.05f);
                trailRenderer.gameObject.SetActive(false);
                DOVirtual.DelayedCall(2.5f, ReturnToPool);
            }
        }

        private bool IsDirectionValid(Direction desired)
        {
            return _directions.Contains(desired);
        }

        public BlockColors GetColor()
        {
            return _config.Color;
        }

        public int GetLength()
        {
            return _movableAttributes.length;
        }

        public CellAttributes GetCellAttribute(Direction direction)
        {
            if (_orientation.TryGetValue(direction, out CellAttributes coordinate)) return coordinate;
            return new CellAttributes((int)transform.position.x, (int)transform.position.z);
        }

        public List<CellAttributes> GetCellAttributesList()
        {
            return _cellAttributes;
        }

        public void SetCellAttributes(List<CellAttributes> cellList)
        {
            _cellAttributes = cellList;
        }

        protected override void InjectLevelController(ControllerReadyEvent eventData)
        {
            base.InjectLevelController(eventData);
            ReturnToPool();
        }

        public void EnableObject()
        {
            gameObject.SetActive(true);
            trailRenderer.gameObject.SetActive(true);
        }

        public void ReturnToPool()
        {
            _dissolveEffect = null;
            meshRenderer.material.SetFloat("_CutoffHeight", 1.2f);
            gameObject.SetActive(false);
        }
    }
}
