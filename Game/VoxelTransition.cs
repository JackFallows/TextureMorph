using System;
using System.Linq.Expressions;
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
        private Vector2 positionSpeed;
        private Vector3 colorSpeed;

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

            var xSpeed = Math.Abs(source.Position.X - target.Position.X) / (durationSeconds * 1000);
            var ySpeed = Math.Abs(source.Position.Y - target.Position.Y) / (durationSeconds * 1000);

            xSpeed = source.Position.X >= target.Position.X ? xSpeed * -1.0f : xSpeed;
            ySpeed = source.Position.Y >= target.Position.Y ? ySpeed * -1.0f : ySpeed;

            positionSpeed = new Vector2(xSpeed, ySpeed);

            var rSpeed = Math.Abs(source.Color.R - target.Color.R) / (durationSeconds * 1000f);
            var gSpeed = Math.Abs(source.Color.G - target.Color.G) / (durationSeconds * 1000f);
            var bSpeed = Math.Abs(source.Color.B - target.Color.B) / (durationSeconds * 1000f);

            rSpeed = source.Color.R >= target.Color.R ? rSpeed * -1.0f : rSpeed;
            gSpeed = source.Color.G >= target.Color.G ? gSpeed * -1.0f : gSpeed;
            bSpeed = source.Color.B >= target.Color.B ? bSpeed * -1.0f : bSpeed;

            colorSpeed = new Vector3(rSpeed, gSpeed, bSpeed);
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

            var elapsed = gameTime.TotalGameTime - startTime.Value;
            var elapsedMilliseconds = elapsed.TotalMilliseconds;

            var xDiff = (float)(positionSpeed.X * elapsedMilliseconds);
            var yDiff = (float)(positionSpeed.Y * elapsedMilliseconds);

            var rDiff = (float)(colorSpeed.X * elapsedMilliseconds);
            var gDiff = (float)(colorSpeed.Y * elapsedMilliseconds);
            var bDiff = (float)(colorSpeed.Z * elapsedMilliseconds);

            active.Position = new Vector2(source.Position.X + xDiff, source.Position.Y + yDiff);
            active.Color = new Color((int)Math.Floor(source.Color.R + rDiff), (int)Math.Floor(source.Color.G + gDiff), (int)Math.Floor(source.Color.B + bDiff));
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
    }
}
