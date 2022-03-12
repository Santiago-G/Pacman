using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended;

namespace Pacman.Content
{
    public class Slider : Sprite
    {
        Rectangle hitbox;
        Color tint;
        float thickness;

        public Slider(Rectangle rectangle, float Thickness, Color Tint) : base(null, new Vector2(rectangle.X, rectangle.Y), Tint)
        {
            hitbox = rectangle;
            tint = Tint;
            thickness = Thickness;
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.DrawRectangle(hitbox, tint, thickness);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
