using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.Screens
{
    class TestScreen
    {
        /*
        MapEditorVisualTile originFloat;
        MapEditorVisualTile originInt;

        Texture2D pixel;


        //We know that it has nothing to do with position/other things drawing ontop
        //We know that it works with a pixel of a SOLID color
        //Possibly issue with the texture itself

        public TestScreen((int width, int height) Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            pixel = new Texture2D(Graphics.GraphicsDevice, 25, 25);
            var colorScheme = Enumerable.Repeat(Color.DarkGreen, 25 * 25 / 2).Concat(Enumerable.Repeat(Color.Blue, 25 * 25 - 25 * 25 / 2)).ToArray();
            pixel.SetData(colorScheme);
        }

        //public override void LoadContent(ContentManager Content)
        //{
        //    MapEditorVisualTile.VertiWallTile = Content.Load<Texture2D>("test2");

        //    originFloat = new MapEditorVisualTile(MapEditorVisualTile.VertiWallTile, new Point(1, 1), Color.White, Vector2.Zero, Vector2.One, new Vector2(MapEditorVisualTile.VertiWallTile.Width / 2f, MapEditorVisualTile.VertiWallTile.Height / 2f), 0f, SpriteEffects.None);
        //    originFloat.TileStates = States.Wall;
        //    originFloat.WallStates = WallStates.Verti;
        //    originFloat.CurrentImage = MapEditorVisualTile.VertiWallTile;
        //    originFloat.prevImage = MapEditorVisualTile.VertiWallTile;


        //    originInt = new MapEditorVisualTile(MapEditorVisualTile.VertiWallTile, new Point(3, 1), Color.White, Vector2.Zero, Vector2.One, new Vector2(MapEditorVisualTile.VertiWallTile.Width / 2, MapEditorVisualTile.VertiWallTile.Height / 2), 0f, SpriteEffects.None);
        //    originInt.TileStates = States.Wall;
        //    originInt.WallStates = WallStates.Verti;
        //    originInt.CurrentImage = MapEditorVisualTile.VertiWallTile;
        //    originInt.prevImage = MapEditorVisualTile.VertiWallTile;
        //}

        public override void Update(GameTime gameTime)
        {
            originInt.Update(gameTime);
            originFloat.Update(gameTime);
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            originInt.Draw(spriteBatch);
            originFloat.Draw(spriteBatch);
        }

        public override void LoadContent(ContentManager Content)
        {
            throw new NotImplementedException();
        }
        */
    }
}
