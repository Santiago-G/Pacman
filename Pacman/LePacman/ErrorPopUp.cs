using LePacman;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using System;
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

        List<wallVisual> invalidWalls;

        public ErrorPopUp(Texture2D Background, Point Size, Vector2 Position, SpriteFont HeaderFont, SpriteFont BodyFont, string HeaderText, string BodyText,
            Vector2 HeaderPos, Vector2 BodyPos, List<wallVisual> InvalidWalls) : base(Background, Size, Position, HeaderFont, BodyFont, HeaderText, BodyText, HeaderPos, BodyPos)
        {
            setVisable(false);
            pauseScreen = true;
            invalidWalls = InvalidWalls;
        }

        public ErrorPopUp(Texture2D Background, Point Size, Vector2 Position, SpriteFont HeaderFont, SpriteFont BodyFont, string HeaderText, string BodyText, List<wallVisual> InvalidWalls)
            : base(Background, Size, Position, HeaderFont, BodyFont, HeaderText, BodyText, Vector2.One, Vector2.One)
        {
            setVisable(false);
            pauseScreen = true;

            headerPos = new Vector2(Image.Width / 2 - (HeaderFont.MeasureString(HeaderText).X / 2), 10);
            bodyPos = new Vector2(20, 40);
            invalidWalls = InvalidWalls;
        }

        #region Functions
        public override void setHeaderText(string newHeaderText)
        {
            headerText = newHeaderText;
        }
        public override void setBodyText(string newBodyText)
        {
            string fixedNewBodyText = "";
            string temp = "";

            for (int i = 0; i < newBodyText.Length; i++)
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


        #endregion

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            if (Hitbox.Contains(ms.Position))
            {
                if (Game1.prevMS.RightButton != ms.RightButton && ms.RightButton == ButtonState.Pressed) 
                {
                    PopUpManager.Instance.ClearQueue();
                    Visable = false;
                }
                else if (Game1.prevMS.LeftButton != ms.LeftButton && ms.LeftButton == ButtonState.Pressed)
                {
                    PopUpManager.Instance.DequeuePopUp();
                    Visable = false;
                }
            }
        }

        public override void enqueuingMisc()
        {
            foreach (var tile in invalidWalls)
            {
                tile.WallState |= (WallStates)Math.Pow(2, 10);
                tile.UpdateStates();
            }
        }

        public override void dequeuingMisc()
        {
            foreach (var wall in invalidWalls)
            {
                wall.WallState &= ~(WallStates)Math.Pow(2, 10);
                wall.UpdateStates();
            }
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

        /*
        public PopUp(Texture2D Background, Point Size, Vector2 Position, SpriteFont HeaderFont, SpriteFont BodyFont, string HeaderText, string BodyText,
            Vector2 HeaderPos, Vector2 bodyPos)
        

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
        */

    }
}
