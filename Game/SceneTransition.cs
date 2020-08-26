using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TextureMorph
{
    public class SceneTransition
    {
        private const int NumBuckets = 4;
        private readonly VoxelTransition[] voxelTransitions;

        private readonly VoxelTransition[][] voxelTransitionBuckets;
        ////private readonly Thread[] threads;

        public SceneTransition(VoxelTransition[] voxelTransitions)
        {
            this.voxelTransitions = voxelTransitions;

            var bucketSize = voxelTransitions.Length / NumBuckets;

            voxelTransitionBuckets = new VoxelTransition[4][];

            for (int i = 0, x = 0; i < voxelTransitions.Length; i += bucketSize, x++)
            {
                voxelTransitionBuckets[x] = new VoxelTransition[bucketSize];
                Array.Copy(voxelTransitions, i, voxelTransitionBuckets[x], 0, bucketSize);
            }

            ////threads = new Thread[numBuckets];

            ////threads[0] = new Thread(() => { });
            ////threads[0].
        }

        public void Start(GameTime gameTime, int durationSeconds)
        {
            var tasks = new Task[NumBuckets];
            for (var i = 0; i < NumBuckets; i++)
            {
                var voxels = voxelTransitionBuckets[i];
                tasks[i] = Task.Run(() =>
                {
                    foreach(var vox in voxels)
                    {
                        vox.Start(gameTime, durationSeconds);
                    }
                });
            }

            Task.WaitAll(tasks);
        }

        public void Update(GameTime gameTime)
        {
            var tasks = new Task[NumBuckets];
            for (var i = 0; i < NumBuckets; i++)
            {
                var voxels = voxelTransitionBuckets[i];
                tasks[i] = Task.Run(() =>
                {
                    foreach (var vox in voxels)
                    {
                        vox.Update(gameTime);
                    }
                });
            }

            Task.WaitAll(tasks);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var tasks = new Task[NumBuckets];
            for (var i = 0; i < NumBuckets; i++)
            {
                var voxels = voxelTransitionBuckets[i];
                tasks[i] = Task.Run(() =>
                {
                    foreach (var vox in voxels)
                    {
                        vox.Draw(gameTime, spriteBatch);
                    }
                });
            }

            Task.WaitAll(tasks);
        }
    }
}
