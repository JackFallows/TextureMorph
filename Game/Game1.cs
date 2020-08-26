using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TextureMorph
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private VoxelTransition[] spriteTransitions;
        ////private SceneTransition sceneTransition;

        private const int Width = 640;
        private const int Height = 480;

        private bool transitionStarted = false;

        private FrameCounter frameCounter = new FrameCounter();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = Width;
            _graphics.PreferredBackBufferHeight = Height;
            _graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            GlobalContent.Initialise(_graphics.GraphicsDevice);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            var sprite1 = new Sprite(Content.Load<Texture2D>("sprite1"), new Vector2(Width * 0.25f, Height * 0.5f), 4);
            var sprite2 = new Sprite(Content.Load<Texture2D>("sprite2"), new Vector2(Width * 0.75f, Height * 0.5f), 4);

            var sprite1Voxels = new VoxelSprite(sprite1).GetVoxels();

            var rnd = new Random();
            var sprite2Voxels = new VoxelSprite(sprite2).GetVoxels().OrderBy(v => rnd.Next()).ToArray();    // shuffle the target array

            spriteTransitions = sprite1Voxels.Select((v, i) => new VoxelTransition(v, sprite2Voxels[i])).ToArray();
            ////sceneTransition = new SceneTransition(spriteTransitions);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !transitionStarted)
            {
                transitionStarted = true;
                foreach (var vox in spriteTransitions)
                {
                    vox.Start(gameTime, 1);
                }
                ////sceneTransition.Start(gameTime, 1);
            }


            if (transitionStarted)
            {
                foreach (var vox in spriteTransitions)
                {
                    vox.Update(gameTime);
                }

                ////sceneTransition.Update(gameTime);
            }

            // TODO: Add your update logic here


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, null);

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            frameCounter.Update(deltaTime);

            var fps = string.Format("FPS: {0}", frameCounter.AverageFramesPerSecond);

            Debug.WriteLine(fps);
            ////_spriteBatch.DrawString(_spriteFont, fps, new Vector2(1, 1), Color.Black);

            ////sprite1.Draw(gameTime, _spriteBatch);
            ////sprite2.Draw(gameTime, _spriteBatch);

            ////sprite1Voxels[0].Draw(gameTime, _spriteBatch);

            ////foreach (var vox in sprite1Voxels)
            ////{
            ////    vox.Draw(gameTime, _spriteBatch);
            ////}

            ////Parallel.ForEach(spriteTransitions, (vox) =>
            ////{
            ////    vox.Draw(gameTime, _spriteBatch);
            ////});

            ////sceneTransition.Draw(gameTime, _spriteBatch);

            for (var i = 0; i < spriteTransitions.Length; i++)
            {
                spriteTransitions[i].Draw(gameTime, _spriteBatch);
            }

            ////sprite2.Draw(gameTime, _spriteBatch);

            ////foreach (var vox in sprite2Voxels)
            ////{
            ////    vox.Draw(gameTime, _spriteBatch);
            ////}

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
