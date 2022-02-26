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
        (int, int) size;
        Vector2 position;
        GraphicsDeviceManager graphics;

        Texture2D titleSprite;

        Texture2D mapEditorSprite;
        Button mapEditorButton;

        Texture2D playSprite;
        Button playButton;

        Texture2D optionsSprite;
        Button optionsButton;

        Texture2D arrow;
        Vector2 arrowPos;

        public TitleScreen((int, int) Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            size = Size;
            position = Position;
            graphics = Graphics;
        }

        public override void LoadContent(ContentManager Content)
        {
            titleSprite = Content.Load<Texture2D>("PacManTitle");

            mapEditorSprite = Content.Load<Texture2D>("mapEditorText");
            mapEditorButton = new Button(mapEditorSprite, new Vector2(400 - (mapEditorSprite.Width / 2), 300), Color.White);

            playSprite = Content.Load<Texture2D>("pressToPlayText");
            playButton = new Button(playSprite, new Vector2(400 - (playSprite.Width / 2), 400), Color.White);

            optionsSprite = Content.Load<Texture2D>("optionsText");
            optionsButton = new Button(optionsSprite, new Vector2(400 - (optionsSprite.Width / 2), 500), Color.White);

            arrow = Content.Load<Texture2D>("arrow");

            //add all your objects to the list in Screen
            
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            if (mapEditorButton.IsClicked(ms))
            {
                Game1.currentScreen = Game1.GameStates.MapEditor;
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

        public o void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(titleSprite, new Vector2(400 - (titleSprite.Width/2), 100), Color.White);
            mapEditorButton.Draw(spriteBatch);
            playButton.Draw(spriteBatch);
            optionsButton.Draw(spriteBatch);

            //spriteBatch.Draw(arrow, arrowPos, Color.White);
        }


    }
}
