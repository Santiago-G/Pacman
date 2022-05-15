using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman.GraphStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pacman.Dummies;

namespace Pacman
{
    public class MapEditor : Screen
    {

        static public SelectedType selectedTileType = SelectedType.Default;

        Vector2 position;

        //Graph<int> graph = new Graph<int>();
        MapEditorVisualTile[,] tiles = new MapEditorVisualTile[31, 28]; //y,x

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


        void addWall(Vector2 MousePosition)
        {
            //check if it's out of bounds

            Point index = PosToIndex(MousePosition);

            if (index == new Point(-1)) return;

            recursivePlaceholderName(tiles[index.Y, index.X]);
        }

        void recursivePlaceholderName(MapEditorVisualTile currentTile)
        {
            bool updated = UpdateWall(currentTile.Position);

            if (!updated) return;

            currentTile.UpdateWalls();

            foreach (var neighbor in currentTile.Neighbors)
            {
                if (neighbor.isWall)
                {
                    recursivePlaceholderName(tiles[neighbor.Index.Y, neighbor.Index.X]);
                }
            }
        }

        Point PosToIndex(Vector2 Pos)
        {
            Pos.X -= globalOffset.X;
            Pos.Y -= globalOffset.Y;

            int gridX = (int)(Pos.X / tiles[0, 0].Hitbox.Width);
            int gridY = (int)(Pos.Y / tiles[0, 0].Hitbox.Height);

            Point retPoint = new Point(gridX, gridY);
            if (!IsValid(retPoint)) return new Point(-1);
            return retPoint;
        }

