using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman.GraphStuff;
using System;
using System.Collections.Generic;

namespace Pacman
{
    public class MapEditor : Screen
    {

        static public SelectedType selectedTileType = SelectedType.Default;

        Vector2 position;

        //Graph<int> graph = new Graph<int>();
        MapEditorTile[,] tiles = new MapEditorTile[31, 28]; //y,x

        Image mapEditorImage;
        Texture2D mapEditorSprite;

        Texture2D pelletButtonSprite;
        Texture2D selectedPelletSprite;
        Button pelletButton;

        Texture2D eraserButtonSprite;
        Texture2D selectedEraserSprite;
        Button eraserButton;

        Texture2D powerPelletButtonSprite;
        Texture2D selectedPowerPelletSprite;
        Button powerPelletButton;

        Texture2D wallButtonSprite;
        Texture2D selectedWallSprite;
        Button wallButton;

        Vector2 globalOffset = new Vector2(40, 90);

        /*  Wall Cases
         * One wall (no neighbors, circle) O
         * Horizontal wall (neighbors left/right ONLY, A line without the fillet)  **
         * Vertical wall (neighbors up/down ONLY, A line without the fillet) |
         * Semicircle wall (edge of horizontal/vertical wall, 180d edge) )
         * Corner wall (neighbors 90 degrees )
         */

        //returns string

        Point PosToIndex(Vector2 Pos)
        {
            Pos.X -= globalOffset.X;
            Pos.Y -= globalOffset.Y;

            int gridX = (int)(Pos.X / tiles[0,0].Hitbox.Width);
            int gridY = (int)(Pos.Y / tiles[0,0].Hitbox.Height);

            if (gridX <= 0 || gridX >= tiles.GetLength(1) || gridY <= 0 || gridY >= tiles.GetLength(0))
            {
                return new Point(-1);
            }

            return new Point(gridX, gridY);
        }

        void UpdateWall(Vector2 TilePos)
        {
            if (PosToIndex(TilePos) == new Point(-1))
            {
                return;
            }

            List<MapEditorTile> Neighbors = new List<MapEditorTile>();
            //y, x

            if (IsValid(new Vector2(TilePos.X - 1, TilePos.Y)))
            {
                Neighbors.Add(tiles[0, 0]);
            }

            /*
            Neighbors.Add(new Vector2(TilePos.X - 1, TilePos.Y)); //left
            Neighbors.Add(new Vector2(TilePos.X, TilePos.Y + 1)); //down
            Neighbors.Add(new Vector2(TilePos.X + 1, TilePos.Y)); //right
            Neighbors.Add(new Vector2(TilePos.X, TilePos.Y - 1)); //up
            */
        }

        private bool IsValid(Vector2 gridIndex)
        {
            return true;
        }

        public MapEditor((int width, int height) Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            size = Size;
        }

        //enum that changes states when selecting walls, pelets, ect...

