using System.Collections.Generic;
using Scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public static class CommonFields
    {
        public static Dictionary<BlockColors, Color> ColorDictionary = new Dictionary<BlockColors, Color>()
        {
            { BlockColors.Blue, Color.blue },
            { BlockColors.Green, Color.green },
            { BlockColors.Purple, Color.magenta },
            { BlockColors.Red, Color.red },
            { BlockColors.Yellow, new Color(255, 165, 0, 255) },
        };

        public static Dictionary<Direction, Vector2Int> DirectionVectors = new Dictionary<Direction, Vector2Int>()
        {
            { Direction.Up, new Vector2Int(-1, 0) },
            { Direction.Right, new Vector2Int(0, 1) },
            { Direction.Down, new Vector2Int(1, 0) },
            { Direction.Left, new Vector2Int(0, -1) },
        };
        
        public static Dictionary<Direction, Vector3> RotationVectors = new Dictionary<Direction, Vector3>()
        {
            { Direction.Up, new Vector3(0, 0, 0) },
            { Direction.Right, new Vector3(0, -90, 0) },
            { Direction.Down, new Vector3(0, 0, 0) },
            { Direction.Left, new Vector3(0, 90, 0) },
        };

        public enum BlockColors
        {
            Blue = 0,
            Green,
            Purple,
            Red,
            Yellow
        }

        public enum Direction
        {
            Up = 0,
            Right,
            Down,
            Left
        }

        public class PoolReadyEvent
        {
            
        }

        public class ControllerReadyEvent
        {
            public LevelController Controller;

            public ControllerReadyEvent(LevelController controller)
            {
                Controller = controller;
            }
        }

        public class LevelIndexEvent
        {
            public int LevelIndex;

            public LevelIndexEvent(int index)
            {
                LevelIndex = index;
            }
        }

        public class MovementEvent
        {
            public int RemainingMoveCount;

            public MovementEvent(int count)
            {
                RemainingMoveCount = count;
            }
        }

        public class LevelFinishedEvent
        {
            public bool IsSuccessful;
            
            public LevelFinishedEvent(bool isSuccess)
            {
                IsSuccessful = isSuccess;
            }
        }
    }
}
