using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Pacman.Screens;
using LePacman;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    public class Options : Screen
    {
        public static Vector2 screenOrigin;
        public static Texture2D background;

        public static Sprite menuBackground;

        private static Button audioButton;
        private static Button visualsButton;
        private static Button controlsButton;

        //Slider volumeBar = new Slider(new Rectangle(350, 30, 300, 37), 5, Color.White);

        /* Volume Bar
         * Music Option (Switch between radios that each have their own songs)
         * 
         */

        public Options(Point Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            size = Size;
            position = Position;
        }


        public static void setUpPositions()
        {
            audioButton.Position = new Vector2(background.Width / 2 - (audioButton.Image.Width / 2), screenOrigin.Y + 200);
            visualsButton.Position = new Vector2(background.Width / 2 - (visualsButton.Image.Width / 2), screenOrigin.Y + 350);
            controlsButton.Position = new Vector2(background.Width / 2 - (controlsButton.Image.Width / 2), screenOrigin.Y + 500);
        }

        public override void LoadContent(ContentManager Content)
        {
            menuBackground = new Sprite(Content.Load<Texture2D>("optionsBackground"), new Vector2(), Color.White);

            audioButton = new Button(Content.Load<Texture2D>("audioText"), new Vector2(), Color.White);
            objects.Add(audioButton);

            visualsButton = new Button(Content.Load<Texture2D>("visualsText"), new Vector2(), Color.White);
            objects.Add(visualsButton);

            controlsButton = new Button(Content.Load<Texture2D>("controlsText"), new Vector2(), Color.White);
            objects.Add(controlsButton);


            //SoundEffect effect = Content.Load<SoundEffect>("examplesound");
            //effect.Play();
            //Song song = Content.Load<Song>("examplesong");
            //MediaPlayer.Play(song);
            //objects.Add(volumeBar);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            if (audioButton.IsClicked(ms))
            {
                ScreenManagerPM.Instance.ChangeScreens(GameStates.OptionsAudio);
            }
            else if (visualsButton.IsClicked(ms))
            {
                //Options.currentScreen = Options.OptionScreens.Visual;
            }
            else if (controlsButton.IsClicked(ms))
            {
                //Options.currentScreen = Options.OptionScreens.Controls;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0), Color.DarkGray);
            menuBackground.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
