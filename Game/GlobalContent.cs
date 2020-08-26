using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TextureMorph
{
    public static class GlobalContent
    {
        public static void Initialise(GraphicsDevice graphicsDevice)
        {
            White = CreateWhiteTexture(graphicsDevice);
        }

        public static Texture2D White { get; private set; }

        private static Texture2D CreateWhiteTexture(GraphicsDevice graphicsDevice)
        {
            var tex = new Texture2D(graphicsDevice, 1, 1);
            tex.SetData(new[] { Color.White });
            return tex;
        }
    }
}
