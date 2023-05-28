using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static LePacman.Screens.MapEditor.MapEditor;
using Newtonsoft.Json;
using System.IO;

namespace LePacman.Screens
{
    public class SaveMap : Screen
    {
        public static Vector2 screenOrigin;

        public static Image menuBackground;

        private static Image headerText;

        private static Button slotOne;
        private static Button slotTwo;
        private static Button slotThree;

        public static SavedMap currentMap;

        private string MapName;
        private bool slotClicked = false;

        public SaveMap(Point Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size , Position, Graphics)
        {
            size = Size;
            position = Position;
            graphics = Graphics;
        }

        public static void setUpPositions()
        {
            menuBackground.Position = screenOrigin;

            headerText.Position += screenOrigin;
            slotOne.Position += screenOrigin;
            slotTwo.Position += screenOrigin;
            slotThree.Position += screenOrigin;

            if (Game1.savedMaps[1] != "EMPTY")
            {
                //DO SMTH FOR SLOT ONE
            }
            if (Game1.savedMaps[2] != "EMPTY")
            {
                //DO SMTH FOR SLOT TWO
            }
            if (Game1.savedMaps[3] != "EMPTY")
            {
                //DO SMTH FOR SLOT THREE
            }
        }

        public override void LoadContent(ContentManager Content)
        {
            menuBackground = new Image(Content.Load<Texture2D>("optionsBackground"), new Vector2(), Color.White);

            headerText = new Image(Content.Load<Texture2D>("saveMapText"), new Vector2(menuBackground.Image.Width/2 - 125, 50), Color.White);
            objects.Add(headerText);

            Texture2D emptySlotImage = Content.Load<Texture2D>("emptyText");

            slotOne = new Button(emptySlotImage, new Vector2(70, 150), Color.White);
            objects.Add(slotOne);
            slotTwo = new Button(emptySlotImage, new Vector2(70, 325), Color.White);
            objects.Add(slotTwo);
            slotThree = new Button(emptySlotImage, new Vector2(70, 500), Color.White);
            objects.Add(slotThree);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            //do pop up saying "are you sure you want to override this save" when overriding
            if (slotOne.IsClicked(ms))
            {
                slotClicked = true;
                MapName = "Map1";
            }
            else if (slotTwo.IsClicked(ms)) 
            {
                slotClicked = true;
                MapName = "MapTwo";
            }
            else if (slotThree.IsClicked(ms))
            {
                slotClicked = true;
                MapName = "MapThree";
            }

            if (slotClicked) 
            {
                string SerializedMap100PercentTrustmebro = "hi";

                File.WriteAllText(MapName + ".json", SerializedMap100PercentTrustmebro);
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0), Color.DarkGray);
            menuBackground.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
