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
        private double centerRadius;

        private TimeSpan? startTime = null;
        private TimeSpan? middleTime = null;
        private TimeSpan? endTime = null;

        private double startAngle;
        private double endAngle;

        private Voxel active;

        private bool done = false;

        public VoxelTransition(Voxel source, Voxel target, Vector2 center)
        {
            this.source = source;
            this.target = target;
            this.center = center;

            centerRadius = Math.Sqrt(Math.Pow(source.Position.X - center.X, 2) + Math.Pow(source.Position.Y - center.Y, 2)); // (x - x0)2 + (y - y0)2 = r2 http://mathcentral.uregina.ca/QQ/database/QQ.09.07/s/raymund1.html

            var x = center.X - source.Position.X;
            startAngle = Math.Acos(x / centerRadius);

            if (source.Position.Y > center.Y)
            {
                startAngle = -startAngle;
            }

            endAngle = ((startAngle * (180 / Math.PI)) + 180) * (Math.PI / 180); // convert to degrees (r * 180 / PI), add 180 degrees, convert back to radians (d * PI / 180)

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

            double linearMu;
            double x, y;

            if (gameTime.TotalGameTime < middleTime.Value)  // first transition
            {
                linearMu = GetLinearMu(startTime.Value.TotalMilliseconds, middleTime.Value.TotalMilliseconds, gameTime.TotalGameTime.TotalMilliseconds);
                var cubicEaseInMu = GetCubicEaseInMu(linearMu);

                var currentAngle = Interpolate(startAngle, endAngle, cubicEaseInMu);

                x = center.X - (Math.Cos(currentAngle) * centerRadius);
                y = center.Y - (Math.Sin(currentAngle) * centerRadius);
            }
            else // last transition
            {
                linearMu = GetLinearMu(middleTime.Value.TotalMilliseconds, endTime.Value.TotalMilliseconds, gameTime.TotalGameTime.TotalMilliseconds);
                var cubicEaseOutMu = GetCubicEaseOutMu(linearMu);

                var sourcePosition = new Vector2((float)(center.X - (Math.Cos(endAngle) * centerRadius)), (float)(center.Y - (Math.Sin(endAngle) * centerRadius)));
                var targetPosition = target.Position;

                x = Interpolate(sourcePosition.X, targetPosition.X, cubicEaseOutMu);
                y = Interpolate(sourcePosition.Y, targetPosition.Y, cubicEaseOutMu);
            }

            linearMu = GetLinearMu(startTime.Value.TotalMilliseconds, endTime.Value.TotalMilliseconds, gameTime.TotalGameTime.TotalMilliseconds);
            var r = Interpolate(source.Color.R, target.Color.R, linearMu);
            var g = Interpolate(source.Color.G, target.Color.G, linearMu);
            var b = Interpolate(source.Color.B, target.Color.B, linearMu);

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
        ////private static double GetCubicEaseInOutMu(double linearMu)
        ////{
        ////    return linearMu < 0.5
        ////        ? 4 * linearMu * linearMu * linearMu
        ////        : 1 - Math.Pow(-2 * linearMu + 2, 3) / 2;
        ////}

        // https://easings.net/#easeInCubic
        private static double GetCubicEaseInMu(double linearMu)
        {
            return Math.Pow(linearMu, 3);
        }

        // https://easings.net/#easeOutCubic
        private static double GetCubicEaseOutMu(double linearMu)
        {
            return 1 - Math.Pow(1 - linearMu, 3);
        }
    }
}
