using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Content;

namespace Pacman
{
    public class Slider : Sprite
    {
        Rectangle hitbox;
        Color tint;
        float thickness;
        int numOfBars;
        List<Image> bars = new List<Image>();

        public Slider(Rectangle rectangle, float BorderThickness, Color Tint, int NumberOfBars) : base(null, new Vector2(rectangle.X, rectangle.Y), Tint)
        {
            hitbox = rectangle;
            tint = Tint;
            thickness = BorderThickness;
            numOfBars = NumberOfBars;
        }

        public void LoadContent(ContentManager Content)
        {
            for (int i = 0; i < numOfBars; i++)
            {
                bars.Add(new Image(Content.Load<Texture2D>("pixel"), new Vector2(hitbox.X + thickness + (hitbox.Width/20) + /*cacluate the width of the bar * i*/, hitbox.Y + thickness), tint));
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
