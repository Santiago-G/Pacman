using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended;

namespace Pacman
{
    public class Slider : Sprite
    {
        Rectangle hitbox;
        Color tint;
        float thickness;
        int numOfBars;

        public Slider(Rectangle rectangle, float BorderThickness, Color Tint, bool TenPercent=true) : base(null, new Vector2(rectangle.X, rectangle.Y), Tint)
        {
            hitbox = rectangle;
            tint = Tint;
            thickness = BorderThickness;

            if (TenPercent)
            {
                numOfBars = 10;
            }
            else
            {
                numOfBars = 5;
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.DrawRectangle(hitbox, tint, thickness);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