        public override void LoadContent(ContentManager Content)
        {
            mapEditorSprite = Content.Load<Texture2D>("mapEditorText");
            mapEditorImage = new Image(mapEditorSprite, new Vector2(800 - mapEditorSprite.Width / 2, 10), Color.White);
            objects.Add(mapEditorImage);

            pelletButtonSprite = Content.Load<Texture2D>("UnselectedPelletButton");
            selectedPelletSprite = Content.Load<Texture2D>("PelletButton");
            pelletButton = new Button(pelletButtonSprite, new Vector2(1000, 200), Color.White);
            objects.Add(pelletButton);

            eraserButtonSprite = Content.Load<Texture2D>("eraserButton");
            selectedEraserSprite = Content.Load<Texture2D>("selectedEraserButton");
            eraserButton = new Button(eraserButtonSprite, new Vector2(1000, 300), Color.White);
            objects.Add(eraserButton);

            powerPelletButtonSprite = Content.Load<Texture2D>("powerPelletButton");
            selectedPowerPelletSprite = Content.Load<Texture2D>("selectedPowerPellet");
            powerPelletButton = new Button(powerPelletButtonSprite, new Vector2(1000, 400), Color.White);
            objects.Add(powerPelletButton);

            wallButtonSprite = Content.Load<Texture2D>("wallButton");
            selectedWallSprite = Content.Load<Texture2D>("selectedWallButton");
            wallButton = new Button(wallButtonSprite, new Vector2(1000, 500), Color.White);
            objects.Add(wallButton);

            //ADD THING THAT IF SELECTED ONLY DRAWS OVER BLANK TILES

            

            MapEditorTile.NormalSprite = Content.Load<Texture2D>("mapEditorTile");
            MapEditorTile.NormalEnlargedBorder = Content.Load<Texture2D>("EnlargeBorderTile");
            MapEditorTile.PelletSprite = Content.Load<Texture2D>("mapEditorTile2");
            MapEditorTile.PelletEnlargedBorder = Content.Load<Texture2D>("enlargedPelletTile");
            MapEditorTile.PowerPelletSprite = Content.Load<Texture2D>("powerPelletSprite");
            MapEditorTile.PowerPelletEnlargedBorder = Content.Load<Texture2D>("enlargedPowerPelletSprite");


            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {

                    float realPositionX = x * MapEditorTile.NormalSprite.Width + globalOffset.X;
                    float realPositionY = y * MapEditorTile.NormalSprite.Height + globalOffset.Y;
                    tiles[y, x] = new MapEditorTile(MapEditorTile.NormalSprite, new Vector2(realPositionX, realPositionY), Color.White);
                    tiles[y, x].Cord = new Point(y, x);
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

            //use the 2d array to find the tile's neighbors.
            // like when you place a tile, check if it's neighbors are walls, if they are change the texture accordingly

            foreach (var tile in tiles)
            {
                tile.Update(gameTime);
            }

            if (pelletButton.IsClicked(ms))
            {
                if (selectedTileType != SelectedType.Pellet)
                {
                    selectedTileType = SelectedType.Pellet;
                    pelletButton.Image = selectedPelletSprite;

                    eraserButton.Image = eraserButtonSprite;
                    powerPelletButton.Image = powerPelletButtonSprite;
                    wallButton.Image = wallButtonSprite;
                }
                else
                {
                    selectedTileType = SelectedType.Default;
                    pelletButton.Image = pelletButtonSprite;
                }
            }

            else if (eraserButton.IsClicked(ms))
            {
                if (selectedTileType != SelectedType.Eraser)
                {
                    selectedTileType = SelectedType.Eraser;
                    eraserButton.Image = selectedEraserSprite;

                    pelletButton.Image = pelletButtonSprite;
                    powerPelletButton.Image = powerPelletButtonSprite;
                    wallButton.Image = wallButtonSprite;
                }
                else
                {
                    selectedTileType = SelectedType.Default;
                    eraserButton.Image = eraserButtonSprite;
                }
            }

            else if (powerPelletButton.IsClicked(ms))
            {
                if (selectedTileType != SelectedType.PowerPellet)
                {
                    selectedTileType = SelectedType.PowerPellet;
                    powerPelletButton.Image = selectedPowerPelletSprite;

                    pelletButton.Image = pelletButtonSprite;
                    eraserButton.Image = eraserButtonSprite;
                    wallButton.Image = wallButtonSprite;
                }
                else
                {
                    selectedTileType = SelectedType.Default;
                    powerPelletButton.Image = powerPelletButtonSprite;
                }
            }

            else if (wallButton.IsClicked(ms))
            {
                if (selectedTileType != SelectedType.Wall)
                {
                    selectedTileType = SelectedType.Wall;
                    wallButton.Image = selectedWallSprite;

                    pelletButton.Image = pelletButtonSprite;
                    eraserButton.Image = eraserButtonSprite;
                    powerPelletButton.Image = powerPelletButtonSprite;
                }
                else
                {
                    selectedTileType = SelectedType.Default;
                    wallButton.Image = wallButtonSprite;
                }
            }

            if (selectedTileType == SelectedType.Wall)
            {
                
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
