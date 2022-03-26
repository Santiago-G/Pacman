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
        List<SliderBar> bars = new List<SliderBar>();
        float spacing;

        public Slider(Rectangle rectangle, float BorderThickness, Color Tint, int NumberOfBars, float Spacing) : base(null, new Vector2(rectangle.X, rectangle.Y), Tint)
        {
            hitbox = rectangle;
            tint = Tint;
            thickness = BorderThickness;
            numOfBars = NumberOfBars;
            spacing = Spacing;
        }

        public void LoadContent(ContentManager Content)
        {
            int barWidth = (int)((hitbox.Width - (thickness*2) - (spacing * (numOfBars + 1)))/numOfBars);
            int barHeight = (int)(hitbox.Height - (thickness*2) - spacing);

            for (int i = 0; i < numOfBars; i++)
            {
                bars.Add(new SliderBar(Content.Load<Texture2D>("smallPixel"), new Vector2(hitbox.X + thickness + spacing + (barHeight * i), hitbox.Y + thickness + spacing), barHeight, barWidth, tint));
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.DrawRectangle(hitbox, tint, thickness);

            foreach (var bar in bars)
            {
                bar.Draw(batch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
