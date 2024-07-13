using System.Collections.Generic;
using UnityEngine;
using Utilities;
using static Utilities.CommonFields;

namespace GameObjects
{
    public class Cell : PuzzleObject
    {
        [SerializeField] private MeshRenderer meshRenderer;
        private Dictionary<Direction, Exit> _exits = new Dictionary<Direction, Exit>();
        public void InjectCellData(CellAttributes position)
        {
            gameObject.SetActive(true);
            transform.position = new Vector3(position.row, 0, position.column);
            transform.localScale = new Vector3(1, 0.1f, 1);
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.enabled = true;
        }

        public void SetExitByDirection(Exit exit)
        {
            _exits[(Direction)exit.GetDirection()] = exit;
        }

        public CellAttributes GetCellAttribute()
        {
            return new CellAttributes((int)transform.position.x, (int)transform.position.z);
        }

        public Exit GetExitByDirection(Direction direction)
        {
            return _exits.TryGetValue(direction, out var exit) ? exit : null;
        }
    
        protected override void InjectLevelController(ControllerReadyEvent eventData)
        {
            base.InjectLevelController(eventData);
            controller.AssignCell(this);
        }
    }
}
