using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman.GraphStuff;
using Pacman.Screens;
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
            Options,
            //Test
        }

        public static GameStates currentScreen = GameStates.TitleScreen;
        GameStates prevScreen;
        public static Dictionary<GameStates, Screen> screens = new Dictionary<GameStates, Screen>();

        public static float MasterVolume = .5f;
        public static float SFXVolume = .5f;
        public static float MusicVolume = .5f;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        KeyboardState prevKeyboardState = Keyboard.GetState();


        public static string WindowText = "";

        public static Texture2D Pixel;
        
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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            screens.Add(GameStates.TitleScreen, new TitleScreen((800, 800), new Vector2(0), _graphics));

            screens.Add(GameStates.MapEditor, new MapEditor((1600, 1000), new Vector2(0), _graphics));

            screens.Add(GameStates.Options, new Options((800, 800), new Vector2(0), _graphics));

            //screens.Add(GameStates.Test, new TestScreen((800, 800), new Vector2(0), _graphics));

            foreach (var screen in screens)
            {
                screen.Value.LoadContent(Content);
            }

            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData(new Color[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Exit();

            // TODO: Add your update logic here

            MouseState ms = Mouse.GetState();



            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && prevKeyboardState != Keyboard.GetState())
            {
                #region debug stuff
                //Color[] colors = new Color[GraphicsDevice.Viewport.Width * GraphicsDevice.Viewport.Height];
                //GraphicsDevice.GetBackBufferData(colors);

                //Texture2D ss = new Texture2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                //ss.SetData(colors);

                //Debug for saving to file
                //using (FileStream memory = new FileStream("ss.png", FileMode.OpenOrCreate))
                //{
                //    ss.SaveAsPng(memory, ss.Width, ss.Height);
                //}
                #endregion

                if (currentScreen != GameStates.Options)
                {
                    prevScreen = currentScreen;
                    Options.SetUpScreen();
                }
                else
                {
                    currentScreen = prevScreen;
                }
            }

            
            screens[currentScreen].Update(gameTime);
            prevKeyboardState = Keyboard.GetState();

            Window.Title = Game1.WindowText; 
            
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
