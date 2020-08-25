using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace TextureMorph
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Sprite sprite1;
        private Sprite sprite2;

        private Voxel[] sprite1Voxels;
        private Voxel[] sprite2Voxels;

        private VoxelTransition[] spriteTransitions;

        private const int Width = 640;
        private const int Height = 480;

        private bool transitionStarted = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = Width;
            _graphics.PreferredBackBufferHeight = Height;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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

            sprite1 = new Sprite(Content.Load<Texture2D>("sprite1"), new Vector2(Width * 0.25f, Height * 0.5f), 4);
            sprite2 = new Sprite(Content.Load<Texture2D>("sprite2"), new Vector2(Width * 0.75f, Height * 0.5f), 4);

            sprite1Voxels = new VoxelSprite(sprite1).GetVoxels();
            sprite2Voxels = new VoxelSprite(sprite2).GetVoxels();

            spriteTransitions = sprite1Voxels.Select((v, i) => new VoxelTransition(v, sprite2Voxels[i])).ToArray();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !transitionStarted)
            {
                transitionStarted = true;
                foreach(var vox in spriteTransitions)
                {
                    vox.Start(gameTime, 3);
                }
            }

            // TODO: Add your update logic here


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            ////sprite1.Draw(gameTime, _spriteBatch);
            ////sprite2.Draw(gameTime, _spriteBatch);

            ////sprite1Voxels[0].Draw(gameTime, _spriteBatch);

            ////foreach (var vox in sprite1Voxels)
            ////{
            ////    vox.Draw(gameTime, _spriteBatch);
            ////}

            foreach (var vox in spriteTransitions)
            {
                vox.Update(gameTime);
                vox.Draw(gameTime, _spriteBatch);
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
