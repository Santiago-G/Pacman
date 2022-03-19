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
        public static Vector2 screenOrigin;

        static Texture2D background;

        Image audioText;

        Slider volumeBar = new Slider(new Rectangle(350, 30, 300, 37), 5, Color.White);

        /* Volume Bar
         * Music Option (Switch between radios that each have their own songs)
         * 
         */

        public Options((int width, int height) Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            size = Size;
            position = Position;
        }

        public static void GetBackground()
        {
            var gd = graphics.GraphicsDevice;

            Color[] colors = new Color[gd.Viewport.Width * gd.Viewport.Height];
            gd.GetBackBufferData(colors);

            background = new Texture2D(gd, gd.Viewport.Width, gd.Viewport.Height);
            background.SetData(colors);

        }

        public override void LoadContent(ContentManager Content)
        {
            audioText = new Image(Content.Load<Texture2D>("audioText"), new Vector2(), Color.White);
            objects.Add(audioText);

            SoundEffect effect = Content.Load<SoundEffect>("examplesound");
            //effect.Play();

            Song song = Content.Load<Song>("examplesong");
            //MediaPlayer.Play(song);

            objects.Add(volumeBar);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0), Color.White);

            base.Draw(spriteBatch);
        }
    }
}
