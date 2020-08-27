using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TextureMorph
{
    public class SceneTransition
    {
        private readonly Sprite sourceSprite;
        private readonly Sprite targetSprite;
        
        private bool started;

        private VoxelTransition[] voxelTransitions;

        public SceneTransition(Sprite sourceSprite, Sprite targetSprite)
        {
            this.sourceSprite = sourceSprite;
            this.targetSprite = targetSprite;

            started = false;
        }

        public void Start(GameTime gameTime)
        {
            if (started)
            {
                return;
            }

            var sprite1Voxels = sourceSprite.GetVoxels();

            var rnd = new Random();
            var sprite2Voxels = targetSprite.GetVoxels().OrderBy(v => rnd.Next()).ToArray();    // shuffle the target array

            if (sprite2Voxels.Length > sprite1Voxels.Length)
            {
                var numToDuplicate = sprite2Voxels.Length - sprite1Voxels.Length;
                var duplicates = new List<Voxel>();

                for (var i = 0; i < numToDuplicate; i++)
                {
                    var indexToDupe = rnd.Next(sprite1Voxels.Length);
                    duplicates.Add(sprite1Voxels[indexToDupe].GetCopy());
                }

                var temp = sprite1Voxels.ToList();
                temp.AddRange(duplicates);
                sprite1Voxels = temp.ToArray();
            }

            var range = 2;  // min + range = maximum duration of transition
            var min = 1;    // minimum duration of transition

            voxelTransitions = sprite1Voxels.Select((v, i) =>
            {
                var vox = new VoxelTransition(v, sprite2Voxels[i]);
                var d = (rnd.NextDouble() * range) + min;
                vox.Start(gameTime, (int)(d * 1000));

                return vox;
            }).ToArray();

            started = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!started)
            {
                return;
            }

            foreach (var vox in voxelTransitions)
            {
                vox.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!started)
            {
                sourceSprite.Draw(gameTime, spriteBatch);
                return;
            }

            foreach (var vox in voxelTransitions)
            {
                vox.Draw(gameTime, spriteBatch);
            }
        }
    }
}
