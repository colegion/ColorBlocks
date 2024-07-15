using Interfaces;
using UnityEngine;
using Utilities;
using static Utilities.CommonFields;
using EventBus = Utilities.EventBus;

namespace GameObjects
{
    public class Exit : PuzzleObject, IPoolable
    {
        [SerializeField] private MeshRenderer[] wallMeshes;
        [SerializeField] private ParticleSystem _exitParticle;
        private ExitAttributes _config;
        
        public void ConfigureSelf(ExitAttributes config)
        {
            _config = config;
            controller.SetCellExit(this);
            var xPos = config.row + DirectionVectors[(Direction)config.direction].x;
            var zPos = config.column + DirectionVectors[(Direction)config.direction].y;
            transform.position = new Vector3(xPos, 0, zPos);
            transform.rotation = Quaternion.Euler(RotationVectors[(Direction)config.direction]);
            SetColor(ColorDictionary[(BlockColors)_config.color]);
        }

        private void SetColor(Color color)
        {
            wallMeshes[0].material.color = color;
            wallMeshes[1].material.color = color;
        }
        
        public override void AnimateObject(bool isSuccessful)
        {
            
        }

        private void PlayParticle(bool isSuccessful)
        {
            //var main = _exitParticle.main.startColor;
            //main.color = ColorDictionary[GetColor()];
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
