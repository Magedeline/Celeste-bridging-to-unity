using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework
{
    public static class ColorExtensions
    {
        public static Vector3 ToVector3(this Color color)
        {
            return new Vector3(color.R / 255f, color.G / 255f, color.B / 255f);
        }
    }
}