        bool UpdateWall(Vector2 Position)
        {
            return UpdateWall(PosToIndex(Position));
        }
        bool UpdateWall(Point tileIndex)
        {
            if (tileIndex == new Point(-1))
            {
                return false;
            }
            //y, x

            MapEditorVisualTile currentTile = tiles[tileIndex.Y, tileIndex.X];

            WallStates oldState = currentTile.wallStates;

            for (int i = 0; i < offsets.Length; i++)
            {
                var newPosition = new Point(tileIndex.X + offsets[i].X, tileIndex.Y + offsets[i].Y);

                currentTile.Neighbors[i].isWall = IsValid(newPosition) && tiles[newPosition.Y, newPosition.X].TileStates == States.Wall;
                currentTile.Neighbors[i].Index = newPosition;
            }

            //0, 1, 2
            //7, *, 3
            //6, 5, 4

            if (currentTile.Neighbors[1].isWall && currentTile.Neighbors[3].isWall && currentTile.Neighbors[5].isWall && currentTile.Neighbors[7].isWall && currentTile.Neighbors[0].isWall && currentTile.Neighbors[2].isWall && currentTile.Neighbors[4].isWall && currentTile.Neighbors[6].isWall)
            {
                tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.InteriorWall;
                return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
            }

            if ((!currentTile.Neighbors[1].isWall && !currentTile.Neighbors[3].isWall && !currentTile.Neighbors[5].isWall && !currentTile.Neighbors[7].isWall))
            {
                tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.LoneWall;
                return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
            }

            if (currentTile.Neighbors[1].isWall && currentTile.Neighbors[3].isWall && currentTile.Neighbors[5].isWall && currentTile.Neighbors[7].isWall)
            {
                //interior center
                tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.InteriorCorner;
                return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
            }



            if (currentTile.Neighbors[3].isWall && currentTile.Neighbors[7].isWall)
            {
                //horizontal
                if (!currentTile.Neighbors[1].isWall && !currentTile.Neighbors[5].isWall)
                {
                    //middle horiz
                    tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.Horiz;
                    return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
                }
                else
                {
                    //Crosses (T)
                    if (!currentTile.Neighbors[0].isWall && !currentTile.Neighbors[2].isWall && !currentTile.Neighbors[4].isWall && !currentTile.Neighbors[6].isWall)
                    {
                        if (currentTile.Neighbors[1].isWall)
                        {
                            //Top T
                            tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.TopCross;
                            return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
                        }

                        tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.BottomCross;
                        return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
                    }

                    //Top Edge
                    if (currentTile.Neighbors[6].isWall && currentTile.Neighbors[5].isWall && currentTile.Neighbors[4].isWall)
                    {
                        tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.TopEdge;
                        return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
                    }

                    //Bottom Edge
                    if (currentTile.Neighbors[0].isWall && currentTile.Neighbors[1].isWall && currentTile.Neighbors[2].isWall)
                    {
                        tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.BottomEdge;
                        return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
                    }
                }
            }
            if (!currentTile.Neighbors[1].isWall && !currentTile.Neighbors[5].isWall)
            {
                if (currentTile.Neighbors[7].isWall)
                {
                    //right horiz
                    tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.HorizRightEnd;
                    return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
                }
                //left horiz
                tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.HorizLeftEnd;
                return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
            }

            if (currentTile.Neighbors[1].isWall && currentTile.Neighbors[5].isWall)
            {
                if (!currentTile.Neighbors[7].isWall && !currentTile.Neighbors[3].isWall)
                {
                    //middle verti
                    tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.Verti;
                    return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
                }
                else 
                {
                    //Crosses (T)
                    if (!currentTile.Neighbors[0].isWall && !currentTile.Neighbors[2].isWall && !currentTile.Neighbors[4].isWall && !currentTile.Neighbors[6].isWall)
                    {
                        if (currentTile.Neighbors[7].isWall)
                        {
                            //Left T
                            tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.LeftCross;
                            return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
                        }

                        tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.RightCross;
                        return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
                    }

                    //Left Edge
                    if (currentTile.Neighbors[2].isWall && currentTile.Neighbors[3].isWall && currentTile.Neighbors[4].isWall)
                    {
                        tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.LeftEdge;
                        return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
                    }
                    //Right Edge
                    if (currentTile.Neighbors[0].isWall && currentTile.Neighbors[7].isWall && currentTile.Neighbors[6].isWall)
                    {
                        tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.RightEdge;
                        return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
                    }
                }

            }
            if (!currentTile.Neighbors[7].isWall && !currentTile.Neighbors[3].isWall)
            {
                if (currentTile.Neighbors[1].isWall)
                {
                    //bottom horiz
                    tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.VertiBottomEnd;
                    return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
                }
                //top horiz
                tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.VertiTopEnd;
                return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
            }


            //Bottom Left Corner
            if (currentTile.Neighbors[1].isWall && currentTile.Neighbors[3].isWall)
            {
                if (currentTile.Neighbors[2].isWall)
                {
                    //corner without 2nd wall
                    tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.BottomLeftCornerFilled;
                    return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
                }

                tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.BottomLeftCorner;
                return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
            }

            //Bottom Right Corner
            if (currentTile.Neighbors[1].isWall && currentTile.Neighbors[7].isWall)
            {
                if (currentTile.Neighbors[0].isWall)
                {
                    //corner without 2nd wall
                    tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.BottomRightCornerFilled;
                    return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
                }

                tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.BottomRightCorner;
                return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
            }

            //Top Right Corner
            if (currentTile.Neighbors[7].isWall && currentTile.Neighbors[5].isWall)
            {
                if (currentTile.Neighbors[6].isWall)
                {
                    //corner without 2nd wall
                    tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.TopRightCornerFilled;
                    return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
                }

                tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.TopRightCorner;
                return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
            }

            //Top Left Corner
            if (currentTile.Neighbors[3].isWall && currentTile.Neighbors[5].isWall)
            {
                if (currentTile.Neighbors[4].isWall)
                {
                    tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.TopLeftCornerFilled;
                    return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
                }

                tiles[tileIndex.Y, tileIndex.X].wallStates = WallStates.TopLeftCorner;
                return oldState != tiles[tileIndex.Y, tileIndex.X].wallStates;
            }

            //cross

            return false;
            //else if (neighboringWalls[1] && neighboringWalls[3] && neighboringWalls[5] && neighboringWalls[7])
            //{
            //    tiles[tileIndex.Y, tileIndex.X].wallStates = MapEditorTile.WallStates.InteriorCorner;
            //}
        }

