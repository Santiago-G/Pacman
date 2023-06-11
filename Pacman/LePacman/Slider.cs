using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    public class Slider : SpriteBase
    {
        SliderBar bar;
        Rectangle borderHitbox;
        Color tint;
        int thickness;
        public int numOfBars;
        List<SliderBar> bars = new List<SliderBar>();
        int spacing;
        public int numOfVisibleBars;

        bool pressed = false;
        bool pressedOutside = false;
        bool outside = false;

        public Slider(SliderBar Bar, Vector2 Position, int BorderThickness, Color Tint, int NumberOfBars, int Spacing) : base(null, Position, Tint)
        {
            bar = Bar;
            tint = Tint;
            thickness = BorderThickness;
            numOfBars = NumberOfBars;
            spacing = Spacing;

            numOfBars = Math.Abs(numOfBars);

            if (numOfBars == 0)
            {
                throw new Exception("Bars?");
            }
        }

        public void UpdatePositions(Vector2 newPosition)
        {
            borderHitbox.X = (int)(newPosition.X);
            borderHitbox.Y = (int)(newPosition.Y);

            foreach (var bars in bars)
            {
                bars.Hitbox.X += borderHitbox.X;
                bars.Hitbox.Y += borderHitbox.Y;
            }
        }

        public void LoadContent(ContentManager Content)
        {
            borderHitbox = new Rectangle((int)Position.X, (int)Position.Y, thickness * 2 + bar.Hitbox.Width * numOfBars + spacing * (numOfBars + 1), bar.Hitbox.Height + thickness * 2 + spacing * 2);
            ;
            bars.Add(new SliderBar(bar.Image, new Vector2(borderHitbox.X + thickness + spacing, borderHitbox.Y + thickness + spacing), bar.Hitbox.Width, bar.Hitbox.Height, bar.Tint));
            bars[0].BarNumber = 0;

            for (int i = 1; i < numOfBars; i++)
            {
                bars.Add(new SliderBar(bar.Image, new Vector2(bars[0].Hitbox.X + spacing * i + bar.Hitbox.Width * i, bars[0].Hitbox.Y), bar.Hitbox.Width, bar.Hitbox.Height, bar.Tint));
                bars[i].BarNumber = i;
            }
        }


        MouseState prevMouseState;
        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            if (borderHitbox.Contains(ms.Position))
            {
                if (outside && ms.LeftButton == ButtonState.Pressed)
                {
                    pressedOutside = true;
                    outside = false;
                }
                else
                {
                    outside = false;
                }
                if (ms.LeftButton == ButtonState.Pressed && prevMouseState != ms && !pressedOutside)
                {
                    pressed = true;
                }
                else if (ms.LeftButton != ButtonState.Pressed)
                {
                    pressed = false;
                    //          Game1.WindowText = "";
                    pressedOutside = false;
                }

                if (pressed)
                {
                    //          Game1.WindowText = "In pressed";
                    foreach (var bar in bars)
                    {
                        if (bar.Hitbox.Contains(ms.Position))
                        {
                            numOfVisibleBars = bar.BarNumber;
                            break;
                        }
                    }
                }
            }
            else
            {
                pressed = false;
                //         Game1.WindowText = "";
                outside = true;
            }

            Game1.WindowText = $"Pressed:{pressed}, Outside: {outside}, Pressed Outside: {pressedOutside}";

            prevMouseState = Mouse.GetState();
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.DrawRectangle(borderHitbox, tint, thickness);

            for (int i = 0; i <= numOfVisibleBars; i++)
            {
                bars[i].Draw(batch);
            }
        }


    }
}
