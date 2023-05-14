using LePacman;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using Pacman.Screens;
using System.Collections.Generic;
using System.IO;


namespace Pacman
{
    public class Game1 : Game
    {
        
        ScreenManagerPM screenManager = ScreenManagerPM.Instance;
        PopUpManager popUpManager = PopUpManager.Instance;

        //0 is normal pacman map
        string[] savedMaps = new string[4];
        
        public static float MasterVolume = .5f;
        public static float SFXVolume = .5f;
        public static float MusicVolume = .5f;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static MouseState prevMS = Mouse.GetState();


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
            screenManager.setGraphics(_graphics);

            ChangeResolution(800, 800);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            screenManager.LoadContent(Content);
            popUpManager.LoadContent(Content);

            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData(new Color[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Exit();

            // TODO: Add your update logic here

            MouseState ms = Mouse.GetState();

            if(!popUpManager.pauseScene)
            {
                screenManager.Update(gameTime);
            }

            popUpManager.Update(gameTime);


            //Window.Title = Game1.WindowText; 
            prevMS = Mouse.GetState();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            screenManager.Draw(_spriteBatch);
            popUpManager.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
