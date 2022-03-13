using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman.GraphStuff;

namespace Pacman
{
    public class MapEditor : Screen
    {

        static public SelectedType selectedTileType = SelectedType.Default;

        Vector2 position;

        Graph<int> graph = new Graph<int>();
        MapEditorTile[,] tiles = new MapEditorTile[31, 28]; //y,x

        Image mapEditorImage;
        Texture2D mapEditorSprite;

        Texture2D pelletButtonSprite;
        Button pelletButton;

        public MapEditor((int width, int height) Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            size = Size;
        }

        //enum that changes states when selecting walls, pelets, ect...

        public override void LoadContent(ContentManager Content)
        {
            mapEditorSprite = Content.Load<Texture2D>("mapEditorText");
            mapEditorImage = new Image(mapEditorSprite, new Vector2(800 - mapEditorSprite.Width/2 , 10), Color.White);
            objects.Add(mapEditorImage);

            pelletButtonSprite = Content.Load<Texture2D>("PelletButton");
            pelletButton = new Button(pelletButtonSprite, new Vector2(400, 400), Color.White);

            Vector2 globalOffset = new Vector2(20, 90);

            MapEditorTile.NormalSprite = Content.Load<Texture2D>("mapEditorTile");
            MapEditorTile.NormalEnlargedBorder = Content.Load<Texture2D>("EnlargeBorderTile");
            MapEditorTile.PelletSprite = Content.Load<Texture2D>("mapEditorTile2");
            MapEditorTile.PelletEnlargedBorder = Content.Load<Texture2D>("enlargedPelletTile");

            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    float realPositionX = x * MapEditorTile.NormalSprite.Width + globalOffset.X;
                    float realPositionY = y * MapEditorTile.NormalSprite.Height + globalOffset.Y;
                    tiles[y, x] = new MapEditorTile(MapEditorTile.NormalSprite, new Vector2(realPositionX, realPositionY), Color.White);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            //If mouse is clicked
            //Calculate position in terms of x and y
            //Call on tile that corresponds to that position
            //Aadianjhd function could take in the selectedTileType

            foreach (var tile in tiles)
            {
                tile.Update(gameTime);
            }
                
            if (pelletButton.IsClicked(ms) && selectedTileType != SelectedType.Pellet)
            {
                selectedTileType = SelectedType.Pellet;
            }
            else if(selectedTileType == SelectedType.Pellet)
            {
                selectedTileType = SelectedType.Default;
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
