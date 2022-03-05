using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman.GraphStuff;

namespace Pacman
{
    public class MapEditor : Screen
    {
        Vector2 position;
        GraphicsDeviceManager graphics;

        Graph<int> graph = new Graph<int>();
        Tile[,] tiles = new Tile[31, 28]; //y,x

        Image mapEditorImage;
        Texture2D mapEditorSprite;

        public MapEditor((int width, int height) Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            size = Size;
            position = Position;
            graphics = Graphics;
        }

        //enum that changes states when selecting walls, pelets, ect...

        public override void LoadContent(ContentManager Content)
        {
            mapEditorSprite = Content.Load<Texture2D>("mapEditorText");
            mapEditorImage = new Image(mapEditorSprite, new Vector2(800 - mapEditorSprite.Width/2 , 10), Color.White);
            objects.Add(mapEditorImage);

            Vector2 globalOffset = new Vector2(20, 90);

            Tile.NormalImage = Content.Load<Texture2D>("NormalTileImage");
            Tile.EnlargedBorder = Content.Load<Texture2D>("EnlargedBorderTile");
            Tile.MapEditorTile = Content.Load<Texture2D>("mapEditorTile");
            Tile.PelletTile = Content.Load<Texture2D>("mapEditorTile2");


            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    float realPositionX = x * Tile.MapEditorTile.Width + globalOffset.X;
                    float realPositionY = y * Tile.MapEditorTile.Height + globalOffset.Y;
                    tiles[y, x] = new Tile(Tile.MapEditorTile, new Vector2(realPositionX, realPositionY), Color.White, true);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var tile in tiles)
            {
                tile.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in tiles)
            {
                tile.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }


    }
}
