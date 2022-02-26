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

        protected Dictionary<string, Sprite> objects = new Dictionary<string, Sprite>();

        public Screen((int, int) Size, Vector2 Position, GraphicsDeviceManager Graphics)
        {
            graphics = Graphics;
        }

        public abstract void LoadContent(ContentManager Content);

        public virtual void Update(GameTime gameTime)
        {
            foreach (var item in objects)
            {
                item.Value.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in objects)
            {
                item.Value.Draw(spriteBatch);
            }
        }
    }
}
