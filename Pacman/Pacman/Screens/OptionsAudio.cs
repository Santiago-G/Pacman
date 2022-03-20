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

        public OptionsAudio((int width, int height) Size, Vector2 Position, GraphicsDeviceManager Graphics, Image BackgroundImage) : base(Size, Position, Graphics)
        {
            size = Size;
            position = Position;
            background = BackgroundImage;
        }

        static public void setUpPositions()
        {
            audioText.Position = new Vector2(screenOrigin.X + (background.image.Width / 2 - (audioText.Image.Width / 2)), screenOrigin.Y + 50);
        }

        public override void LoadContent(ContentManager Content)
        {
            audioText = new Button(Content.Load<Texture2D>("audioText"), new Vector2(), Color.White);
            objects.Add(audioText);
        }
    }
}
