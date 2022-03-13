using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman.GraphStuff;
using System.Collections.Generic;
using System.IO;

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

        public static GameStates currentScreen = GameStates.TitleScreen;
        public static Dictionary<GameStates, Screen> screens = new Dictionary<GameStates, Screen>();

        public static float MasterVolume = .7f;
        public static float SFXVolume = .7f;
        public static float MusicVolume = .7f;

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

            screens.Add(GameStates.TitleScreen, new TitleScreen((800, 800), new Vector2(0), _graphics));

            screens.Add(GameStates.MapEditor, new MapEditor((1600, 1000), new Vector2(0), _graphics));

            screens.Add(GameStates.Options, new Options((800, 800), new Vector2(0), _graphics));

            foreach (var screen in screens)
            {
                screen.Value.LoadContent(Content);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Exit();

            // TODO: Add your update logic here

            MouseState ms = Mouse.GetState();

            // screens[currentScreen].Update();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Color[] colors = new Color[GraphicsDevice.Viewport.Width * GraphicsDevice.Viewport.Height];
                GraphicsDevice.GetBackBufferData(colors);

                Texture2D ss = new Texture2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                ss.SetData(colors);

                //Debug for saving to file
                using (FileStream memory = new FileStream("ss.png", FileMode.OpenOrCreate))
                {
                    ss.SaveAsPng(memory, ss.Width, ss.Height);
                }

            }

            screens[currentScreen].Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            screens[currentScreen].Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
