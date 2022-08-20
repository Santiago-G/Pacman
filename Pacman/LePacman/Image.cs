using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman
{
    public class Image : Sprite
    {
        public Image(Texture2D texture2D, Vector2 Position, Color Tint) : base(texture2D, Position, Tint)
        {
        }

        public Image(Texture2D Image, Vector2 Position, Color Tint, Vector2 Scale, Vector2 Origin, float Rotation, SpriteEffects spriteEffects) : base(Image, Position, Tint, Scale, Origin, Rotation, spriteEffects)
        {       
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch batch)
        {
            //batch.Draw(Image, Position, Tint);
            batch.Draw(Image, Position, null, Tint, Rotation, Origin, Scale, SpriteEffects, 0);
        }
    }
}
