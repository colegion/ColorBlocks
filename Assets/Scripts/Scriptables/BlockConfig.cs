using System;
using System.Collections.Generic;
using GameObjects;
using TreeEditor;
using UnityEngine;
using static Utilities.CommonFields;

namespace Scriptables
{
    [CreateAssetMenu(menuName = "Config/Block", fileName = "NewBlockConfig")]
    public class BlockConfig : ScriptableObject
    {
        public BlockColors Color;
        public List<TextureConfig> BlockTextures;
        
        public Texture2D GetTextureByLength(int length, Direction direction)
        {
            foreach (var element in BlockTextures)
            {
                if (element.Length == length)
                    return element.GetTextureByDirection(direction);
            }

            return BlockTextures[0].GetTextureByDirection(direction);
        }
    }
}
