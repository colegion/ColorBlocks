using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Utilities;
using static Utilities.CommonFields;

namespace GameObjects
{
    public class Cell : PuzzleObject, IPoolable
    {
        private Dictionary<Direction, Exit> _exits = new Dictionary<Direction, Exit>();
        public void InjectCellData(CellAttributes position)
        {
            transform.position = new Vector3(position.row, 0, position.column);
            controller.AssignCell(this);
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
            ReturnToPool();
        }

        public void EnableObject()
        {
            gameObject.SetActive(true);
        }

        public void ReturnToPool()
        {
            _exits.Clear();
            gameObject.SetActive(false);
        }
    }
}
