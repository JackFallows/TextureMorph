using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TextureMorph
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SceneTransition sceneTransition;

        private const int Width = 800;
        private const int Height = 600;

        private FrameCounter frameCounter = new FrameCounter();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            _graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            base.Initialize();

            _graphics.PreferredBackBufferWidth = Width;
            _graphics.PreferredBackBufferHeight = Height;
            _graphics.ApplyChanges();
         
            GlobalContent.Initialise(_graphics.GraphicsDevice);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var sprite1 = new Sprite(Content.Load<Texture2D>("sprite1"), new Vector2(Width * 0.25f, Height * 0.5f), 4, 0);
            var sprite2 = new Sprite(Content.Load<Texture2D>("sprite2"), new Vector2(Width * 0.65f, Height * 0.35f), 4, 1);
            var sprite3 = new Sprite(Content.Load<Texture2D>("sprite3"), new Vector2(Width * 0.75f, Height * 0.5f), 4, 2);

            sceneTransition = new SceneTransition(new[] { sprite1 }, new[] { sprite2, sprite3 });
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                sceneTransition.Start(gameTime);
            }

            sceneTransition.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, null);

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            frameCounter.Update(deltaTime);

            var fps = string.Format("FPS: {0}", frameCounter.AverageFramesPerSecond);

            Debug.WriteLine(fps);

            sceneTransition.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
