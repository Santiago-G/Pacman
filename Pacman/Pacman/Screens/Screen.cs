using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Pacman
{
    public abstract class Screen
    {
        GraphicsDeviceManager graphics;

        public (int width, int height) size;

        protected List<Sprite> objects = new List<Sprite>();

        public Screen((int width, int height) Size, Vector2 Position, GraphicsDeviceManager Graphics)
        {
            size = Size;
        }

        public abstract void LoadContent(ContentManager Content);

        public virtual void Update(GameTime gameTime)
        {
            foreach (var item in objects)
            {
                item.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in objects)
            {
                item.Draw(spriteBatch);
            }
        }
    }
}
