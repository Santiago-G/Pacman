using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman
{
    public class Image : Sprite
    {
        public Texture2D image;
        public Vector2 position;
        Color tint;

        public Image(Texture2D texture2D, Vector2 Position, Color Tint) : base(texture2D, Position, Tint)
        {
            image = texture2D;
            position = Position;
            tint = Tint;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(image, position, tint);
        }
    }
}
