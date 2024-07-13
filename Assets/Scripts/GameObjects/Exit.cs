using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
using static Utilities.CommonFields;
using EventBus = Utilities.EventBus;

namespace GameObjects
{
    public class Exit : PuzzleObject
    {
        [SerializeField] private List<MeshRenderer> wallMeshes;
        [SerializeField] private ParticleSystem exitParticle;
        private ExitAttributes _config;

        private void OnValidate()
        {
            wallMeshes = GetComponentsInChildren<MeshRenderer>().ToList();
        }

        public void ConfigureSelf(ExitAttributes config)
        {
            _config = config;
            var xPos = config.row + DirectionVectors[(Direction)config.direction].x;
            var zPos = config.column + DirectionVectors[(Direction)config.direction].y;
            transform.position = new Vector3(xPos, 0, zPos);
            transform.rotation = Quaternion.Euler(RotationVectors[(Direction)config.direction]);
            SetColor();
            gameObject.SetActive(true);
        }

        private void SetColor()
        {
            wallMeshes[0].material.color = ColorDictionary[(BlockColors)_config.color];
            wallMeshes[1].material.color = ColorDictionary[(BlockColors)_config.color];
            wallMeshes[0].enabled = true;
            wallMeshes[1].enabled = true;
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
            controller.SetCellExit(this);
        }
    }
}
