using UnityEngine;

namespace Galaxy
{
    public static partial class Constant
    {
        /// <summary>
        /// 层。
        /// </summary>
        public static class Layer
        {
            public const string DefaultLayerName = "Default";
            public static readonly int DefaultLayerId = LayerMask.NameToLayer(DefaultLayerName);

            public const string UILayerName = "UI";
            public static readonly int UILayerId = LayerMask.NameToLayer(UILayerName);

            public const string AvatarLayerName = "Avatar";
            public static readonly int AvatarLayerId = LayerMask.NameToLayer(AvatarLayerName);
        }
    }
}