        private bool IsValid(Point gridIndex)
        {
            return gridIndex.X >= 0 && gridIndex.X < tiles.GetLength(1) && gridIndex.Y >= 0 && gridIndex.Y < tiles.GetLength(0);
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



            MapEditorVisualTile.NormalSprite = Content.Load<Texture2D>("mapEditorTile");
            MapEditorVisualTile.NormalEnlargedBorder = Content.Load<Texture2D>("EnlargeBorderTile");
            MapEditorVisualTile.PelletSprite = Content.Load<Texture2D>("mapEditorTile2");
            MapEditorVisualTile.PelletEnlargedBorder = Content.Load<Texture2D>("enlargedPelletTile");
            MapEditorVisualTile.PowerPelletSprite = Content.Load<Texture2D>("powerPelletSprite");
            MapEditorVisualTile.PowerPelletEnlargedBorder = Content.Load<Texture2D>("enlargedPowerPelletSprite");

            MapEditorVisualTile.LoneWallTile = Content.Load<Texture2D>("loneWall");
            MapEditorVisualTile.InteriorWall = Content.Load<Texture2D>("InteriorWall");

            MapEditorVisualTile.HorizWallTile = Content.Load<Texture2D>("horizWall");
            MapEditorVisualTile.HorizLeftWallTile = Content.Load<Texture2D>("horizLeftWall");
            MapEditorVisualTile.HorizRightWallTile = Content.Load<Texture2D>("horizRightWall");

            MapEditorVisualTile.VertiWallTile = Content.Load<Texture2D>("VertiWall");
            MapEditorVisualTile.VertiTopWallTile = Content.Load<Texture2D>("VertiTopWall");
            MapEditorVisualTile.VertiBottomWallTile = Content.Load<Texture2D>("VertiBottomWall");

            MapEditorVisualTile.TopLeftWallTile = Content.Load<Texture2D>("TopLeftCorner");
            MapEditorVisualTile.TopRightWallTile = Content.Load<Texture2D>("TopRightCorner");
            MapEditorVisualTile.BottomLeftWallTile = Content.Load<Texture2D>("BottomLeftCorner");
            MapEditorVisualTile.BottomRightWallTile = Content.Load<Texture2D>("BottomRightCorner");

            MapEditorVisualTile.TopLeftFilledWallTile = Content.Load<Texture2D>("TopLeftCornerFilled");
            MapEditorVisualTile.TopRightFilledWallTile = Content.Load<Texture2D>("TopRightCornerFilled");
            MapEditorVisualTile.BottomRightFilledWallTile = Content.Load<Texture2D>("BottomRightCornerFilled");
            MapEditorVisualTile.BottomLeftFilledWallTile = Content.Load<Texture2D>("BottomLeftCornerFilled");

            MapEditorVisualTile.TopEdge = Content.Load<Texture2D>("TopEdge");
            MapEditorVisualTile.RightEdge = Content.Load<Texture2D>("RightEdge");
            MapEditorVisualTile.BottomEdge = Content.Load<Texture2D>("BottomEdge");
            MapEditorVisualTile.LeftEdge = Content.Load<Texture2D>("LeftEdge");

            MapEditorVisualTile.InteriorCross = Content.Load<Texture2D>("Cross");

            MapEditorVisualTile.TopCross = Content.Load<Texture2D>("topCross");
            MapEditorVisualTile.RightCross = Content.Load<Texture2D>("rightCross");
            MapEditorVisualTile.BottomCross = Content.Load<Texture2D>("bottomCross");
            MapEditorVisualTile.LeftCross = Content.Load<Texture2D>("leftCross");


            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {

                    float realPositionX = x * MapEditorVisualTile.NormalSprite.Width + globalOffset.X;
                    float realPositionY = y * MapEditorVisualTile.NormalSprite.Height + globalOffset.Y;
                    tiles[y, x] = new MapEditorVisualTile(MapEditorVisualTile.NormalSprite, new Vector2(realPositionX, realPositionY), Color.White);
                    tiles[y, x].Cord = new Point(y, x);

                    for (int i = 0; i < offsets.Length; i++)
                    {
                        tiles[y, x].Neighbors[i].Index = new Point(y + offsets[i].Y, x + offsets[i].X);
                    }
                }
            }
        }

        MouseState prevms;
        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            KeyboardState kb = Keyboard.GetState();


            if (kb.IsKeyDown(Keys.Space)) 
            {
                for (int row = 0; row < tiles.GetLength(0); row++)
                {
                    for (int col = 0; col < tiles.GetLength(1); col++)
                    {
                        
                    }
                }
                ;
            }

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
                    //Recursive function that takes in a tile
                    //Update that tile with the update wall function
                    //If nothing changed return
                    //Loop through its neighbors
                    //Call that recursive function on each neighbor

                    addWall(new Vector2(ms.Position.X, ms.Position.Y));
                    ;
                }
            }

            prevms = ms;

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
