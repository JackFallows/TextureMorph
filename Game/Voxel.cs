using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TextureMorph
{
    public class Voxel : IActor
    {
        private readonly Vector2 position;
        private readonly Color color;
        private readonly float scale;

        public Voxel(Vector2 position, Color color, float scale = 1.0f)
        {
            this.position = position;
            this.color = color;
            this.scale = scale;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var tex = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            tex.SetData(new[] { Color.White });

            spriteBatch.Draw(tex, position, new Rectangle(0, 0, 1, 1), color, 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
        }
    }
}
