using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TextureMorph
{
    public class VoxelTransition
    {
        private readonly Voxel source;
        private readonly Voxel target;

        private TimeSpan? startTime = null;
        private TimeSpan? endTime = null;

        private Voxel active;

        private bool done = false;

        public VoxelTransition(Voxel source, Voxel target)
        {
            this.source = source;
            this.target = target;

            active = new Voxel(source.Position, source.Color, source.Scale);
        }

        public void Start(GameTime currentGameTime, int durationSeconds)
        {
            if (startTime != null)
            {
                return;
            }

            startTime = currentGameTime.TotalGameTime;
            endTime = startTime.Value.Add(TimeSpan.FromSeconds(durationSeconds));
        }

        public void Update(GameTime gameTime)
        {
            if (startTime == null)
            {
                return;
            }

            if (gameTime.TotalGameTime >= endTime.Value)
            {
                done = true;
                return;
            }

            var mu = GetMu(startTime.Value.TotalMilliseconds, endTime.Value.TotalMilliseconds, gameTime.TotalGameTime.TotalMilliseconds);

            var x = CosineInterpolate(source.Position.X, target.Position.X, mu);
            var y = CosineInterpolate(source.Position.Y, target.Position.Y, mu);
            
            var r = CosineInterpolate(source.Color.R, target.Color.R, mu);
            var g = CosineInterpolate(source.Color.G, target.Color.G, mu);
            var b = CosineInterpolate(source.Color.B, target.Color.B, mu);

            active.Position = new Vector2((float)x, (float)y);
            active.Color = new Color((int)r, (int)g, (int)b);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!done)
            {
                active.Draw(gameTime, spriteBatch);
            }
            else
            {
                target.Draw(gameTime, spriteBatch);
            }
        }

        // http://paulbourke.net/miscellaneous/interpolation/
        private static double LinearInterpolate(double v1, double v2, double mu)
        {
            return (v1 * (1 - mu) + v2 * mu);
        }

        private static double CosineInterpolate(double v1, double v2, double mu)
        {
            double mu2;

            mu2 = (1 - Math.Cos(mu * Math.PI)) / 2;
            return (v1 * (1 - mu2) + v2 * mu2);
        }

        // https://stats.stackexchange.com/a/154211
        private static double GetMu(double startMilliseconds, double endMilliseconds, double currentMilliseconds)
        {
            return (currentMilliseconds - startMilliseconds) / (endMilliseconds - startMilliseconds);
        }
    }
}
