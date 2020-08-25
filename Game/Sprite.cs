using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TextureMorph
{
    public class Sprite : IActor
    {
        private readonly Texture2D texture;

        public Sprite(Texture2D texture, Vector2 position, int scale = 1)
        {
            this.texture = texture;
            Position = position;
            Scale = scale;
        }

        public int Scale { get; private set; }

        public Vector2 Position { get; private set; }

        public Color[] GetPixels()
        {
            var rawData = new Color[texture.Width * texture.Height];
            texture.GetData(rawData);
            return rawData;
        }

        public Vector2 GetDimensions()
        {
            return new Vector2(texture.Width, texture.Height);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, new Rectangle(0, 0, 32, 32), Color.White, 0.0f, new Vector2(), Scale, SpriteEffects.None, 0.0f);
        }
    }
}
