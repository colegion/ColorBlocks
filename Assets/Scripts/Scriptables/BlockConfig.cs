using TreeEditor;
using UnityEngine;
using static Utilities.CommonFields;

namespace Scriptables
{
    [CreateAssetMenu(menuName = "Block/Config", fileName = "NewConfig")]
    public class BlockConfig : ScriptableObject
    {
        public Texture2D TextureMap;
        public BlockColors Color;
    }
}
