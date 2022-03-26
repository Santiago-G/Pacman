using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Pacman
{
    public abstract class Sprite
    {
        public Texture2D Image;
        public Vector2 Position;
        public Color Tint;

        public Sprite(Texture2D Image, Vector2 Position, Color Tint)
        {
            this.Image = Image;
            this.Position = Position;
            this.Tint = Tint;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch batch);
    }
}
