using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Pacman.Screens;

namespace Pacman.Screens
{
    public class OptionsAudio : Screen
    {
        static Image background;
        static Button audioText;
        static Vector2 screenOrigin => Options.screenOrigin;

        static Slider volumeControl;

        public OptionsAudio((int width, int height) Size, Vector2 Position, GraphicsDeviceManager Graphics, Image BackgroundImage) : base(Size, Position, Graphics)
        {
            size = Size;
            position = Position;
            background = BackgroundImage;
        }

        public override void LoadContent(ContentManager Content)
        {
            audioText = new Button(Content.Load<Texture2D>("audioText"), new Vector2(), Color.White);
            objects.Add(audioText);

            volumeControl = new Slider(new SliderBar(Content.Load<Texture2D>("smallPixel"), new Vector2(0), 10, 20, Color.White), new Vector2(200), 2, Color.White, 10, 3);
            //new Rectangle(new Point(0), new Point(200, 50))
            volumeControl.LoadContent(Content);
            volumeControl.numOfVisibleBars = volumeControl.numOfBars/2 - 1;
            objects.Add(volumeControl);
        }

        static public void setUpPositions()
        {
            audioText.Position = new Vector2(screenOrigin.X + (background.Image.Width / 2 - (audioText.Image.Width / 2)), screenOrigin.Y + 50);

            volumeControl.Position = new Vector2(screenOrigin.X + volumeControl.Position.X, screenOrigin.Y + volumeControl.Position.Y);
        }
    }
}
