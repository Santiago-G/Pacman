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

        public OptionsAudio((int width, int height) Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            size = Size;
            position = Position;
        }

        public override void LoadContent(ContentManager Content)
        {
            audioText = new Button(Content.Load<Texture2D>("audioText"), new Vector2(), Color.White);
            objects.Add(audioText);
        }
    }
}
