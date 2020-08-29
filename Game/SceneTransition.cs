using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TextureMorph
{
    public class SceneTransition
    {
        private readonly Sprite[] sourceSprites;
        private readonly Sprite[] targetSprites;
        
        private bool started;
        private TimeSpan? startTime;

        private VoxelTransition[] voxelTransitions;

        public SceneTransition(Sprite sourceSprite, Sprite targetSprite)
        {
            sourceSprites = new[] { sourceSprite };
            targetSprites = new[] { targetSprite };

            started = false;
            startTime = null;
        }

        public SceneTransition(Sprite[] sourceSprites, Sprite[] targetSprites)
        {
            this.sourceSprites = sourceSprites;
            this.targetSprites = targetSprites;

            started = false;
        }

        public void Start(GameTime gameTime)
        {
            if (started)
            {
                return;
            }

            var sourceVoxels = sourceSprites.SelectMany(s => s.GetVoxels()).ToArray();

            var rnd = new Random();
            var targetVoxels = targetSprites.SelectMany(s => s.GetVoxels()).OrderBy(v => rnd.Next()).ToArray();    // shuffle the target array

            if (targetVoxels.Length > sourceVoxels.Length)
            {
                var numToDuplicate = targetVoxels.Length - sourceVoxels.Length;
                sourceVoxels = WithDuplicatedVoxels(sourceVoxels, numToDuplicate);
            }
            else if (sourceVoxels.Length > targetVoxels.Length)
            {
                var numToDuplicate = sourceVoxels.Length - targetVoxels.Length;
                targetVoxels = WithDuplicatedVoxels(targetVoxels, numToDuplicate);
            }

            var range = 2;  // min + range = maximum duration of transition
            var min = 2;    // minimum duration of transition

            voxelTransitions = sourceVoxels
                .Select((v, i) =>
                {
                    var d = (rnd.NextDouble() * range) + min;

                    var vox = new VoxelTransition(v, targetVoxels[i], new Vector2(400, 300), (int)(d * 1000));
                    return vox;
                })
                .OrderBy(v => v.DrawOrder)
                .ToArray();

            started = true;
            startTime = gameTime.TotalGameTime;
        }

        public void Update(GameTime gameTime)
        {
            if (!started)
            {
                return;
            }

            const int transitionStartIntervalMilliseconds = 1;

            for (var i = 0; i < voxelTransitions.Length; i++)
            {
                var vox = voxelTransitions[i];

                if (vox.Started)
                {
                    vox.Update(gameTime);
                }
                else
                {
                    var elapsed = gameTime.TotalGameTime.TotalMilliseconds - startTime.Value.TotalMilliseconds;
                    if (elapsed >= (i * transitionStartIntervalMilliseconds))
                    {
                        vox.Start(gameTime);
                        vox.Update(gameTime);
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!started)
            {
                foreach(var sprite in sourceSprites)
                {
                    sprite.Draw(gameTime, spriteBatch);
                }

                return;
            }

            foreach (var vox in voxelTransitions)
            {
                vox.Draw(gameTime, spriteBatch);
            }
        }

        private Voxel[] WithDuplicatedVoxels(Voxel[] voxels, int numToDuplicate)
        {
            var duplicates = new List<Voxel>();

            var rnd = new Random();
            for (var i = 0; i < numToDuplicate; i++)
            {
                var indexToDupe = rnd.Next(voxels.Length);
                duplicates.Add(voxels[indexToDupe].GetCopy());
            }

            var temp = voxels.ToList();
            temp.AddRange(duplicates);

            return temp.ToArray();
        }
    }
}
