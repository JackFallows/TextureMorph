using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TextureMorph
{
    public class VoxelSprite
    {
        private readonly Sprite sprite;
        
        public VoxelSprite(Sprite sprite)
        {
            this.sprite = sprite;
        }

        public Voxel[] GetVoxels()
        {
            var pixels = sprite.GetPixels();
            var dimensions = sprite.GetDimensions();
            var position = sprite.Position;

            var pixels2D = Get2DPixels(pixels, dimensions);

            var voxels = new List<Voxel>();

            for (var y = 0; y < pixels2D.GetLength(0); y++)
            {
                for (var x = 0; x < pixels2D.GetLength(1); x++)
                {
                    var p = pixels2D[y, x];
                    voxels.Add(new Voxel(new Vector2(position.X + (sprite.Scale * x), position.Y + (sprite.Scale * y)), p, sprite.Scale));
                }
            }

            return voxels.ToArray();
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
    }
}
