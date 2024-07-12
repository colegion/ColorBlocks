using UnityEngine;
using Utilities;
using static Utilities.CommonFields;

namespace Scriptables
{
    [CreateAssetMenu(menuName = "Config/Texture", fileName = "NewTextureConfig")]
    public class TextureConfig : ScriptableObject
    {
        public int Length;
        public Texture2D[] TextureMap;

        public Texture2D GetTextureByDirection(Direction direction)
        {
            return direction == Direction.Up || direction == Direction.Down
                ? TextureMap[0]
                : TextureMap[1];
        }
    }
}
