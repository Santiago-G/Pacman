using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman;
using System.Collections.Generic;

namespace LePacman
{
    public abstract class PopUp : Sprite
    {
        //public static GraphicsDeviceManager graphics;

        public Point size;
        protected bool pauseScreen;

        protected List<Sprite> objects = new List<Sprite>();

        protected SpriteFont headerFont;
        protected SpriteFont bodyFont;

        protected string headerText;
        protected string bodyText;

        protected Vector2 headerPos;
        protected Vector2 bodyPos;


        protected bool Visable = false;

        public PopUp(Texture2D Background, Point Size, Vector2 Position, SpriteFont HeaderFont, SpriteFont BodyFont, string HeaderText, string BodyText,
            Vector2 HeaderPos, Vector2 bodyPos) : base(Background, Position, Color.White)
        {
            size = Size;

            headerFont = HeaderFont;
            bodyFont = BodyFont;

            headerText = HeaderText;
            bodyText = BodyText;

            headerPos = HeaderPos;
            bodyFont = BodyFont;
        }

        public void setVisable(bool newVisable)
        {
            Visable = newVisable;
        }

        public bool isVisable() { return Visable; }

        public abstract void setHeaderText(string newHeaderText);


        public abstract void setBodyText(string newBodyText);
        public string getBodyText()
        {
            return bodyText;
        }

        public void setPosition(Vector2 newPosition)
        {
            Position = newPosition;
        }

        public bool getPauseScreen()
        {
            return pauseScreen;
        }

        //public abstract void LoadContent(ContentManager Content);

        public override void Update(GameTime gameTime)
        {
            foreach (var item in objects)
            {
                item.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in objects)
            {
                item.Draw(spriteBatch);
            }
        }

        //public virtual void Update(GameTime gameTime)
        //{
        //    foreach (var item in objects)
        //    {
        //        item.Update(gameTime);
        //    }
        //}


    }
}
