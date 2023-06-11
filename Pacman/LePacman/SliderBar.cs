using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Content;

namespace Pacman
{
    public class SliderBar : SpriteBase
    {
        public Rectangle Hitbox;
        public int BarNumber;

        public SliderBar(Texture2D Image, Vector2 Position, int Width, int Height, Color Tint) : base(Image, Position, Tint)
        {
            Hitbox = new Rectangle((int)(base.Position.X), (int)(base.Position.Y), Width, Height);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, Hitbox, Tint);
        }
    }
}
