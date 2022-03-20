using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Pacman.Screens;
using Microsoft.Xna.Framework.Input;

namespace Pacman.Screens
{
    public class OptionsMainMenu : Screen
    {
        static Image background;
        static Button audioButton;
        static Button visualsButton;
        static Button controlsButton;
        static Vector2 screenOrigin => Options.screenOrigin;

        public OptionsMainMenu((int width, int height) Size, Vector2 Position, GraphicsDeviceManager Graphics, Image BackgroundImage) : base(Size, Position, Graphics)
        {
            size = Size;
            position = Position;
            background = BackgroundImage;
        }

        static public void setUpPositions()
        {
            audioButton.Position = new Vector2(screenOrigin.X + (background.image.Width / 2 - (audioButton.Image.Width / 2)), screenOrigin.Y + 200);
            visualsButton.Position = new Vector2(screenOrigin.X + (background.image.Width / 2 - (visualsButton.Image.Width / 2)), screenOrigin.Y + 350);
            controlsButton.Position = new Vector2(screenOrigin.X + (background.image.Width / 2 - (controlsButton.Image.Width / 2)), screenOrigin.Y + 500);
        }

        public override void LoadContent(ContentManager Content)
        {
            audioButton = new Button(Content.Load<Texture2D>("audioText"), new Vector2(), Color.White);
            objects.Add(audioButton);

            visualsButton = new Button(Content.Load<Texture2D>("visualsText"), new Vector2(), Color.White);
            objects.Add(visualsButton);

            controlsButton = new Button(Content.Load<Texture2D>("controlsText"), new Vector2(), Color.White);
            objects.Add(controlsButton);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            if (audioButton.IsClicked(ms))
            {
                Options.currentScreen = Options.OptionScreens.Audio;
            }
            else if (visualsButton.IsClicked(ms))
            {
                Options.currentScreen = Options.OptionScreens.Visual;
            }
            else if (controlsButton.IsClicked(ms))
            {
                Options.currentScreen = Options.OptionScreens.Controls;
            }

            base.Update(gameTime);  
        }
    }
}
