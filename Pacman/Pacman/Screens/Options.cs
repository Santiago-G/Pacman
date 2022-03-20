using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Pacman.Screens;

namespace Pacman
{
    public class Options : Screen
    {
        public enum OptionScreens
        {
            MainMenu,
            Audio,
            Visual,
            Controls
        }

        public static OptionScreens currentScreen = OptionScreens.MainMenu;
        Dictionary<OptionScreens, Screen> screens = new Dictionary<OptionScreens, Screen>();

        public static Vector2 screenOrigin;
        static Texture2D background;

        static Image menuBackground;

        //Slider volumeBar = new Slider(new Rectangle(350, 30, 300, 37), 5, Color.White);

        /* Volume Bar
         * Music Option (Switch between radios that each have their own songs)
         * 
         */

        public Options((int width, int height) Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            size = Size;
            position = Position;
        }

        public static void SetUpScreen()
        {
            GetBackground();
            Game1.currentScreen = Game1.GameStates.Options;

            screenOrigin = new Vector2((background.Width/2) - (menuBackground.image.Width/2), (background.Height / 2) - (menuBackground.image.Height / 2));
            currentScreen = OptionScreens.MainMenu;
            menuBackground.position = screenOrigin;

            OptionsMainMenu.setUpPositions();
            OptionsAudio.setUpPositions();
        }

        static void GetBackground()
        {
            var gd = graphics.GraphicsDevice;

            Color[] colors = new Color[gd.Viewport.Width * gd.Viewport.Height];
            gd.GetBackBufferData(colors);

            background = new Texture2D(gd, gd.Viewport.Width, gd.Viewport.Height);
            background.SetData(colors);
        }

        public override void LoadContent(ContentManager Content)
        {
            menuBackground = new Image(Content.Load<Texture2D>("optionsBackground"), new Vector2(), Color.White);

            screens.Add(OptionScreens.MainMenu, new OptionsMainMenu((800, 800), screenOrigin, graphics, menuBackground));
            screens.Add(OptionScreens.Audio, new OptionsAudio((800, 800),  screenOrigin, graphics, menuBackground));
            //screens.Add(OptionScreens.MainMenu, new OptionsMainMenu());

            
            //objects.Add(menuBackground);

            foreach (var screen in screens)
            {
                screen.Value.LoadContent(Content);
            }

            SoundEffect effect = Content.Load<SoundEffect>("examplesound");
            //effect.Play();

            Song song = Content.Load<Song>("examplesong");
            //MediaPlayer.Play(song);

            //objects.Add(volumeBar);
        }

        public override void Update(GameTime gameTime)
        {
            screens[currentScreen].Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0), Color.DarkGray);
            menuBackground.Draw(spriteBatch);

            screens[currentScreen].Draw(spriteBatch);
        }
    }
}
