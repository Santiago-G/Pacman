using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman.GraphStuff
{
    public class Tile : Sprite
    {
        public enum States
        {
            Empty,
            Wall,
        }

        public Texture2D Image;
        public Vector2 Position;
        public Color Tint;

        public bool isVisible;

        public Rectangle Hitbox { get => new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height); set { } }

        public States TileStates = States.Empty;

        public Vector2 Cord;

        public Tile(Texture2D image, Vector2 position, Color tint, Vector2 numb) : base(image, position, tint)
        {
            Image = image;
            Position = position;
            Cord = numb;
            Tint = tint;
        }

        public bool IsClicked()
        {
            return false;
        }


        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            if(isVisible && Hitbox.Intersects(new Rectangle(ms.Position, new Point(1))))
            {
                //enlarge the outline
                //have 2 images, one for each outline
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            if (isVisible)
            {
                //draw the outline
            }
            else
            {
                batch.Draw(Image, Position, Tint);
            }
        }
    }
}
