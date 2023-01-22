using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LePacman
{
    public class ScreenManager
    {
        Dictionary<GameStates, Screen> screens = new Dictionary<GameStates, Screen>()
        {
            [GameStates.TitleScreen] = new TitleScreen(new Point(800), new Vector2(0), graphics),
            [GameStates.MapEditor] = new MapEditor(new Point(1600, 1000), new Vector2(0), graphics),
            [GameStates.Options] = new Options(new Point(800), new Vector2(0), graphics),

        };
        public Screen currentScreen;

        private static GraphicsDeviceManager graphics;



        private ScreenManager(GraphicsDeviceManager Graphics) 
        {
            graphics = Graphics;
        }

        public static ScreenManager Instance { get;} = new ScreenManager(graphics);

        public void LoadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
        }

        public void Draw(SpriteBatch batch)
        {
            currentScreen.Draw(batch);
        }

    }
}
