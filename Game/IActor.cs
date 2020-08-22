using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TextureMorph
{
    public interface IActor
    {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}