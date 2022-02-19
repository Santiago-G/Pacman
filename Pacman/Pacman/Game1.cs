using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    public class Game1 : Game
    {
        enum GameStates
        {
            TitleScreen,
            MapEditor,
            MainGame,
            Options
        }

        GameStates gameStates = GameStates.TitleScreen;

        TitleScreen titleScreen = new TitleScreen();
        MapEditor mapEditor = new MapEditor();

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public void ChangeResolution(int width, int height)
        {
            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            _graphics.ApplyChanges();
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            ChangeResolution(800, 1070);
            //800, 1070
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            switch (gameStates)
            {
                case GameStates.TitleScreen:
                    titleScreen.Update();
                    break;
                case GameStates.MapEditor:
                    mapEditor.Update();
                    break;
                case GameStates.MainGame:
                    break;
                case GameStates.Options:
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            switch (gameStates)
            {
                case GameStates.TitleScreen:
                    titleScreen.Update();
                    break;
                case GameStates.MapEditor:
                    mapEditor.Update();
                    break;
                case GameStates.MainGame:
                    break;
                case GameStates.Options:
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
