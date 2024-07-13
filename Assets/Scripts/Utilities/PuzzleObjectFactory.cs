using GameObjects;
using UnityEngine;
using static Utilities.CommonFields;

namespace Utilities
{
    public class PuzzleObjectFactory : MonoBehaviour
    {
        public static GameObject SetPuzzleObjectType(GameObject spawnedObject, PuzzleObjectType type)
        {
            spawnedObject.GetComponent<PuzzleObject>().enabled = false;
            switch (type)
            {
                case PuzzleObjectType.Cell:
                    spawnedObject.AddComponent<Cell>();
                    break;
                case PuzzleObjectType.Exit:
                    spawnedObject.AddComponent<Exit>();
                    break;
                case PuzzleObjectType.Block:
                    break;
            }

            return spawnedObject;
        }
    }
}
