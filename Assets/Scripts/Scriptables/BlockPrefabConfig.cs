using GameObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scriptables
{
   [CreateAssetMenu(menuName = "Config/Prefab", fileName = "NewPrefabConfig")]
   public class BlockPrefabConfig : ScriptableObject
   {
      public int Length;
      public Block Prefab;
   }
}
