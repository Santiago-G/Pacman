using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    public class Options : Screen
    {
        Vector2 position;
        GraphicsDeviceManager graphics;

        Image volumeText;

        /* Volume Bar
         * Music Option (Switch between radios that each have their own songs)
         * 
         */

        public Options((int width, int height) Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            size = Size;
            position = Position;
            graphics = Graphics;
        }

        public override void LoadContent(ContentManager Content)
        {
            volumeText = new Image(Content.Load<Texture2D>("volumeText"), new Vector2(30), Color.White);
            objects.Add(volumeText);

            SoundEffect effect = Content.Load<SoundEffect>("examplesound");
            //effect.Play();

            Song song = Content.Load<Song>("examplesong");
            //MediaPlayer.Play(song);
        }
    }
}
