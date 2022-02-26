using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Text;

namespace Pacman
{
    public class Button : Sprite
    {
        public Texture2D Image;
        public Vector2 Position;
        public Color Tint;

        bool isPressed = false;

        public Rectangle Hitbox { get => new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height); set { } }

        public Button(Texture2D image, Vector2 pos, Color tint) : base(image, pos, tint)
        {
            Image = image;
            Position = pos;
            Tint = tint;
        }

        public bool IsClicked(MouseState ms)
        {
            if ((ms.LeftButton == ButtonState.Pressed && (Hitbox.Contains(ms.Position))) && !isPressed)
            {
                isPressed = true;
                return true;
            }
            else if (ms.LeftButton == ButtonState.Released)
            {
                isPressed = false;
            }

            return false;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            IsClicked(ms);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, Position, Tint);
        }
    }
}
