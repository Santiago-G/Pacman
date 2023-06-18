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
using LePacman.Screens.MainGame;
using MonoGame.Extended.BitmapFonts;

namespace LePacman.Screens
{
    public class SaveMap : Screen
    {
        #region Objects
        public static Vector2 screenOrigin;

        public static Sprite menuBackground;

        private static Sprite headerText;

        private static Button slotOne;
        private static Button slotTwo;
        private static Button slotThree;

        private static Button clearSlotOne;
        private static Button clearSlotTwo;
        private static Button clearSlotThree;

        private static Button playButton;

        private SpriteFont NameText;

        public static SavedMap currentMap;

        private string MapName;
        private int MapNum;
        private bool slotClicked = false;
        private bool playMap = false;

        static Random gen = new Random();
        private static string[] names = new string[] { "Scary Monsters", "Super Creeps", "Soma", "Helden", "Cracked Actor", "Filibuster", "Pringles Pope"};
        private static Color[] colors = new Color[] { Color.White, Color.White, Color.White };

        #endregion

        public SaveMap(Point Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
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

            clearSlotOne.Position += screenOrigin;
            clearSlotTwo.Position += screenOrigin;
            clearSlotThree.Position += screenOrigin;

            playButton.Position += screenOrigin;

            setUpMapNames(1);
            setUpMapNames(2);
            setUpMapNames(3);
        }

        private static void setUpMapNames(int mapNum)
        {
            string mapName = $"Map{mapNum}.json";
            if (File.Exists(mapName))
            {
                FileInfo mapInfo = new FileInfo(mapName);

                if (mapInfo.Length > 40)
                {
                    Game1.savedMaps[mapNum].Name = names[gen.Next(0, names.Length)];
                    colors[mapNum - 1] = Color.DarkRed;
                }
            }
        }

        public override void LoadContent(ContentManager Content)
        {
            menuBackground = new Sprite(Content.Load<Texture2D>("optionsBackground"), new Vector2(), Color.White);

            headerText = new Sprite(Content.Load<Texture2D>("saveMapText"), new Vector2(menuBackground.Image.Width / 2 - 125, 50), Color.White);
            objects.Add(headerText);

            Texture2D emptySlotImage = Content.Load<Texture2D>("emptyText");
            Texture2D veryCoolRedButton = Content.Load<Texture2D>("coolRedButton");

            slotOne = new Button(emptySlotImage, new Vector2(70, 150), Color.White);
            objects.Add(slotOne);
            slotTwo = new Button(emptySlotImage, new Vector2(70, 325), Color.White);
            objects.Add(slotTwo);
            slotThree = new Button(emptySlotImage, new Vector2(70, 500), Color.White);
            objects.Add(slotThree);

            clearSlotOne = new Button(veryCoolRedButton, new Vector2(560, 150), Color.White);
            objects.Add(clearSlotOne);
            clearSlotTwo = new Button(veryCoolRedButton, new Vector2(560, 325), Color.White);
            objects.Add(clearSlotTwo);
            clearSlotThree = new Button(veryCoolRedButton, new Vector2(560, 500), Color.White);
            objects.Add(clearSlotThree);

            playButton = new Button(Content.Load<Texture2D>("emptyText2"), new Vector2(255, 650), Color.White);

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
                objects.Add(playButton);
                playMap = true;

                MapName = $"Map{MapNum}";
                currentMap.Name = names[gen.Next(0, names.Length)];
                Game1.savedMaps[MapNum].Name = currentMap.Name;
                colors[MapNum - 1] = Color.DarkRed;

                string SerializedMap100PercentTrustmebro = $"{JsonConvert.SerializeObject(currentMap.PixelTiles)};{JsonConvert.SerializeObject(currentMap.WallTiles)};" +
                    $"{JsonConvert.SerializeObject(currentMap.Portals.ToPortalDataArray())}";

                File.WriteAllText(MapName + ".json", SerializedMap100PercentTrustmebro);
                slotClicked = false;
            }

            if (clearSlotOne.IsClicked(ms))
            {
                File.WriteAllText("Map1.json", "");
                colors[0] = Color.White;
                Game1.savedMaps[1].Name = "Empty";
            }
            else if (clearSlotTwo.IsClicked(ms))
            {
                File.WriteAllText("Map2.json", "");
                colors[1] = Color.White;
                Game1.savedMaps[2].Name = "Empty";
            }
            else if (clearSlotThree.IsClicked(ms))
            {
                File.WriteAllText("Map3.json", "");
                colors[2] = Color.White;
                Game1.savedMaps[3].Name = "Empty";
            }

            if (playButton.IsClicked(ms) && playMap)
            {
                ScreenManagerPM.Instance.ChangeScreens(GameStates.MainGame);
                MainGame.MainGame.LoadMap();
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0), Color.DarkGray);
            menuBackground.Draw(spriteBatch);

            base.Draw(spriteBatch);

            if (playMap)
            {
                spriteBatch.DrawString(NameText, "Play", new Vector2(playButton.Position.X + 64, playButton.Position.Y + 5), Color.Green);
            }

            spriteBatch.DrawString(NameText, Game1.savedMaps[1].Name, new Vector2(slotOne.Position.X + 105, slotOne.Position.Y + 20), colors[0]);
            spriteBatch.DrawString(NameText, Game1.savedMaps[2].Name, new Vector2(slotTwo.Position.X + 105, slotTwo.Position.Y + 20), colors[1]);
            spriteBatch.DrawString(NameText, Game1.savedMaps[3].Name, new Vector2(slotThree.Position.X + 105, slotThree.Position.Y + 20), colors[2]);
        }
    }
}
