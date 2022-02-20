using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    public class Game1 : Game
    {
        public enum GameStates
        {
            TitleScreen,
            MapEditor,
            MainGame,
            Options
        }

        public static GameStates gameStates = GameStates.TitleScreen;

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

            ChangeResolution(800, 800);
            //800, 1070 for titleScreen & Options(allow options to scroll if you dont have enough room)
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            titleScreen.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            MouseState ms = Mouse.GetState();

            switch (gameStates)
            {
                case GameStates.TitleScreen:
                    titleScreen.Update(ms);
                    break;
                case GameStates.MapEditor:
                    mapEditor.Update();
                    break;
                case GameStates.MainGame:
                    ;
                    break;
                case GameStates.Options:
                    ;
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            switch (gameStates)
            {
                case GameStates.TitleScreen:
                    titleScreen.Draw(_spriteBatch);
                    break;
                case GameStates.MapEditor:
                    mapEditor.Draw();
                    break;
                case GameStates.MainGame:
                    break;
                case GameStates.Options:
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
