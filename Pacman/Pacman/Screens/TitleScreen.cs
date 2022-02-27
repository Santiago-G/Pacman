using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    public class TitleScreen : Screen
    {
        Vector2 position;
        GraphicsDeviceManager graphics;

        Image titleImage;
        Texture2D titleSprite;

        Texture2D mapEditorSprite;
        Button mapEditorButton;

        Texture2D playSprite;
        Button playButton;

        Texture2D optionsSprite;
        Button optionsButton;
        public TitleScreen((int width, int height) Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            position = Position;
            graphics = Graphics;
        }

        public override void LoadContent(ContentManager Content)
        {
            titleSprite = Content.Load<Texture2D>("PacManTitle");
            titleImage = new Image(titleSprite, new Vector2(400 - (titleSprite.Width / 2), 100), Color.White);
            objects.Add(titleImage);

            mapEditorSprite = Content.Load<Texture2D>("mapEditorText");
            mapEditorButton = new Button(mapEditorSprite, new Vector2(400 - (mapEditorSprite.Width / 2), 300), Color.White);
            objects.Add(mapEditorButton);

            playSprite = Content.Load<Texture2D>("pressToPlayText");
            playButton = new Button(playSprite, new Vector2(400 - (playSprite.Width / 2), 400), Color.White);
            objects.Add(playButton);

            optionsSprite = Content.Load<Texture2D>("optionsText");
            optionsButton = new Button(optionsSprite, new Vector2(400 - (optionsSprite.Width / 2), 500), Color.White);
            objects.Add(optionsButton);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            if (mapEditorButton.IsClicked(ms))
            {
                Game1.currentScreen = Game1.GameStates.MapEditor;

                graphics.PreferredBackBufferWidth = Game1.screens[Game1.GameStates.MapEditor].size.width;
                graphics.PreferredBackBufferHeight = Game1.screens[Game1.GameStates.MapEditor].size.height;
                graphics.ApplyChanges();
            }

            else if(playButton.IsClicked(ms))
            {
                Game1.currentScreen = Game1.GameStates.MainGame;
            }

            else if (optionsButton.IsClicked(ms))
            {
                Game1.currentScreen = Game1.GameStates.Options;
            }

            base.Update(gameTime);
        }
    }
}
