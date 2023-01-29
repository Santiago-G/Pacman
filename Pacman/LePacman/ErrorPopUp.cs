using LePacman;
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
    public class ErrorPopUp : PopUp
    {
        /*
        public PopUp(Texture2D Background, Point Size, Vector2 Position, SpriteFont HeaderFont, SpriteFont BodyFont, string HeaderText, string BodyText,
            Vector2 HeaderPos, Vector2 bodyPos)
        */

        //check if i really need an abstract pop up class, and if the old one worked

        public ErrorPopUp(Texture2D Background, Point Size, Vector2 Position, SpriteFont HeaderFont, SpriteFont BodyFont, string HeaderText, string BodyText,
            Vector2 HeaderPos, Vector2 BodyPos) : base(Background, Size, Position, HeaderFont, BodyFont, HeaderText, BodyText, HeaderPos, BodyPos)
        {
            
        }

        public ErrorPopUp(Texture2D Image, Vector2 Pos, Color Tint, SpriteFont HeaderFont, SpriteFont BodyFont, string HeaderText, string BodyText, Vector2 HeaderPos, Vector2 BodyPos)
        : base(Image, Pos, Tint)
        {
            headerFont = HeaderFont;
            bodyFont = BodyFont;

            headerText = HeaderText;
            bodyText = BodyText;

            headerPos = HeaderPos;
            bodyPos = BodyPos;
        }

        public ErrorPopUp(Texture2D Image, Vector2 Pos, Color Tint, SpriteFont HeaderFont, SpriteFont BodyFont, string HeaderText, string BodyText, float HeaderY, Vector2 BodyPos)
        : base(Image, Pos, Tint)
        {
            headerFont = HeaderFont;
            bodyFont = BodyFont;

            headerText = HeaderText;
            bodyText = BodyText;

            headerPos = new Vector2(Image.Width /2 - (HeaderFont.MeasureString(HeaderText).X / 2), HeaderY);
            bodyPos = BodyPos;
        }

        public ErrorPopUp(Texture2D Image, Vector2 Pos, Color Tint, SpriteFont HeaderFont, SpriteFont BodyFont, string HeaderText, string BodyText)
        : base(Image, Pos, Tint)
        {
            headerFont = HeaderFont;
            bodyFont = BodyFont;

            headerText = HeaderText;
            bodyText = BodyText;

            headerPos = new Vector2(Image.Width / 2 - (HeaderFont.MeasureString(HeaderText).X / 2), 10);
            bodyPos = new Vector2(20, 40);
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

        public bool isVisable() { return Visable; }

        public void setHeaderText(string newHeaderText)
        {
            headerText = newHeaderText;
        }
        public void setBodyText(string newBodyText)
        {
            string fixedNewBodyText = "";
            string temp = "";

            for(int i = 0; i < newBodyText.Length; i++)
            {
                temp += newBodyText[i];

                if (newBodyText[i] == ' ' || i == newBodyText.Length - 1)
                {
                    if (bodyFont.MeasureString(fixedNewBodyText + temp).X + bodyPos.X < Image.Width - 10)
                    {
                        fixedNewBodyText += temp;
                    }
                    else
                    {
                        fixedNewBodyText = $"{fixedNewBodyText}\n{temp}";
                    }
                    temp = "";
                }
            }

            bodyText = fixedNewBodyText;
        }

        public void setPosition(Vector2 newPosition)
        {
            Position = newPosition;
        }

        public override void Draw(SpriteBatch batch)
        {
            if (Visable)
            {
                batch.Draw(Image, Position, Tint);
                batch.DrawString(headerFont, headerText, Position + headerPos, Tint);
                batch.DrawString(bodyFont, bodyText, Position + bodyPos, Tint);

                Vector2 textSize = bodyFont.MeasureString("hello");
                batch.DrawString(bodyFont, "Click to continue", new Vector2(Position.X + bodyFont.MeasureString("Click to continue").X / 2, Position.Y + (Image.Height - Image.Height / 6)), Tint);
            }
        }
        
    }
}
