using System.Collections.Generic;
using GameObjects;
using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(menuName = "Config/Global", fileName = "NewGameConfig")]
    public class GameConfig : ScriptableObject
    {
        public Mesh[] BlockMeshes;
        public BlockConfig[] BlockConfigs;
        public List<BlockPrefabConfig> BlockPrefabs;

        public Mesh GetMeshByLength(int length)
        {
            return length == 1 ? BlockMeshes[0] : BlockMeshes[1];
        }

        public BlockConfig GetConfigByColor(int colorIndex)
        {
            return BlockConfigs[colorIndex];
        }
        
        public Block GetPrefabByLength(int length)
        {
            var config = BlockPrefabs.Find(x => x.Length == length);
            return config.Prefab;
        }
    }
}
