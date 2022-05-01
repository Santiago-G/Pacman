using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman.GraphStuff;
using System;
using System.Collections.Generic;
using System.Linq;

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

        Point[] offsets = new Point[] { new Point(-1, -1), new Point(0, -1), new Point(1, -1), new Point(1, 0), new Point(1, 1), new Point(0, 1), new Point(-1, 1), new Point(-1, 0) };

        /*  Wall Cases
         * One wall (no neighbors, circle) O
         * Horizontal wall (neighbors left/right ONLY, A line without the fillet)  **
         * Vertical wall (neighbors up/down ONLY, A line without the fillet) |
         * Semicircle wall (edge of horizontal/vertical wall, 180d edge) )
         * Corner wall (neighbors 90 degrees)
         */

        //returns string

        Point PosToIndex(Vector2 Pos)
        {
            Pos.X -= globalOffset.X;
            Pos.Y -= globalOffset.Y;

            int gridX = (int)(Pos.X / tiles[0, 0].Hitbox.Width);
            int gridY = (int)(Pos.Y / tiles[0, 0].Hitbox.Height);

            if (gridX <= 0 || gridX >= tiles.GetLength(1) || gridY <= 0 || gridY >= tiles.GetLength(0))
            {
                return new Point(-1);
            }

            return new Point(gridX, gridY);
        }

        void UpdateWall(Vector2 TilePos)
        {
            Point tileIndex = PosToIndex(TilePos);

            if (tileIndex == new Point(-1))
            {
                return;
            }

            bool[] neighboringWalls = new bool[offsets.Length];
            //y, x

            for (int i = 0; i < offsets.Length; i++)
            {
                var newPosition = new Point(tileIndex.X + offsets[i].X, tileIndex.Y + offsets[i].Y);

                neighboringWalls[i] = IsValid(newPosition) && tiles[newPosition.Y, newPosition.X].TileStates == MapEditorTile.States.Wall;
            }

            //0, 1, 2
            //7, *, 3
            //6, 5, 4

            if (neighboringWalls.Count(x => x) == neighboringWalls.Length)
            {
                tiles[tileIndex.Y, tileIndex.X].wallStates = MapEditorTile.WallStates.InteriorWall;
            }
            else
            {
                if (!(neighboringWalls[1] && neighboringWalls[3] && neighboringWalls[5] && neighboringWalls[7]))
                {
                    tiles[tileIndex.Y, tileIndex.X].wallStates = MapEditorTile.WallStates.LoneWall;
                }
                else
                {
                    //Horizontal
                    if (!neighboringWalls[1] && !neighboringWalls[5])
                    {
                        if (neighboringWalls[7] && neighboringWalls[3])
                        {
                            //middle horiz
                            tiles[tileIndex.Y, tileIndex.X].wallStates = MapEditorTile.WallStates.Horiz;
                            return;
                        }
                        else if (neighboringWalls[7])
                        {
                            //right horiz
                            tiles[tileIndex.Y, tileIndex.X].wallStates = MapEditorTile.WallStates.HorizRightEnd;
                            return;
                        }
                        else
                        {
                            //left horiz
                            tiles[tileIndex.Y, tileIndex.X].wallStates = MapEditorTile.WallStates.HorizLeftEnd;
                            return;
                        }
                    }

                    //Vertical
                    else if (!neighboringWalls[7] && !neighboringWalls[3])
                    {
                        if (neighboringWalls[1] && neighboringWalls[5])
                        {
                            //middle verti
                            tiles[tileIndex.Y, tileIndex.X].wallStates = MapEditorTile.WallStates.Verti;
                        }
                        else if (neighboringWalls[1])
                        {
                            //bottom horiz
                            tiles[tileIndex.Y, tileIndex.X].wallStates = MapEditorTile.WallStates.VertiBottomEnd;
                        }
                        else 
                        {
                            //top horiz
                            tiles[tileIndex.Y, tileIndex.X].wallStates = MapEditorTile.WallStates.VertiTopEnd;
                        }
                    }

                    //cross
                    if (neighboringWalls[1] && neighboringWalls[3] && neighboringWalls[5] && neighboringWalls[7])
                    {
                        //interior center
                        tiles[tileIndex.Y, tileIndex.X].wallStates = MapEditorTile.WallStates.InteriorCorner;
                    }

                    
                }


                //else if (neighboringWalls[1] && neighboringWalls[3] && neighboringWalls[5] && neighboringWalls[7])
                //{
                //    tiles[tileIndex.Y, tileIndex.X].wallStates = MapEditorTile.WallStates.InteriorCorner;
                //}
            }
        }

        private bool IsValid(Point gridIndex)
        {
            return !(gridIndex.X < 0 || gridIndex.X >= tiles.GetLength(1)) || (gridIndex.Y < 0 || gridIndex.Y >= tiles.GetLength(0));
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

            MapEditorTile.LoneWallTile = Content.Load<Texture2D>("loneWall");

            MapEditorTile.HorizWallTile = Content.Load<Texture2D>("horizWall");
            MapEditorTile.HorizLeftWallTile = Content.Load<Texture2D>("horizLeftWall");




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
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    UpdateWall(new Vector2(ms.Position.X, ms.Position.Y));

                     //call update wall on it's neighbors too
                     a
                }
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
