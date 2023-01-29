using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman;
using System.Collections.Generic;

namespace LePacman
{
    public abstract class PopUp
    {
        //public static GraphicsDeviceManager graphics;

        public Point size;
        public Vector2 position;

        protected List<Sprite> objects = new List<Sprite>();

        private SpriteFont headerFont;
        private SpriteFont bodyFont;

        private string headerText;
        private string bodyText;

        private Vector2 headerPos;
        private Vector2 bodyPos;

        private Texture2D background;
        private Color tint;

        private bool Visable = false;

        public PopUp(Texture2D Background, Point Size, Vector2 Position, Color Tint, SpriteFont HeaderFont, SpriteFont BodyFont, string HeaderText,
            string BodyText, Vector2 HeaderPos, Vector2 bodyPos)
        {
            background = Background;
            size = Size;
            position = Position;
            tint = Tint;

            headerFont = HeaderFont;
            bodyFont = BodyFont;

            headerText = HeaderText;
            bodyText = BodyText;

            headerPos = HeaderPos;
            bodyFont = BodyFont;
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
