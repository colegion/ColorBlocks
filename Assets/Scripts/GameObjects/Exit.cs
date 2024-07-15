using DG.Tweening;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;
using static Utilities.CommonFields;
using EventBus = Utilities.EventBus;

namespace GameObjects
{
    public class Exit : PuzzleObject, IPoolable
    {
        [SerializeField] private MeshRenderer[] wallMeshes;
        [SerializeField] private ParticleSystem exitParticle;
        private ExitAttributes _config;
        
        public void ConfigureSelf(ExitAttributes config)
        {
            _config = config;
            controller.SetCellExit(this);
            var xPos = config.row + DirectionVectors[(Direction)config.direction].x;
            var zPos = config.column + DirectionVectors[(Direction)config.direction].y;
            transform.position = new Vector3(xPos, 0, zPos);
            transform.rotation = Quaternion.Euler(RotationVectors[(Direction)config.direction]);
            if ((Direction)config.direction == Direction.Up)
            {
                exitParticle.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0));
            }
            SetColor(ColorDictionary[(BlockColors)_config.color]);
        }

        private void SetColor(Color color)
        {
            wallMeshes[0].material.color = color;
            wallMeshes[1].material.color = color;
        }
        
        public override void AnimateObject(bool isSuccessful)
        {
           AnimateDoors(isSuccessful);
        }

        private void AnimateDoors(bool isSuccess)
        {
            Sequence sequence = DOTween.Sequence();
            if (isSuccess)
            {
                sequence.Append(wallMeshes[0].transform.DOScaleY(0.1f, 0.4f).SetEase(Ease.OutCirc));
                sequence.Join(wallMeshes[1].transform.DOScaleY(0.1f, 0.4f).SetEase(Ease.OutCirc));

                sequence.AppendCallback(() =>
                {
                    exitParticle.GetComponent<Renderer>().material.color = ColorDictionary[GetColor()];


                    var main = exitParticle.main;
                    main.startColor = new ParticleSystem.MinMaxGradient(ColorDictionary[GetColor()]);
                    exitParticle.Play();
                });

                sequence.AppendInterval(0.4f);
                sequence.Append(wallMeshes[0].transform.DOScaleY(1, 0.4f).SetEase(Ease.OutCirc));
                sequence.Join(wallMeshes[1].transform.DOScaleY(1, 0.4f).SetEase(Ease.OutCirc));
                sequence.Play();
            }
            else
            {
                sequence.Append(wallMeshes[0].transform.DOPunchRotation(new Vector3(0.5f, 0, 0), 0.5f)
                    .SetEase(Ease.Linear));
                sequence.Join(wallMeshes[1].transform.DOPunchRotation(new Vector3(0.5f, 0, 0), 0.5f)
                    .SetEase(Ease.Linear));
            }
        }

        public BlockColors GetColor()
        {
            return (BlockColors)_config.color;
        }

        public int GetDirection()
        {
            return _config.direction;
        }

        public CellAttributes GetCellAttributes()
        {
            return new CellAttributes(_config.row, _config.column);
        }
    
        public CellAttributes GetExitPosition()
        {
            return new CellAttributes((int)transform.position.x, (int)transform.position.z);
        }
    
        protected override void InjectLevelController(ControllerReadyEvent eventData)
        {
            base.InjectLevelController(eventData);
            ReturnToPool();
        }

        public void EnableObject()
        {
            gameObject.SetActive(true);
        }

        public void ReturnToPool()
        {
            _config = null;
            SetColor(Color.white);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            gameObject.SetActive(false);
        }
    }
}
