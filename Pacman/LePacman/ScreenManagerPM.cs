using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman;
using Pacman.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LePacman.Screens.MapEditor;
using static Pacman.Options;
using LePacman.Screens;
using LePacman.Screens.MainGame;

namespace LePacman
{
    public class ScreenManagerPM
    {
        static public GraphicsDeviceManager graphics;

        public Dictionary<GameStates, Screen> screens = new Dictionary<GameStates, Screen>()
        {
            [GameStates.TitleScreen] = new TitleScreen(new Point(800), new Vector2(0), graphics),
            [GameStates.MapEditor] = new MapEditor(new Point(1600, 1000), new Vector2(0), graphics),
            [GameStates.Options] = new Options(new Point(800), new Vector2(0), graphics),
            [GameStates.OptionsAudio] = new OptionsAudio(new Point(800), screenOrigin, graphics),
            [GameStates.SaveMap] = new SaveMap(new Point(800, 900), new Vector2(100), graphics),
            [GameStates.MainGame] = new MainGame(new Point(1000, 1000), Vector2.Zero, graphics),
        };
       
        public Screen currentScreen;
        public Screen prevScreen;
        Screen originalScreen;

        KeyboardState prevKeyboardState = Keyboard.GetState();

        private ScreenManagerPM() { }

        public static ScreenManagerPM Instance { get; } = new ScreenManagerPM();

        #region Functions

        public void ChangeScreens(GameStates newScreen)
        {
            prevScreen = currentScreen;
            currentScreen = screens[newScreen];

            if(newScreen == GameStates.OptionsAudio)
            {
                screenOrigin = new Vector2(screens[newScreen].size.X - 400, screens[newScreen].size.Y - 400);
            }

            if (notUpdateScreens(newScreen)) 
            {
                graphics.PreferredBackBufferWidth = screens[newScreen].size.X;
                graphics.PreferredBackBufferHeight = screens[newScreen].size.Y;
                graphics.ApplyChanges();

                screens[GameStates.Options].size = screens[newScreen].size;
                screenOrigin = new Vector2(screens[newScreen].size.X - 700, screens[newScreen].size.Y - 400);
            }
            else if (newScreen == GameStates.SaveMap)
            {
                SaveMap.screenOrigin = new Vector2((screens[GameStates.SaveMap].background.Width / 2) - (menuBackground.Image.Width / 2), (screens[GameStates.SaveMap].background.Height / 2) - (menuBackground.Image.Height / 2));
                SaveMap.setUpPositions();
            }
        }
        public void setGraphics(GraphicsDeviceManager grap)
        {
            graphics = grap;
        }

        private bool notUpdateScreens(GameStates currScreen)
        {
            return (!(currScreen >= GameStates.Options && currScreen <= GameStates.SaveMap));
            
            //return (currScreen != GameStates.Options && currScreen != GameStates.OptionsAudio && currScreen != GameStates.OptionsVisual && currScreen != GameStates.OptionsControl);
        }



        #region Options Functions
        public void SetUpOptionScreens()
        {
            originalScreen = currentScreen;

            GetOptionsBackground();
            ChangeScreens(GameStates.Options);

            screenOrigin = new Vector2((background.Width / 2) - (menuBackground.Image.Width / 2), (background.Height / 2) - (menuBackground.Image.Height / 2));
            menuBackground.Position = screenOrigin;

            setUpPositions();
            OptionsAudio.setUpPositions();
        }
        static void GetOptionsBackground()
        {
            var gd = graphics.GraphicsDevice;

            Color[] colors = new Color[gd.Viewport.Width * gd.Viewport.Height];
            gd.GetBackBufferData(colors);

            background = new Texture2D(gd, gd.Viewport.Width, gd.Viewport.Height);
            background.SetData(colors);
        }

        public Texture2D GetBackgroundImage()
        {
            var gd = graphics.GraphicsDevice;

            Color[] colors = new Color[gd.Viewport.Width * gd.Viewport.Height];
            gd.GetBackBufferData(colors);

            Texture2D backgroundImage = new Texture2D(gd, gd.Viewport.Width, gd.Viewport.Height);
            backgroundImage.SetData(colors);

            return backgroundImage;
        }
        #endregion

        #endregion

        public void LoadContent(ContentManager Content)
        {
            foreach (var screen in screens)
            {
                screen.Value.LoadContent(Content);
            }
            currentScreen = screens[GameStates.TitleScreen];
        }

        public void Update(GameTime GameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && prevKeyboardState != Keyboard.GetState())
            {
                if (currentScreen != screens[GameStates.Options] && currentScreen != screens[GameStates.OptionsAudio])
                {
                    SetUpOptionScreens();
                }
                else
                {
                    currentScreen = originalScreen;
                }
            }

            currentScreen.Update(GameTime);
            prevKeyboardState = Keyboard.GetState();
        }

        public void Draw(SpriteBatch batch)
        {
            currentScreen.Draw(batch);
        }

    }
}
