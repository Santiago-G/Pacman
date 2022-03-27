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
        Rectangle barHitbox;
        Rectangle borderHitbox;
        Color tint;
        float thickness;
        int numOfBars;
        List<SliderBar> bars = new List<SliderBar>();
        float spacing;

        public Slider(Rectangle Bar, Vector2 Position, float BorderThickness, Color Tint, int NumberOfBars, float Spacing) : base(null, new Vector2(Bar.X, Bar.Y), Tint)
        {
            barHitbox = Bar;
            tint = Tint;
            thickness = BorderThickness;
            numOfBars = NumberOfBars;
            spacing = Spacing;
        }

        public void LoadContent(ContentManager Content)
        {
            borderHitbox = new Rectangle((int)(Position.X), (int)(Position.Y), barHitbox.Width * numOfBars + spacing * (numOfBars - 1) + ,);


        //    int barWidth = (int)(((hitbox.Width - thickness * 2) - (spacing * 2) - spacing * (numOfBars - 1)) / numOfBars);
        //    int barHeight = (int)(hitbox.Height - thickness * 2 - spacing* 2);// - (thickness*2) - spacing);

        //    //if ((hitbox.Width - thickness * 2) % numOfBars >= .5)
        //    //{
        //    //    barWidth ++;
        //    //    hitbox.Width = (int)(barWidth * numOfBars + thickness * 2 + (spacing * 2) - spacing * (numOfBars - 1));
        //    //}

        //    for (int i = 0; i < numOfBars; i++)
        //    {
        //        bars.Add(new SliderBar(Content.Load<Texture2D>("smallPixel"), new Vector2((hitbox.X + thickness + spacing) + barWidth * i + spacing * i, hitbox.Y + thickness + spacing), barWidth, barHeight, Color.Red)) ;
        //        //(hitbox.X + thickness + spacing) + barWidth * i + spacing *i
        //    }
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
