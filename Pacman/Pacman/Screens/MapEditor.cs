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

        MapEditorGrid Grid;
        Vector2 GridOffest = new Vector2(40, 90);
        Point GridSize = new Point(28, 31);


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

            Grid = new MapEditorGrid(GridSize, new Point(MapEditorVisualTile.NormalSprite.Width, MapEditorVisualTile.NormalSprite.Height), GridOffest);
        }

        MouseState prevms;
        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.Space))
            {
                string stringified = JsonConvert.SerializeObject(Grid.Tiles.Flatten().Select(tile => tile.Data));
                System.IO.File.WriteAllText("SavedMap.json", stringified);
            }

            if (kb.IsKeyDown(Keys.M))
            {
                string content = System.IO.File.ReadAllText("SavedMap.json");
                List<MapEditorDataTile> flattenedTiles = JsonConvert.DeserializeObject<List<MapEditorDataTile>>(content);
                //tiles = flattenedTiles.Select(x => new MapEditorVisualTile(.To2DArray(/*maybe needs parameters*/);
                //Create grid based on this two d array
                Grid.LoadGrid(flattenedTiles);
                ;
            }

            //If mouse is clicked
            //Call on tile that corresponds to that position
            //Calculate position in terms of x and y
            //Aadianjhd function could take in the selectedTileType

            //use the 2d array to find the tile's neighbors.
            // like when you place a tile, check if it's neighbors are walls, if they are change the texture accordingly

            Grid.Update(gameTime);

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
                    Grid.addWall(new Vector2(ms.Position.X, ms.Position.Y));
                }
            }

            prevms = ms;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Grid.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }


    }
}
