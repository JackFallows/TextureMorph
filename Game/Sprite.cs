using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TextureMorph
{
    public class Sprite : IActor
    {
        private readonly Texture2D texture;
        
        private Vector2 position;
        private int scale;
        private int drawOrder;

        public Sprite(Texture2D texture, Vector2 position, int scale = 1, int drawOrder = 0)
        {
            this.texture = texture;
            this.position = position;
            this.scale = scale;

            this.drawOrder = drawOrder;
        }

        public List<Voxel> GetVoxels()
        {
            var pixels = GetPixels();
            var dimensions = GetDimensions();

            var pixels2D = Get2DPixels(pixels, dimensions);

            var voxels = new List<Voxel>();

            for (var y = 0; y < pixels2D.GetLength(0); y++)
            {
                for (var x = 0; x < pixels2D.GetLength(1); x++)
                {
                    var p = pixels2D[y, x];
                    voxels.Add(new Voxel(new Vector2(position.X + (scale * x), position.Y + (scale * y)), p, scale, drawOrder));
                }
            }

            return voxels;
        }

        private Color[,] Get2DPixels(Color[] pixels, Vector2 dimensions)
        {
            Color[,] rawDataAsGrid = new Color[(int)dimensions.Y, (int)dimensions.X];
            for (int row = 0; row < (int)dimensions.Y; row++)
            {
                for (int column = 0; column < (int)dimensions.X; column++)
                {
                    // Assumes row major ordering of the array.
                    rawDataAsGrid[row, column] = pixels[row * (int)dimensions.X + column];
                }
            }

            return rawDataAsGrid;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
        }

        private Color[] GetPixels()
        {
            var rawData = new Color[texture.Width * texture.Height];
            texture.GetData(rawData);
            return rawData;
        }

        private Vector2 GetDimensions()
        {
            return new Vector2(texture.Width, texture.Height);
        }
    }
}
