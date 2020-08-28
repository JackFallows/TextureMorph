using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TextureMorph
{
    public class VoxelTransition
    {
        private readonly Voxel source;
        private readonly Voxel target;

        private readonly Vector2 center;    // transition around a circle at this position
        private int centerRadius;

        private TimeSpan? startTime = null;
        private TimeSpan? middleTime = null;
        private TimeSpan? endTime = null;
        private Voxel active;

        private bool done = false;

        public VoxelTransition(Voxel source, Voxel target, Vector2 center)
        {
            this.source = source;
            this.target = target;
            this.center = center;

            var rnd = new Random();
            centerRadius = rnd.Next(50, 60);

            active = new Voxel(source.Position, source.Color, source.Scale);
        }

        public int DrawOrder => target.DrawOrder;

        public void Start(GameTime currentGameTime, int durationMilliseconds)
        {
            if (startTime != null)
            {
                return;
            }

            startTime = currentGameTime.TotalGameTime;
            middleTime = startTime.Value.Add(TimeSpan.FromMilliseconds(durationMilliseconds / 2));
            endTime = startTime.Value.Add(TimeSpan.FromMilliseconds(durationMilliseconds));
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

            double mu;
            Vector2 sourcePosition;
            Vector2 targetPosition;

            if (gameTime.TotalGameTime < middleTime.Value)  // first transition
            {
                mu = GetCubicEaseInMu(startTime.Value.TotalMilliseconds, middleTime.Value.TotalMilliseconds, gameTime.TotalGameTime.TotalMilliseconds);
                sourcePosition = source.Position;
                targetPosition = new Vector2(center.X, center.Y + centerRadius);
            }
            else // last transition
            {
                mu = GetCubicEaseOutMu(middleTime.Value.TotalMilliseconds, endTime.Value.TotalMilliseconds, gameTime.TotalGameTime.TotalMilliseconds);
                sourcePosition = new Vector2(center.X, center.Y + centerRadius);
                targetPosition = target.Position;
            }

            var x = Interpolate(sourcePosition.X, targetPosition.X, mu);
            var y = Interpolate(sourcePosition.Y, targetPosition.Y, mu);
            
            var r = Interpolate(source.Color.R, target.Color.R, mu);
            var g = Interpolate(source.Color.G, target.Color.G, mu);
            var b = Interpolate(source.Color.B, target.Color.B, mu);

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
        private static double Interpolate(double v1, double v2, double mu)
        {
            return (v1 * (1 - mu) + v2 * mu);
        }

        // https://stats.stackexchange.com/a/154211
        private static double GetLinearMu(double startMilliseconds, double endMilliseconds, double currentMilliseconds)
        {
            return (currentMilliseconds - startMilliseconds) / (endMilliseconds - startMilliseconds);
        }

        ////private static double GetCosineMu(double startMilliseconds, double endMilliseconds, double currentMilliseconds)
        ////{
        ////    var linearMu = GetLinearMu(startMilliseconds, endMilliseconds, currentMilliseconds);
        ////    var mu = (1 - Math.Cos(linearMu * Math.PI)) / 2;
        ////    return mu;
        ////}

        // https://easings.net/#easeInOutCubic
        ////private static double GetCubicBezierMu(double startMilliseconds, double endMilliseconds, double currentMilliseconds)
        ////{
        ////    var linearMu = GetLinearMu(startMilliseconds, endMilliseconds, currentMilliseconds);
        ////    return linearMu < 0.5
        ////        ? 4 * linearMu * linearMu * linearMu
        ////        : 1 - Math.Pow(-2 * linearMu + 2, 3) / 2;
        ////}

        // https://easings.net/#easeInCubic
        private static double GetCubicEaseInMu(double startMilliseconds, double endMilliseconds, double currentMilliseconds)
        {
            var mu = GetLinearMu(startMilliseconds, endMilliseconds, currentMilliseconds);
            return Math.Pow(mu, 3);
        }

        // https://easings.net/#easeOutCubic
        private static double GetCubicEaseOutMu(double startMilliseconds, double endMilliseconds, double currentMilliseconds)
        {
            var mu = GetLinearMu(startMilliseconds, endMilliseconds, currentMilliseconds);
            return 1 - Math.Pow(1 - mu, 3);
        }
    }
}
