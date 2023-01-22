using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Pacman
{
    public abstract class Screen
    {
        public static GraphicsDeviceManager graphics;

        public Point size;

        public Vector2 position;

        protected List<Sprite> objects = new List<Sprite>();

        public Screen(Point Size, Vector2 Position, GraphicsDeviceManager Graphics)
        {
            size = Size;
            position = Position;
            graphics = Graphics;
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
