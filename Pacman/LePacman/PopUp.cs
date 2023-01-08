using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Pacman
{
    public class PopUp : Sprite
    {
        private SpriteFont headerFont;
        private SpriteFont bodyFont;

        private string headerText;
        private string bodyText;

        private Vector2 headerPos;
        private Vector2 bodyPos;

        private bool Visable = false;


        public PopUp(Texture2D Image, Vector2 Pos, Color Tint, SpriteFont HeaderFont, SpriteFont BodyFont, string HeaderText, string BodyText, Vector2 HeaderPos, Vector2 BodyPos) 
        : base(Image, Pos, Tint)
        {
            headerFont = HeaderFont;
            bodyFont = BodyFont;

            headerText = HeaderText;
            bodyText = BodyText;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            if (ms.LeftButton == ButtonState.Pressed && Hitbox.Contains(ms.Position))
            {
                Visable = false;
            }
        }

        public void setVisable(bool newVisable)
        {
            Visable = newVisable;
        }

        public void setHeaderText(string newHeaderText)
        {
            headerText = newHeaderText;
        }
        public void setBodyText(string newBodyText)
        {
            headerText = newBodyText;
        }

        public override void Draw(SpriteBatch batch)
        {
            if(Visable)
            {
                batch.Draw(Image, Position, Tint);
                batch.DrawString(headerFont, headerText, Position + headerPos, Tint);
                batch.DrawString(bodyFont, bodyText, Position + bodyPos, Tint);

                Vector2 textSize = bodyFont.MeasureString("hello");
                batch.DrawString(bodyFont, "Click to continue", new Vector2(Position.X + bodyFont.MeasureString("Click to continue").X/2, Position.Y + (Image.Height - Image.Height / 8)), Tint);
            }
        }
    }
}
