using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TextureMorph
{
    public class Voxel : IActor
    {

        public Voxel(Vector2 position, Color color, float scale = 1.0f)
        {
            Position = position;
            Color = color;
            Scale = scale;
        }

        public Vector2 Position { get; set; }

        public Color Color { get; set; }

        public float Scale { get; set; }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var tex = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            tex.SetData(new[] { Color.White });

            spriteBatch.Draw(tex, Position, new Rectangle(0, 0, 1, 1), Color, 0.0f, new Vector2(), Scale, SpriteEffects.None, 0.0f);
        }
    }
}
