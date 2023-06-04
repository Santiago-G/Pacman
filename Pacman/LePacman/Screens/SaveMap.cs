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
using LePacman.Screens.MapEditor;

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

        private SpriteFont NameText;

        public static SavedMap currentMap;

        private string MapName;
        private int MapNum;
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

            if (Game1.savedMaps[1].Name != "EMPTY")
            {
                //DO SMTH FOR SLOT ONE
            }
            if (Game1.savedMaps[2].Name != "EMPTY")
            {
                //DO SMTH FOR SLOT TWO
            }
            if (Game1.savedMaps[3].Name != "EMPTY")
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

            NameText = Content.Load<SpriteFont>("MapNameText");
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();


            //do pop up saying "are you sure you want to override this save" when overriding
            if (slotOne.IsClicked(ms))
            {
                slotClicked = true;
                MapNum = 1;
            }
            else if (slotTwo.IsClicked(ms)) 
            {
                slotClicked = true;
                MapNum = 2;
            }
            else if (slotThree.IsClicked(ms))
            {
                slotClicked = true;
                MapNum = 3;
            }

            if (slotClicked) 
            {
                MapName = $"Map{MapNum}";
                currentMap.Name = "Scary Monsters";
                Game1.savedMaps[MapNum] = currentMap;
                string SerializedMap100PercentTrustmebro = $"{JsonConvert.SerializeObject(currentMap.PixelTiles)};{JsonConvert.SerializeObject(currentMap.WallTiles)};" +
                    $"{JsonConvert.SerializeObject(currentMap.Portals.ToPortalDataArray())}";

                File.WriteAllText(MapName + ".json", SerializedMap100PercentTrustmebro);
                slotClicked = false;
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0), Color.DarkGray);
            menuBackground.Draw(spriteBatch);

            base.Draw(spriteBatch);

            spriteBatch.DrawString(NameText, Game1.savedMaps[1].Name, new Vector2(slotOne.Position.X + 105, slotOne.Position.Y + 20), Color.DarkRed);
            spriteBatch.DrawString(NameText, Game1.savedMaps[2].Name, new Vector2(slotTwo.Position.X + 105, slotTwo.Position.Y + 20), Color.DarkRed);
            spriteBatch.DrawString(NameText, Game1.savedMaps[3].Name, new Vector2(slotThree.Position.X + 105, slotThree.Position.Y + 20), Color.DarkRed);
        } 
    }
}
